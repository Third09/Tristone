"""
Writes the "Schema reference" appendix of docs/_source/Database Guide.md from the live database, so the guide always
matches construction_db exactly. Run after applying a new numbered script:

    python docs/_tools/dbref/gen_schema.py

Reads the database with psql (password from %APPDATA%\\postgresql\\pgpass.conf) and replaces everything between the
markers <!-- schema:start --> and <!-- schema:end --> in the guide.
"""
import os
import re
import subprocess
import sys

PSQL = r"C:\Program Files\PostgreSQL\18\bin\psql.exe"
GUIDE = os.path.join(os.path.dirname(__file__), "..", "..", "_source", "Database Guide.md")

# Tables in reading order, grouped like the app's parts.
GROUPS = [
    ("Login, accounts and history (scripts 001, 004, 011)", ["users", "login_logs", "password_requests", "audit_logs"]),
    ("Projects (002)", ["projects"]),
    ("HR: employees and attendance (003, 012)", ["employees", "employee_rates", "attendance_imports", "attendance_scans", "attendance_days"]),
    ("HR: payroll (013)", ["holidays", "cash_advances", "payroll_runs", "payslips", "cash_advance_deductions"]),
    ("Inventory (005)", ["materials", "material_usages", "vendors", "vendor_documents", "quotes", "purchases", "deliveries", "equipment", "maintenance_logs"]),
    ("Subcontractors (006)", ["subcontractors", "subcontractor_trades", "sub_jobs", "sub_job_stages", "sub_job_documents", "sub_progress", "sub_vouchers", "sub_payouts"]),
    ("Receipts (007)", ["receipts", "change_orders"]),
    ("Website (010)", ["inquiries"]),
    ("System", ["app_settings", "schema_versions"]),
]


def query(sql):
    out = subprocess.run([PSQL, "-w", "-h", "localhost", "-U", "postgres", "-d", "construction_db", "-At", "-F", "\t", "-c", sql],
                         capture_output=True, text=True, encoding="utf-8", check=True).stdout
    return [line.split("\t") for line in out.splitlines() if line]


def readable(definition):
    """CHECK (((status)::text = ANY ((ARRAY['A'::character varying, 'B'::character varying])::text[])))  ->  CHECK (status IN ('A', 'B'))"""
    d = re.sub(r"::(character varying|text|numeric|integer|date)(\[\])?", "", definition)
    d = re.sub(r"= ANY \(\(?ARRAY\[([^\]]*)\]\)?\)", r"IN (\1)", d)
    d = re.sub(r"<> ALL \(\(?ARRAY\[([^\]]*)\]\)?\)", r"NOT IN (\1)", d)
    d = re.sub(r"(?<!\w)\((\w+)\)", r"\1", d)          # (status) -> status, (0) -> 0; keeps users(user_id)
    for _ in range(4):
        d = re.sub(r"\(\(([^()]*)\)\)", r"(\1)", d)     # drop doubled brackets
    return d


def type_name(data_type, length, precision, scale):
    return {
        "character varying": f"varchar({length})" if length else "varchar",
        "numeric": f"numeric({precision},{scale})" if precision else "numeric",
        "timestamp without time zone": "timestamp",
        "time without time zone": "time",
        "integer": "int",
    }.get(data_type, data_type)


def main():
    columns = query("""select table_name, column_name, data_type, coalesce(character_maximum_length::text,''),
                              coalesce(numeric_precision::text,''), coalesce(numeric_scale::text,''), is_nullable, coalesce(column_default,'')
                       from information_schema.columns where table_schema='public' order by table_name, ordinal_position""")
    constraints = query("""select conrelid::regclass::text, conname, contype, pg_get_constraintdef(oid)
                           from pg_constraint where connamespace='public'::regnamespace and contype in ('p','f','u','c')
                           order by 1, position(contype in 'pufc'), 2""")
    indexes = query("""select i.tablename, i.indexname, i.indexdef from pg_indexes i
                       where i.schemaname='public' and not exists (select 1 from pg_constraint c where c.conname = i.indexname)
                       order by 1, 2""")
    tables = sorted({c[0] for c in columns})
    listed = [t for _, ts in GROUPS for t in ts]
    missing = [t for t in tables if t not in listed]
    if missing:
        GROUPS.append(("Other", missing))

    out = ["Generated from the live database by `docs/_tools/dbref/gen_schema.py`. "
           f"{len(tables)} tables. Types: `int` = 32-bit integer (`serial` = auto-numbered), `numeric(p,s)` = exact decimal, "
           "`timestamp` = date and time without time zone, `time` = time of day. \"Null\" = may be empty.", ""]
    for title, group in GROUPS:
        out.append(f"## {title}")
        out.append("")
        for t in group:
            cols = [c for c in columns if c[0] == t]
            if not cols:
                continue
            out.append(f"### {t}")
            out.append("")
            out.append("| Column | Type | Null | Default |")
            out.append("|---|---|---|---|")
            for _, name, dt, ln, pr, sc, nullable, default in cols:
                typ = type_name(dt, ln, pr, sc)
                if default.startswith("nextval("):
                    typ, default = "serial", ""
                default = re.sub(r"::[a-z ]+(\(\d+\))?", "", default).replace("|", "\\|")
                out.append(f"| `{name}` | {typ} | {'yes' if nullable == 'YES' else ''} | {default} |")
            out.append("")
            rules = [c for c in constraints if c[0] == t]
            if rules:
                kind = {"p": "Primary key", "f": "Foreign key", "u": "Unique", "c": "Check"}
                for _, name, ct, definition in rules:
                    definition = readable(definition).replace("|", "\\|")
                    out.append(f"- **{kind[ct]}** `{name}`: `{definition}`")
            for _, name, definition in [i for i in indexes if i[0] == t]:
                definition = readable(re.sub(r"^CREATE (UNIQUE )?INDEX \S+ ON public\.\S+ USING btree ", "", definition)).replace("|", "\\|")
                unique = "Unique index" if "UNIQUE" in [i for i in indexes if i[1] == name][0][2] else "Index"
                out.append(f"- **{unique}** `{name}`: `{definition}`")
            out.append("")

    text = open(GUIDE, encoding="utf-8").read()
    start, end = "<!-- schema:start -->", "<!-- schema:end -->"
    if start not in text or end not in text:
        sys.exit("Markers not found in the Database Guide.")
    before, rest = text.split(start, 1)
    _, after = rest.split(end, 1)
    open(GUIDE, "w", encoding="utf-8", newline="").write(before + start + "\n\n" + "\n".join(out) + "\n" + end + after)
    print(f"schema reference written: {len(tables)} tables")


if __name__ == "__main__":
    main()
