# Tri-Stone Database: Technical Guide

How `construction_db` is designed, how it is built and changed, how the application reads and writes it, and which tables each page of the system uses. Appendix A is the complete schema reference, generated from the live database.

> PostgreSQL 18.6, database `construction_db`, encoding UTF8, extensions `plpgsql` and `pgcrypto`. Scripts 001-013 applied: **37 tables** (35 application tables + `app_settings` + `schema_versions`).

# 1. Architecture

```
 Windows PC (x5 at deployment)                                   Server (localhost while building; 192.168.1.50 later)
+-------------------------------------------+                  +------------------------------------+
| TriStone.exe (Company.Launcher)           |                  | PostgreSQL 18                      |
|  Company.OfficeApp / Company.WebsiteAdmin |   TCP 5432       |  construction_db (37 tables)       |
|   Pages (WinForms UserControls)           | ---------------> |                                    |
|    -> Services (rules, permission checks) |   Npgsql         | Shared folder (scans, photos)      |
|      -> AppDbContext (EF Core 10, Npgsql) | <--------------- |  path in app_settings              |
+-------------------------------------------+                  +------------------------------------+
                                                                  ^
 Public website (Netlify) --HTTPS--> Company.Api (later) ---------+   (the website never connects directly)
```

- **Direct connection.** The desktop app talks to PostgreSQL itself through Npgsql; there is no application server in between. The ASP.NET Core API exists only for the public website, which must never hold database credentials.
- **Connection settings per PC.** `C:\ProgramData\TriStone\OfficeApp\dbsettings.json` holds host, port, database, user and the password encrypted with Windows DPAPI (LocalMachine scope), so the file is useless if copied to another PC. `DbSettings.ConnectionString` builds an Npgsql connection string with a 5-second connect timeout; Npgsql's connection pooling is on by default.
- **One context per unit of work.** Every service method opens its own context (`using var db = Db.Open();`), does its reads and writes, calls `SaveChangesAsync`, and disposes it. Contexts are never shared between screens or kept open, so there are no stale tracked entities and no long-lived connections. Read-only queries use `AsNoTracking()`.
- **EF Core maps, scripts define.** Tables are created only by the numbered SQL scripts (chapter 2). `AppDbContext` maps one C# class per table (`Company.Shared/Models`, with `[Table]` and `[Column]` attributes) and declares keys; EF Core migrations are not used. This keeps the database design in plain SQL that can be read, reviewed and run on any server.
- **Files are not stored in the database.** Uploaded scans and photos are copied into a storage folder (`app_settings.file_storage_path`, default `C:\TriStoneFiles`, a shared folder on the server later) under `<area>\yyyy\MM\<random>_<name>`; tables keep only the relative path and the original file name.

# 2. How the database is built

## 2.1 The migration runner

`database\setup-database.ps1` (double-click `setup-database.cmd`) is the only way the schema changes:

1. Finds `psql.exe` (PATH, then the newest `C:\Program Files\PostgreSQL\*\bin`).
2. Connects to the `postgres` database and creates `construction_db` if it doesn't exist.
3. Creates `schema_versions (version INT PRIMARY KEY, script_name, applied_at)` if needed and reads which versions are applied.
4. For every file named `NNN_*.sql` in order, if `NNN` isn't in `schema_versions`: runs it with `psql -1 -v ON_ERROR_STOP=1 -X -f`, then inserts the version row.

`-1` wraps the whole script in one transaction and `ON_ERROR_STOP` aborts at the first error, so a script either applies completely or not at all; `-X` ignores any local `psqlrc`. The password comes from `%APPDATA%\postgresql\pgpass.conf`. Parameters `-DbHost`, `-Port`, `-Database`, `-User` point it at another server (the same scripts will build the production database in Phase 6).

Scripts are also written to be **re-runnable** (`CREATE TABLE IF NOT EXISTS`, `CREATE INDEX IF NOT EXISTS`, `ON CONFLICT DO NOTHING` for seed rows), so applying one twice by hand does no harm.

**Rules:** a script that has been applied is never edited; every change is a new script with the next number; a script that moves data (011) copies first and drops last, inside its single transaction.

## 2.2 Script history

| Script | Changes |
|---|---|
| 001_login_security | `users`, `login_logs`, `password_requests`, `audit_logs`; enables `pgcrypto` (used to hash the first Admin's password in SQL) |
| 002_projects | `projects` |
| 003_employees | `employees`, `employee_rates`; `users.employee_id` → `employees` |
| 004_one_login_per_employee | partial unique index `ux_users_employee` (one login per employee, old unlinked logins allowed) |
| 005_inventory | `app_settings` (seeds `file_storage_path`), `materials`, `material_usages`, `vendors`, `vendor_documents`, `quotes`, `purchases`, `deliveries`, `equipment`, `maintenance_logs` |
| 006_subcontractors | `subcontractors`, `subcontractor_trades`, `sub_jobs`, `sub_job_stages`, `sub_job_documents`, `sub_progress`, `sub_vouchers`, `sub_payouts` |
| 007_receipts | `receipts`, `change_orders` |
| 008_website_accounts | `website_accounts`, `website_login_logs` (a separate account list for the Website Admin app; superseded by 011) |
| 009_website_main_account | `website_accounts.is_main` (superseded by 011) |
| 010_inquiries | `inquiries` (website Contact form messages) |
| 011_merge_website_accounts | adds the Website Admin level to `users` (new CHECK rules), copies website accounts with their hashes into `users`, their sign-ins into `login_logs`, re-points `inquiries.updated_by` to `users`, then drops `website_accounts` and `website_login_logs` |
| 012_attendance | `attendance_imports`, `attendance_scans`, `attendance_days` |
| 013_payroll | `holidays`, `cash_advances`, `payroll_runs`, `payslips`, `cash_advance_deductions` |

## 2.3 Design conventions

| Topic | Convention | Why |
|---|---|---|
| Names | `snake_case`, tables plural (`sub_jobs`), primary key `<entity>_id` | Readable SQL; matches the C# `[Column]` names |
| Keys | `SERIAL` (int) surrogate keys; natural keys only where the value *is* the identity (`holidays.holiday_date`, `app_settings.key`, `(subcontractor_id, trade)`) | IDs never change, names do |
| Money | `NUMERIC(14,2)` for totals and contract amounts, `NUMERIC(12,2)` for prices and quantities, `NUMERIC(10,2)` daily rates | Exact decimal arithmetic; mapped to C# `decimal` by a convention in `AppDbContext` |
| Percentages | `NUMERIC(5,2)` (`retention_pct`, stage `percent`) | 0.00 to 100.00 exactly |
| Dates | `DATE` for calendar days (C# `DateOnly`); `TIME` for clock times (`TimeOnly`); `TIMESTAMP` without time zone for events | One office, one time zone. `AppDbContext.ConfigureConventions` maps every `DateTime` to `timestamp without time zone` (Npgsql otherwise expects UTC `timestamptz`) |
| Enumerations | `VARCHAR` + `CHECK (x IN (...))` rather than PostgreSQL enum types | Adding a value is one `ALTER TABLE ... DROP/ADD CONSTRAINT` in a new script; readable in any tool |
| Free text | `VARCHAR(n)` with a sensible limit; `TEXT` only for settings values and import problem lists | The limit is also checked by the service before saving |
| JSON | `JSONB` only in `audit_logs.old_values/new_values` | The changed fields differ per table |
| Who / when | `created_by`, `recorded_by`, `uploaded_by`, `prepared_by`, `requested_by`, `imported_by`, `computed_by` → `users`; `decided_by` / `approved_by` / `handled_by` / `finalized_by` → `users`; `*_at` timestamps | Accountability on every record, beside the audit log |
| Deleting | Foreign keys use the default `NO ACTION` (deleting a referenced row fails). The only `ON DELETE CASCADE` is `subcontractor_trades` | History is never lost by accident; the app offers a status instead (Inactive, Cancelled, Retired, Rejected) |

## 2.4 Integrity: what the database enforces itself

Every rule that protects money or history is enforced by the database, not only by the screens, so a bug or a manual edit can't break it. The services check the same rules first to show a friendly message.

**Cross-column CHECK rules**

| Table | Rule |
|---|---|
| `users` | Admin / Website Admin ⇔ `department IS NULL`; Supervisor / Employee ⇔ department set |
| `employees` | `department = 'Site'` ⇔ `pay_schedule = 'Site'`; every other department ⇔ `Office` |
| `projects` | `target_end_date` and `completed_date` not before `start_date`; `contract_price >= 0` |
| `deliveries` | `physical_only` with `filed_location`, or not physical with `receipt_file_path` |
| `equipment` | `Owned`, or `Rented` with company, rate and rate unit |
| `receipts` | `expense_type = 'Project'` ⇔ `project_id IS NOT NULL`; exactly one kind of copy: scan, paper + location, or `no_receipt` + reason (with a receipt number unless `no_receipt`) |
| `change_orders` | `cost_change <> 0 OR days_change <> 0` |
| `sub_jobs` | `end_date >= start_date`; `retention_pct` 0 to under 100; `contract_amount > 0` |
| `sub_vouchers` | `kind = 'Stage'` ⇔ `stage_id IS NOT NULL` |
| `payroll_runs` | `period_to >= period_from` |
| `payslips`, `cash_advances` | `net_pay >= 0`; `balance >= 0` |
| `inquiries` | `email IS NOT NULL OR phone IS NOT NULL` |

**Uniqueness, including partial (filtered) unique indexes**

| Rule | Implemented as |
|---|---|
| One login per employee, old unlinked logins allowed | `ux_users_employee ON users (employee_id) WHERE employee_id IS NOT NULL` |
| One live voucher per stage; a rejected one can be redone | `ux_sub_vouchers_stage ON sub_vouchers (stage_id) WHERE status <> 'Rejected'` |
| One live retention voucher per job | `ux_sub_vouchers_retention ON sub_vouchers (job_id) WHERE kind = 'Retention' AND status <> 'Rejected'` |
| A scan is stored once | `UNIQUE (employee_id, scan_time)` on `attendance_scans` |
| One attendance row per person per day | `UNIQUE (employee_id, work_date)` |
| One payroll per group and period; one payslip per person per run | `UNIQUE (pay_group, period_from)`, `UNIQUE (run_id, employee_id)` |
| A quote becomes at most one purchase; a voucher is paid once | `purchases.quote_id UNIQUE`, `sub_payouts.voucher_id UNIQUE` |
| Document numbers | `sub_vouchers.voucher_no` (SV-0001), `change_orders.co_no` (CO-001) |
| Names | `users.username`, `materials.name`, `vendors.name`, `subcontractors.business_name`, `employees.biometric_id` |

Usernames are unique ignoring capital letters: the service compares `lower(username)` before insert.

## 2.5 Indexes

Every primary key and unique constraint has its own index. The extra indexes follow the queries the screens actually run:

| Index | Serves |
|---|---|
| `ix_login_logs_logged_at` (DESC), `ix_login_logs_user` | Dashboard Log History, newest first; a user's sign-ins |
| `ix_audit_logs_dept_time (department, changed_at DESC)` | Each department's Edit History tab |
| `ix_material_usages_project`, `ix_receipts_project`, `ix_change_orders_project` (all `WHERE status = 'Approved'`) | `ProjectFinance`: only approved rows count, so the partial index stays small |
| `ix_sub_jobs_project`, `ix_sub_payouts_job` | Payouts per project (join through `sub_jobs`) |
| `ix_receipts_date`, `ix_attendance_days_date` | Date-range filters (vault, attendance, payroll, reports) |
| `ix_password_requests_waiting` (`WHERE status = 'Waiting'`) | The Admin's "Password requests (N)" badge |
| `ix_projects_status`, `ix_projects_website` (`WHERE show_on_website`), `ix_employees_status`, `ix_inquiries_status`, `ix_inquiries_received` | List filters |

## 2.6 Transactions and concurrency

- Each `SaveChangesAsync` runs in one transaction. Records that must change together are saved in the same call: approving material usage updates `materials.stock_qty`, sets `material_usages.cost` and writes the audit row together; a delivery inserts the row and raises the stock together.
- **Explicit transactions** where several saves must be atomic: payroll Finalize (`PayrollService.FinalizeAsync`) recomputes the payslips, writes `cash_advance_deductions`, lowers balances and marks the run Final inside `BeginTransactionAsync` / `CommitAsync`.
- **Document numbers** (SV-0001, CO-001) are taken as "last number + 1". The unique constraint is the safety net: if two people create one at the same instant, the second save fails instead of duplicating, and can simply be repeated.
- **Edits are last-write-wins** (no row-version column). Each department has one Employee and one Supervisor, and every edit asks Yes/No with the list of changes, so simultaneous edits of the same record are unlikely; the audit log shows both if they happen.
- Status changes check the current status inside the same save (`if (u.Status != Waiting) return "already ..."`), so approving twice or approving a rejected item is refused.

## 2.7 Security in the data

- **Passwords and security answers** are bcrypt hashes (cost 11, `BCrypt.Net-Next` in the app; `crypt(..., gen_salt('bf', 11))` from `pgcrypto` for the first Admin). Answers are trimmed and lower-cased before hashing.
- **Private values never reach `audit_logs`:** password hashes, security answers, SSS/PhilHealth/Pag-IBIG/TIN numbers and bank account numbers. Snapshots record `*_on_file: true/false` instead.
- **Phase 6:** port 5432 open only to the office LAN (`ufw` + `pg_hba.conf`); the app connects with its own role limited to `SELECT/INSERT/UPDATE/DELETE` on the application tables (no DDL), not `postgres`; the API gets an even smaller role; nightly `pg_dump` backups copied off the server.

# 3. Entity map

`projects` and `employees` are the two hubs; `users` is referenced by every "who" column (omitted below).

```
LOGINS                 users --(employee_id, 0..1)--> employees
                       login_logs, password_requests, audit_logs --> users

PROJECT MONEY          projects <--- material_usages ---> materials
                       projects <--- receipts
                       projects <--- change_orders
                       projects <--- sub_jobs ---> subcontractors <--- subcontractor_trades
                                       ^--- sub_job_stages <--- sub_vouchers <--- sub_payouts
                                       ^--- sub_job_documents, sub_progress

PURCHASING             vendors <--- vendor_documents
                       quotes ---> materials, vendors
                       purchases ---> quotes (1:1), materials, vendors
                       deliveries ---> purchases (optional), materials, vendors
                       equipment <--- maintenance_logs

HR                     employees <--- employee_rates           (rate history)
                       employees <--- attendance_scans ---> attendance_imports
                       employees <--- attendance_days
                       employees <--- cash_advances <--- cash_advance_deductions ---> payslips
                       payroll_runs <--- payslips ---> employees
                       holidays                                 (no links)

WEBSITE                inquiries --(updated_by)--> users
SYSTEM                 app_settings, schema_versions
```

All links are one-to-many except `purchases.quote_id` and `sub_payouts.voucher_id` (one-to-one, enforced by `UNIQUE`) and `users.employee_id` (one-to-one, by the partial unique index).

# 4. How each module's data behaves

## 4.1 Accounts and history

- **`users.status`**: Active → Locked (5 failed passwords; `failed_login_count` counts, reset on success) → Active (Admin unlock); Active ⇄ Disabled (Admin, or the employee marked Inactive). `last_login_at` is set on sign-in.
- **`login_logs`**: append-only; `action` ∈ Login, Logout, Idle Sign Out, Failed Login, Account Locked, Account Unlocked, Password Changed, Password Request. `username_tried` keeps unknown usernames; `computer_name` is the PC.
- **`password_requests`**: Waiting → Done (Admin set a password) / Dismissed.
- **`audit_logs`**: one row per add/edit/delete, written by `AuditService.Add` in the same `SaveChanges` as the change. `department` decides which Edit History tab shows it (NULL = Admin-only records such as projects and users). `old_values` / `new_values` are JSON snapshots; the Edit History tab flattens them into one row per changed field.

```json
{"old_values": {"status": "Waiting"}, "new_values": {"status": "Approved", "cost": 5000.00, "stock_after": 80}}
```

## 4.2 Projects and project money

A project's money is derived, never stored. `ProjectFinance.ForProjectsAsync` computes it with grouped sums:

```sql
SELECT p.project_id, p.project_name,
       p.contract_price + COALESCE(co.total, 0)                        AS revenue,
       COALESCE(mu.total, 0) + COALESCE(r.total, 0) + COALESCE(po.total, 0) AS expenses
FROM projects p
LEFT JOIN (SELECT project_id, SUM(cost_change) total FROM change_orders   WHERE status = 'Approved' GROUP BY 1) co USING (project_id)
LEFT JOIN (SELECT project_id, SUM(cost) total        FROM material_usages WHERE status = 'Approved' GROUP BY 1) mu USING (project_id)
LEFT JOIN (SELECT project_id, SUM(amount) total      FROM receipts        WHERE status = 'Approved' AND project_id IS NOT NULL GROUP BY 1) r USING (project_id)
LEFT JOIN (SELECT j.project_id, SUM(o.amount) total  FROM sub_payouts o JOIN sub_jobs j USING (job_id) GROUP BY 1) po USING (project_id);
```

`completed_date` is set by the service when `status` becomes Done and cleared if it changes back. A project can be deleted only while `material_usages`, `sub_jobs`, `receipts` and `change_orders` have no rows for it (`ProjectService.CountLinkedRecordsAsync`); the foreign keys would refuse it anyway.

**Dashboard (completed-contract method).** Revenue and project expenses count for projects with `status = 'Done'` and `completed_date` in the period; overhead counts by its own date: approved `receipts` with `expense_type = 'Overhead'` (`purchase_date`) and `maintenance_logs.cost` (`done_on`). Department views keep only that department's sources (Inventory = materials + repairs; Subcontractors = payouts; Receipts = receipts).

## 4.3 HR: employees and rate history

`employee_rates` is a dated history; the rate on a day is the row with the latest `effective_from` not after it:

```sql
SELECT daily_rate FROM employee_rates
WHERE employee_id = $1 AND effective_from <= $2
ORDER BY effective_from DESC LIMIT 1;
```

Past payslips therefore keep the rate that applied. Marking an employee Inactive sets every linked login to Disabled in the same save.

## 4.4 HR: attendance pipeline

```
All Report.xls --ScannerFile.Read--> (scanner ID, date+time)[] --match biometric_id--> attendance_scans (unique)
                                                                                         |
                    holidays, hr.* settings, payroll locks --> RecomputeAsync --> attendance_days (one row per person per day)
```

1. **Import** (`AttendanceService.ImportAsync`): one `attendance_imports` row (counts and the rejected-rows text). Scanner IDs are matched to `employees.biometric_id` ignoring leading zeros. New scans are inserted; ones already present (same employee, same minute) are skipped, which is what makes re-uploading safe.
2. **Recompute** for the file's date range, for every Active employee with a Biometric ID: scans of a day → `AttendanceRules.FromScans` (repeat scans within 3 minutes merged, slots in / break out / break in / out assigned, late / early / overtime minutes, status); a work day with no scans, before today, not a holiday, on or after the employee's first rate date → Absent; otherwise any non-fixed row is removed.
3. **Never overwritten:** rows with `fixed_by` set (HR's manual fix) and days inside a **finalized payroll period** for the employee's pay group.
4. `ot_status` becomes Waiting when overtime minutes appear and returns to Waiting if they change; a decision (Approved / Rejected) stays while the minutes stay the same.

## 4.5 HR: payroll

```
payroll_runs (Draft) --Compute--> payslips (replaced each time)
payroll_runs (Draft) --Finalize--> payslips (final) + cash_advance_deductions + cash_advances.balance down + status Final
```

- **Periods** (`PayPeriods`): Office 26th-10th paid the 15th and 11th-25th paid the 30th (February: last day); Site Saturday-Friday paid Saturday; a Sunday or holiday payday moves back a day.
- **Per payslip** (`PayrollService.FillAsync`): `basic_pay` = Σ rate(day) over paid days (Present, Late, or fixed and not Absent); `holiday_pay` = rate on each Regular holiday that is a work day the employee didn't work; `late_deduction` = Σ (late + early minutes) × rate(day) / 480; `ot_pay` = approved overtime minutes × `hr.ot_rate` / 60; `gross_pay` = basic + holiday + overtime − late; government shares only when `employees.gov_deductions`; cash advances (oldest first) take `MIN(per_payday, balance, pay left)`; `net_pay` = what is left (≥ 0).
- **Finalize** is refused while the period has Missing Scan rows without a fix or overtime still Waiting. Once Final, the run can't be computed again and its attendance days are locked (4.4).

## 4.6 Inventory

- **Stock is a ledger result, not an input.** `materials.stock_qty` is set once at creation, then changes only when a `material_usages` row is approved (down) or a `deliveries` row is inserted (up), in the same save.
- **Cost is frozen on approval:** `material_usages.cost` = quantity × `materials.unit_price` at that moment; a later price change doesn't alter project expenses.
- **Buying chain:** `quotes` (Open) → approve one → `purchases` row (1:1 with the quote), the other open quotes of that material become Not Selected → `deliveries` (optionally against the purchase; delivered quantity = Σ deliveries per purchase) → the delivered price becomes `materials.unit_price`.

## 4.7 Subcontractors

- **Stage amounts** = `contract_amount × percent / 100`, rounded to centavos, with the last stage taking the remainder so the stages sum exactly to the contract. Stages must total 100% (checked by the service).
- **Voucher lifecycle:** stage reached (`reached_on`) → voucher Waiting (`gross_amount`, `retention_amount = gross × retention_pct`, `payable_amount = gross − retention`) → Approved / Rejected → Paid (a `sub_payouts` row for exactly `payable_amount`). Once a stage is reached or a voucher exists, amount, retention and stages are locked.
- **Retention** held = Σ `retention_amount` of paid stage vouchers; released by one Retention voucher once the latest `sub_progress.percent` is 100 and every stage is paid.

## 4.8 Receipts and change orders

- A receipt counts as an expense only when `status = 'Approved'`; Project receipts go to the project, Overhead ones to the Dashboard. `no_receipt` rows are listed in Needs Review; attaching a receipt later clears the flag and keeps the decision.
- **Stock purchases** are not copied into `receipts`: the Receipt Vault unions `deliveries` into its list (marked "Stock purchase"), so they're stored once and never counted as project expenses (materials count when used).
- **Change orders:** approving (Admin) adds `cost_change` to revenue through `ProjectFinance` and moves `projects.target_end_date` by `days_change` in the same save.

## 4.9 Website and settings

- `inquiries`: New → Contacted → Closed, internal `notes`, never deleted. The website will insert rows through the API.
- `app_settings` (key/value): `file_storage_path`; `hr.work_start`, `hr.work_end`, `hr.break_start`, `hr.break_end` (HH:mm), `hr.grace_minutes`, `hr.work_days` (0 = Sunday ... 6 = Saturday, comma-separated), `hr.ot_rate`, `hr.ot_minimum_minutes`, `hr.work_days_per_month`, `hr.sss_rate`, `hr.sss_min_credit`, `hr.sss_max_credit`, `hr.philhealth_rate`, `hr.philhealth_floor`, `hr.philhealth_ceiling`, `hr.pagibig_rate`, `hr.pagibig_cap`, `hr.withholding_tax`. A missing key means "use the default in the code" (`HrSettings.Default`).

# 5. Pages and the data they use

R = reads, W = writes. Every write also inserts into `audit_logs`, except sign-in events (which go to `login_logs`).

| Page / tab | Service | Reads | Writes |
|---|---|---|---|
| Database Settings | `DbSettings` | (test connection) | `dbsettings.json` on the PC |
| Sign In, idle sign-out | `AuthService` | `users` | `users.failed_login_count/status/last_login_at`, `login_logs` |
| Forgot Password | `PasswordService` | `users` | `password_requests`, `login_logs`; Admins: `users.password_hash` |
| My Account | `AccountService` | `users` | `users.password_hash`, `security_question`, `security_answer_hash` |
| User Accounts | `UserAdminService` | `users`, `employees` | `users` |
| Dashboard | `DashboardService`, `ProjectFinance` | `projects`, money tables (4.2), `maintenance_logs`, `login_logs` | — |
| Projects | `ProjectService`, `ProjectFinance` | `projects` + money tables | `projects` |
| HR > Employees | `EmployeeService` | `employees`, `employee_rates` | `employees`, `employee_rates`, linked `users.status` |
| HR > Attendance | `AttendanceService`, `ScannerFile`, `ScannerEmployeeFile` | scanner file, `employees`, `holidays`, `payroll_runs`, `app_settings` | `attendance_imports`, `attendance_scans`, `attendance_days` |
| HR > Payroll | `PayrollService`, `PayPeriods` | `employees`, `employee_rates`, `attendance_days`, `holidays`, `cash_advances`, `app_settings` | `payroll_runs`, `payslips`; on Finalize `cash_advance_deductions`, `cash_advances` |
| HR > Payslip History | `PayrollService` | `payslips`, `payroll_runs`, `employees`, `cash_advance_deductions` | — |
| HR > Cash Advances | `CashAdvanceService` | `cash_advances`, `cash_advance_deductions` | `cash_advances` |
| HR > Settings | `HrSettingsService` | `app_settings`, `holidays` | `app_settings`, `holidays`; recomputes `attendance_days` |
| Inventory > Materials / Usage | `MaterialService` | `materials`, `material_usages`, `projects` | `materials`, `material_usages` |
| Inventory > Deliveries / Price Comparison / Vendors | `PurchasingService` | `vendors`, `vendor_documents`, `quotes`, `purchases`, `deliveries` | the same, plus `materials.stock_qty/unit_price` |
| Inventory > Equipment | `EquipmentService` | `equipment`, `maintenance_logs` | the same |
| Subcontractors > Directory | `SubcontractorService` | `subcontractors`, `subcontractor_trades` | the same |
| Subcontractors > Jobs & Progress | `SubJobService` | `sub_jobs`, `sub_job_stages`, `sub_job_documents`, `sub_progress` | the same |
| Subcontractors > Billing & Payouts | `SubBillingService` | jobs, stages, `sub_vouchers`, `sub_payouts` | `sub_job_stages.reached_on`, `sub_vouchers`, `sub_payouts` |
| Receipts > Vault / Needs Review | `ReceiptService` | `receipts`, `deliveries` (stock purchases), `projects` | `receipts` |
| Receipts > Change Orders | `ChangeOrderService` | `change_orders`, `projects` | `change_orders`, `projects.target_end_date` |
| Receipts > Reports | `ReceiptReports` | `receipts`, `projects` | — |
| Every Edit History tab | `AuditService` | `audit_logs`, `users` | — |
| Website Admin > Inquiries | `WebsiteInquiries` | `inquiries` | `inquiries.status/notes/updated_by/updated_at` |

# 6. The DFD and the real tables

The Data Flow Diagram (`docs\diagrams\Desktop_App_DFD.drawio`) names data stores; this is where each one lives.

| DFD store | Tables |
|---|---|
| D1 Central Database | `construction_db` |
| D1.1 Project & Financial Records | `projects` + the money tables (4.2) |
| D1.2 Website Traffic Records | not built yet (Website Admin) |
| D2.1 Employee Records | `employees`, `employee_rates` |
| D2.2 User Accounts & Roles | `users`, `login_logs`, `password_requests` |
| D2.3 Attendance & Site Schedule Records | `attendance_imports`, `attendance_scans`, `attendance_days` |
| D2.4 Tax Brackets & Statutory Tables | `app_settings` (`hr.*`), `holidays`; the TRAIN table in `PayrollService.AnnualTax` |
| D2.5 Payroll Records | `payroll_runs`, `payslips`, `cash_advances`, `cash_advance_deductions` |
| D3.1 Material Inventory | `materials`, `material_usages`, `deliveries` |
| D3.2 Equipment Records | `equipment` |
| D3.3 Site Deployment Records | not built (location tracking dropped by the owner) |
| D3.4 Maintenance Logs | `maintenance_logs` |
| D3.5 Quotation Records | `quotes`, `purchases`, `vendors`, `vendor_documents` |
| D4.1 Subcontractor Records | `subcontractors`, `subcontractor_trades` |
| D4.2 Scope & Progress Records | `sub_jobs`, `sub_job_stages`, `sub_job_documents`, `sub_progress` |
| D4.3 Billing & Payout Records | `sub_vouchers`, `sub_payouts` |
| D5.1 Receipt Records | `receipts` (+ `deliveries` shown as stock purchases) |
| D5.2 Change Order Records | `change_orders` |
| D5.3 Project & Contract Records | `projects` |
| not in the DFD | `audit_logs`, `inquiries`, `app_settings`, `schema_versions` |

Differences from the drawn DFD: site deployment and subcontractor ratings were dropped; account statuses are Active / Locked / Disabled; Website Admin is a separate app with the same `users` table.

# 7. Queries for study

Run in pgAdmin (**Tools > Query Tool** on `construction_db`). Use `SELECT` only on the real database; practice changes on the work PC's test database.

```sql
-- Row counts per table
SELECT relname AS table_name, n_live_tup AS rows FROM pg_stat_user_tables ORDER BY relname;

-- Every foreign key in the schema
SELECT conrelid::regclass AS from_table, pg_get_constraintdef(oid) AS link
FROM pg_constraint WHERE contype = 'f' ORDER BY 1;

-- Each employee's current daily rate (DISTINCT ON keeps the latest row per employee)
SELECT DISTINCT ON (e.employee_id) e.full_name, r.daily_rate, r.effective_from
FROM employees e JOIN employee_rates r USING (employee_id)
WHERE r.effective_from <= CURRENT_DATE
ORDER BY e.employee_id, r.effective_from DESC;

-- Attendance summary per person for October
SELECT e.full_name,
       COUNT(*) FILTER (WHERE d.status IN ('Present', 'Late')) AS days_present,
       COUNT(*) FILTER (WHERE d.status = 'Absent')             AS absences,
       SUM(d.late_minutes)                                     AS late_minutes,
       SUM(d.ot_minutes) FILTER (WHERE d.ot_status = 'Approved') AS approved_ot_minutes
FROM attendance_days d JOIN employees e USING (employee_id)
WHERE d.work_date BETWEEN '2026-10-01' AND '2026-10-31'
GROUP BY e.full_name ORDER BY e.full_name;

-- Payroll totals per finalized run
SELECT r.pay_group, r.pay_date, COUNT(p.*) AS payslips, SUM(p.gross_pay) AS gross, SUM(p.net_pay) AS net
FROM payroll_runs r JOIN payslips p USING (run_id)
WHERE r.status = 'Final' GROUP BY r.run_id ORDER BY r.pay_date DESC;

-- A material's stock ledger: deliveries in, approved usage out, running total
WITH moves AS (
  SELECT delivered_on AS day, quantity AS change, 'delivery' AS what FROM deliveries WHERE material_id = 1
  UNION ALL
  SELECT used_on, -quantity, 'usage' FROM material_usages WHERE material_id = 1 AND status = 'Approved')
SELECT day, what, change, SUM(change) OVER (ORDER BY day, what) AS running_change FROM moves;

-- Who changed what today (Edit History across departments)
SELECT a.changed_at, u.full_name, a.department, a.table_name, a.record_name, a.action, a.new_values
FROM audit_logs a LEFT JOIN users u USING (user_id)
WHERE a.changed_at >= CURRENT_DATE ORDER BY a.changed_at DESC;
```

# 8. Working with the database

**Adding a change**

1. Write `database\NNN_name.sql` with the next number (`IF NOT EXISTS`, CHECK rules, indexes for the new queries).
2. Run `setup-database.cmd`.
3. Add or update the model class in `Company.Shared/Models` and its `DbSet` and key in `AppDbContext`.
4. Run `python docs\_tools\dbref\gen_schema.py` to refresh Appendix A, add the table to chapters 4 and 5, then rebuild the guides.

**Backup and restore**

```
pg_dump -h localhost -U postgres -Fc -f construction_db.backup construction_db
pg_restore -h localhost -U postgres -d construction_db_restored construction_db.backup   (into a new, empty database)
```

Back up the file storage folder together with the database: the tables only hold paths to those files.

# Appendix A: Schema reference

<!-- schema:start -->

Generated from the live database by `docs/_tools/dbref/gen_schema.py`. 37 tables. Types: `int` = 32-bit integer (`serial` = auto-numbered), `numeric(p,s)` = exact decimal, `timestamp` = date and time without time zone, `time` = time of day. "Null" = may be empty.

## Login, accounts and history (scripts 001, 004, 011)

### users

| Column | Type | Null | Default |
|---|---|---|---|
| `user_id` | serial |  |  |
| `employee_id` | int | yes |  |
| `username` | varchar(50) |  |  |
| `password_hash` | varchar(255) |  |  |
| `full_name` | varchar(100) |  |  |
| `level` | varchar(20) |  |  |
| `department` | varchar(30) | yes |  |
| `status` | varchar(20) |  | 'Active' |
| `failed_login_count` | int |  | 0 |
| `security_question` | varchar(200) | yes |  |
| `security_answer_hash` | varchar(255) | yes |  |
| `last_login_at` | timestamp | yes |  |
| `created_at` | timestamp |  | now() |
| `updated_at` | timestamp |  | now() |

- **Primary key** `users_pkey`: `PRIMARY KEY user_id`
- **Unique** `users_username_key`: `UNIQUE username`
- **Foreign key** `fk_users_employee`: `FOREIGN KEY employee_id REFERENCES employees(employee_id)`
- **Check** `chk_users_department`: `CHECK ((((level IN ('Admin', 'Website Admin')) AND (department IS NULL)) OR ((level IN ('Supervisor', 'Employee')) AND (department IS NOT NULL))))`
- **Check** `users_department_check`: `CHECK ((department IN ('HR & Payroll', 'Inventory', 'Subcontractors', 'Receipts')))`
- **Check** `users_level_check`: `CHECK ((level IN ('Admin', 'Supervisor', 'Employee', 'Website Admin')))`
- **Check** `users_status_check`: `CHECK ((status IN ('Active', 'Locked', 'Disabled')))`
- **Unique index** `ux_users_employee`: `employee_id WHERE (employee_id IS NOT NULL)`

### login_logs

| Column | Type | Null | Default |
|---|---|---|---|
| `log_id` | serial |  |  |
| `user_id` | int | yes |  |
| `username_tried` | varchar(50) | yes |  |
| `action` | varchar(30) |  |  |
| `details` | varchar(255) | yes |  |
| `computer_name` | varchar(100) | yes |  |
| `logged_at` | timestamp |  | now() |

- **Primary key** `login_logs_pkey`: `PRIMARY KEY log_id`
- **Foreign key** `login_logs_user_id_fkey`: `FOREIGN KEY user_id REFERENCES users(user_id)`
- **Check** `login_logs_action_check`: `CHECK ((action IN ('Login', 'Logout', 'Idle Sign Out', 'Failed Login', 'Account Locked', 'Account Unlocked', 'Password Changed', 'Password Request')))`
- **Index** `ix_login_logs_logged_at`: `(logged_at DESC)`
- **Index** `ix_login_logs_user`: `user_id`

### password_requests

| Column | Type | Null | Default |
|---|---|---|---|
| `request_id` | serial |  |  |
| `user_id` | int |  |  |
| `requested_at` | timestamp |  | now() |
| `status` | varchar(20) |  | 'Waiting' |
| `handled_by` | int | yes |  |
| `handled_at` | timestamp | yes |  |

- **Primary key** `password_requests_pkey`: `PRIMARY KEY request_id`
- **Foreign key** `password_requests_handled_by_fkey`: `FOREIGN KEY handled_by REFERENCES users(user_id)`
- **Foreign key** `password_requests_user_id_fkey`: `FOREIGN KEY user_id REFERENCES users(user_id)`
- **Check** `password_requests_status_check`: `CHECK ((status IN ('Waiting', 'Done', 'Dismissed')))`
- **Index** `ix_password_requests_waiting`: `status WHERE (status = 'Waiting')`

### audit_logs

| Column | Type | Null | Default |
|---|---|---|---|
| `audit_id` | serial |  |  |
| `user_id` | int | yes |  |
| `department` | varchar(30) | yes |  |
| `table_name` | varchar(50) |  |  |
| `record_id` | int | yes |  |
| `record_name` | varchar(150) | yes |  |
| `action` | varchar(10) |  |  |
| `old_values` | jsonb | yes |  |
| `new_values` | jsonb | yes |  |
| `changed_at` | timestamp |  | now() |

- **Primary key** `audit_logs_pkey`: `PRIMARY KEY audit_id`
- **Foreign key** `audit_logs_user_id_fkey`: `FOREIGN KEY user_id REFERENCES users(user_id)`
- **Check** `audit_logs_action_check`: `CHECK ((action IN ('Added', 'Edited', 'Deleted')))`
- **Index** `ix_audit_logs_dept_time`: `(department, changed_at DESC)`

## Projects (002)

### projects

| Column | Type | Null | Default |
|---|---|---|---|
| `project_id` | serial |  |  |
| `project_name` | varchar(150) |  |  |
| `site_location` | varchar(255) |  |  |
| `client_name` | varchar(150) | yes |  |
| `sector` | varchar(40) |  |  |
| `contract_price` | numeric(14,2) |  | 0 |
| `start_date` | date |  |  |
| `target_end_date` | date | yes |  |
| `completed_date` | date | yes |  |
| `status` | varchar(20) |  | 'Pending' |
| `show_on_website` | boolean |  | false |
| `created_by` | int | yes |  |
| `created_at` | timestamp |  | now() |
| `updated_at` | timestamp |  | now() |

- **Primary key** `projects_pkey`: `PRIMARY KEY project_id`
- **Foreign key** `projects_created_by_fkey`: `FOREIGN KEY created_by REFERENCES users(user_id)`
- **Check** `chk_projects_completed_after_start`: `CHECK (((completed_date IS NULL) OR (completed_date >= start_date)))`
- **Check** `chk_projects_target_after_start`: `CHECK (((target_end_date IS NULL) OR (target_end_date >= start_date)))`
- **Check** `projects_contract_price_check`: `CHECK (contract_price >= 0)`
- **Check** `projects_sector_check`: `CHECK ((sector IN ('Residential', 'Commercial', 'Industrial', 'Repairs / Renovations')))`
- **Check** `projects_status_check`: `CHECK ((status IN ('Pending', 'Ongoing', 'Done', 'Cancelled')))`
- **Index** `ix_projects_status`: `status`
- **Index** `ix_projects_website`: `show_on_website WHERE show_on_website`

## HR: employees and attendance (003, 012)

### employees

| Column | Type | Null | Default |
|---|---|---|---|
| `employee_id` | serial |  |  |
| `full_name` | varchar(100) |  |  |
| `contact_number` | varchar(30) | yes |  |
| `address` | varchar(255) | yes |  |
| `position` | varchar(80) | yes |  |
| `department` | varchar(30) |  |  |
| `pay_schedule` | varchar(10) |  |  |
| `pay_grade` | varchar(30) | yes |  |
| `biometric_id` | varchar(20) | yes |  |
| `gov_deductions` | boolean |  | false |
| `sss_no` | varchar(20) | yes |  |
| `philhealth_no` | varchar(20) | yes |  |
| `pagibig_no` | varchar(20) | yes |  |
| `tin` | varchar(20) | yes |  |
| `status` | varchar(10) |  | 'Active' |
| `created_by` | int | yes |  |
| `created_at` | timestamp |  | now() |
| `updated_at` | timestamp |  | now() |

- **Primary key** `employees_pkey`: `PRIMARY KEY employee_id`
- **Unique** `employees_biometric_id_key`: `UNIQUE biometric_id`
- **Foreign key** `employees_created_by_fkey`: `FOREIGN KEY created_by REFERENCES users(user_id)`
- **Check** `chk_employees_schedule_matches_department`: `CHECK ((((department = 'Site') AND (pay_schedule = 'Site')) OR ((department <> 'Site') AND (pay_schedule = 'Office'))))`
- **Check** `employees_department_check`: `CHECK ((department IN ('Administration', 'HR & Payroll', 'Inventory', 'Subcontractors', 'Receipts', 'Site')))`
- **Check** `employees_pay_schedule_check`: `CHECK ((pay_schedule IN ('Office', 'Site')))`
- **Check** `employees_status_check`: `CHECK ((status IN ('Active', 'Inactive')))`
- **Index** `ix_employees_status`: `status`

### employee_rates

| Column | Type | Null | Default |
|---|---|---|---|
| `rate_id` | serial |  |  |
| `employee_id` | int |  |  |
| `daily_rate` | numeric(10,2) |  |  |
| `effective_from` | date |  |  |
| `created_by` | int | yes |  |
| `created_at` | timestamp |  | now() |

- **Primary key** `employee_rates_pkey`: `PRIMARY KEY rate_id`
- **Unique** `employee_rates_employee_id_effective_from_key`: `UNIQUE (employee_id, effective_from)`
- **Foreign key** `employee_rates_created_by_fkey`: `FOREIGN KEY created_by REFERENCES users(user_id)`
- **Foreign key** `employee_rates_employee_id_fkey`: `FOREIGN KEY employee_id REFERENCES employees(employee_id)`
- **Check** `employee_rates_daily_rate_check`: `CHECK (daily_rate > 0)`

### attendance_imports

| Column | Type | Null | Default |
|---|---|---|---|
| `import_id` | serial |  |  |
| `file_name` | varchar(255) |  |  |
| `period_from` | date |  |  |
| `period_to` | date |  |  |
| `scans_added` | int |  | 0 |
| `scans_skipped` | int |  | 0 |
| `rows_rejected` | int |  | 0 |
| `problems` | text | yes |  |
| `imported_by` | int | yes |  |
| `imported_at` | timestamp |  | now() |

- **Primary key** `attendance_imports_pkey`: `PRIMARY KEY import_id`
- **Foreign key** `attendance_imports_imported_by_fkey`: `FOREIGN KEY imported_by REFERENCES users(user_id)`

### attendance_scans

| Column | Type | Null | Default |
|---|---|---|---|
| `scan_id` | serial |  |  |
| `employee_id` | int |  |  |
| `scan_time` | timestamp |  |  |
| `import_id` | int | yes |  |

- **Primary key** `attendance_scans_pkey`: `PRIMARY KEY scan_id`
- **Unique** `attendance_scans_employee_id_scan_time_key`: `UNIQUE (employee_id, scan_time)`
- **Foreign key** `attendance_scans_employee_id_fkey`: `FOREIGN KEY employee_id REFERENCES employees(employee_id)`
- **Foreign key** `attendance_scans_import_id_fkey`: `FOREIGN KEY import_id REFERENCES attendance_imports(import_id)`

### attendance_days

| Column | Type | Null | Default |
|---|---|---|---|
| `day_id` | serial |  |  |
| `employee_id` | int |  |  |
| `work_date` | date |  |  |
| `time_in` | time | yes |  |
| `break_out` | time | yes |  |
| `break_in` | time | yes |  |
| `time_out` | time | yes |  |
| `late_minutes` | int |  | 0 |
| `early_minutes` | int |  | 0 |
| `ot_minutes` | int |  | 0 |
| `ot_status` | varchar(10) |  | 'None' |
| `status` | varchar(15) |  |  |
| `fix_reason` | varchar(500) | yes |  |
| `fixed_by` | int | yes |  |
| `fixed_at` | timestamp | yes |  |
| `ot_decided_by` | int | yes |  |
| `ot_decided_at` | timestamp | yes |  |
| `updated_at` | timestamp |  | now() |

- **Primary key** `attendance_days_pkey`: `PRIMARY KEY day_id`
- **Unique** `attendance_days_employee_id_work_date_key`: `UNIQUE (employee_id, work_date)`
- **Foreign key** `attendance_days_employee_id_fkey`: `FOREIGN KEY employee_id REFERENCES employees(employee_id)`
- **Foreign key** `attendance_days_fixed_by_fkey`: `FOREIGN KEY fixed_by REFERENCES users(user_id)`
- **Foreign key** `attendance_days_ot_decided_by_fkey`: `FOREIGN KEY ot_decided_by REFERENCES users(user_id)`
- **Check** `attendance_days_ot_status_check`: `CHECK ((ot_status IN ('None', 'Waiting', 'Approved', 'Rejected')))`
- **Check** `attendance_days_status_check`: `CHECK ((status IN ('Present', 'Late', 'Missing Scan', 'Absent', 'Rest Day Work')))`
- **Index** `ix_attendance_days_date`: `work_date`

## HR: payroll (013)

### holidays

| Column | Type | Null | Default |
|---|---|---|---|
| `holiday_date` | date |  |  |
| `name` | varchar(100) |  |  |
| `kind` | varchar(10) |  | 'Regular' |

- **Primary key** `holidays_pkey`: `PRIMARY KEY holiday_date`
- **Check** `holidays_kind_check`: `CHECK ((kind IN ('Regular', 'Special')))`

### cash_advances

| Column | Type | Null | Default |
|---|---|---|---|
| `advance_id` | serial |  |  |
| `employee_id` | int |  |  |
| `given_on` | date |  |  |
| `amount` | numeric(12,2) |  |  |
| `per_payday` | numeric(12,2) |  |  |
| `balance` | numeric(12,2) |  |  |
| `reason` | varchar(500) | yes |  |
| `status` | varchar(10) |  | 'Waiting' |
| `created_by` | int | yes |  |
| `created_at` | timestamp |  | now() |
| `decided_by` | int | yes |  |
| `decided_at` | timestamp | yes |  |

- **Primary key** `cash_advances_pkey`: `PRIMARY KEY advance_id`
- **Foreign key** `cash_advances_created_by_fkey`: `FOREIGN KEY created_by REFERENCES users(user_id)`
- **Foreign key** `cash_advances_decided_by_fkey`: `FOREIGN KEY decided_by REFERENCES users(user_id)`
- **Foreign key** `cash_advances_employee_id_fkey`: `FOREIGN KEY employee_id REFERENCES employees(employee_id)`
- **Check** `cash_advances_amount_check`: `CHECK (amount > 0)`
- **Check** `cash_advances_balance_check`: `CHECK (balance >= 0)`
- **Check** `cash_advances_per_payday_check`: `CHECK (per_payday > 0)`
- **Check** `cash_advances_status_check`: `CHECK ((status IN ('Waiting', 'Active', 'Paid Off', 'Rejected')))`

### payroll_runs

| Column | Type | Null | Default |
|---|---|---|---|
| `run_id` | serial |  |  |
| `pay_group` | varchar(10) |  |  |
| `period_from` | date |  |  |
| `period_to` | date |  |  |
| `pay_date` | date |  |  |
| `status` | varchar(10) |  | 'Draft' |
| `computed_by` | int | yes |  |
| `computed_at` | timestamp |  | now() |
| `finalized_by` | int | yes |  |
| `finalized_at` | timestamp | yes |  |

- **Primary key** `payroll_runs_pkey`: `PRIMARY KEY run_id`
- **Unique** `payroll_runs_pay_group_period_from_key`: `UNIQUE (pay_group, period_from)`
- **Foreign key** `payroll_runs_computed_by_fkey`: `FOREIGN KEY computed_by REFERENCES users(user_id)`
- **Foreign key** `payroll_runs_finalized_by_fkey`: `FOREIGN KEY finalized_by REFERENCES users(user_id)`
- **Check** `payroll_runs_check`: `CHECK (period_to >= period_from)`
- **Check** `payroll_runs_pay_group_check`: `CHECK ((pay_group IN ('Office', 'Site')))`
- **Check** `payroll_runs_status_check`: `CHECK ((status IN ('Draft', 'Final')))`

### payslips

| Column | Type | Null | Default |
|---|---|---|---|
| `payslip_id` | serial |  |  |
| `run_id` | int |  |  |
| `employee_id` | int |  |  |
| `daily_rate` | numeric(10,2) |  |  |
| `days_paid` | numeric(5,1) |  | 0 |
| `basic_pay` | numeric(12,2) |  | 0 |
| `holiday_days` | int |  | 0 |
| `holiday_pay` | numeric(12,2) |  | 0 |
| `late_minutes` | int |  | 0 |
| `late_deduction` | numeric(12,2) |  | 0 |
| `ot_minutes` | int |  | 0 |
| `ot_pay` | numeric(12,2) |  | 0 |
| `gross_pay` | numeric(12,2) |  | 0 |
| `sss` | numeric(12,2) |  | 0 |
| `philhealth` | numeric(12,2) |  | 0 |
| `pagibig` | numeric(12,2) |  | 0 |
| `tax` | numeric(12,2) |  | 0 |
| `cash_advance` | numeric(12,2) |  | 0 |
| `net_pay` | numeric(12,2) |  | 0 |

- **Primary key** `payslips_pkey`: `PRIMARY KEY payslip_id`
- **Unique** `payslips_run_id_employee_id_key`: `UNIQUE (run_id, employee_id)`
- **Foreign key** `payslips_employee_id_fkey`: `FOREIGN KEY employee_id REFERENCES employees(employee_id)`
- **Foreign key** `payslips_run_id_fkey`: `FOREIGN KEY run_id REFERENCES payroll_runs(run_id)`
- **Check** `payslips_net_pay_check`: `CHECK (net_pay >= 0)`

### cash_advance_deductions

| Column | Type | Null | Default |
|---|---|---|---|
| `deduction_id` | serial |  |  |
| `advance_id` | int |  |  |
| `payslip_id` | int |  |  |
| `amount` | numeric(12,2) |  |  |
| `balance_after` | numeric(12,2) |  |  |

- **Primary key** `cash_advance_deductions_pkey`: `PRIMARY KEY deduction_id`
- **Foreign key** `cash_advance_deductions_advance_id_fkey`: `FOREIGN KEY advance_id REFERENCES cash_advances(advance_id)`
- **Foreign key** `cash_advance_deductions_payslip_id_fkey`: `FOREIGN KEY payslip_id REFERENCES payslips(payslip_id)`
- **Check** `cash_advance_deductions_amount_check`: `CHECK (amount > 0)`
- **Check** `cash_advance_deductions_balance_after_check`: `CHECK (balance_after >= 0)`

## Inventory (005)

### materials

| Column | Type | Null | Default |
|---|---|---|---|
| `material_id` | serial |  |  |
| `name` | varchar(120) |  |  |
| `category` | varchar(60) |  |  |
| `unit` | varchar(20) |  |  |
| `unit_price` | numeric(12,2) |  | 0 |
| `stock_qty` | numeric(12,2) |  | 0 |
| `minimum_stock` | numeric(12,2) |  | 0 |
| `created_by` | int | yes |  |
| `created_at` | timestamp |  | now() |
| `updated_at` | timestamp |  | now() |

- **Primary key** `materials_pkey`: `PRIMARY KEY material_id`
- **Unique** `materials_name_key`: `UNIQUE name`
- **Foreign key** `materials_created_by_fkey`: `FOREIGN KEY created_by REFERENCES users(user_id)`
- **Check** `materials_minimum_stock_check`: `CHECK (minimum_stock >= 0)`
- **Check** `materials_stock_qty_check`: `CHECK (stock_qty >= 0)`
- **Check** `materials_unit_price_check`: `CHECK (unit_price >= 0)`

### material_usages

| Column | Type | Null | Default |
|---|---|---|---|
| `usage_id` | serial |  |  |
| `material_id` | int |  |  |
| `project_id` | int |  |  |
| `quantity` | numeric(12,2) |  |  |
| `used_on` | date |  |  |
| `notes` | varchar(255) | yes |  |
| `status` | varchar(10) |  | 'Waiting' |
| `cost` | numeric(14,2) | yes |  |
| `requested_by` | int | yes |  |
| `decided_by` | int | yes |  |
| `decided_at` | timestamp | yes |  |
| `created_at` | timestamp |  | now() |

- **Primary key** `material_usages_pkey`: `PRIMARY KEY usage_id`
- **Foreign key** `material_usages_decided_by_fkey`: `FOREIGN KEY decided_by REFERENCES users(user_id)`
- **Foreign key** `material_usages_material_id_fkey`: `FOREIGN KEY material_id REFERENCES materials(material_id)`
- **Foreign key** `material_usages_project_id_fkey`: `FOREIGN KEY project_id REFERENCES projects(project_id)`
- **Foreign key** `material_usages_requested_by_fkey`: `FOREIGN KEY requested_by REFERENCES users(user_id)`
- **Check** `material_usages_quantity_check`: `CHECK (quantity > 0)`
- **Check** `material_usages_status_check`: `CHECK ((status IN ('Waiting', 'Approved', 'Rejected')))`
- **Index** `ix_material_usages_project`: `project_id WHERE (status = 'Approved')`

### vendors

| Column | Type | Null | Default |
|---|---|---|---|
| `vendor_id` | serial |  |  |
| `name` | varchar(150) |  |  |
| `contact_person` | varchar(100) | yes |  |
| `phone` | varchar(30) | yes |  |
| `email` | varchar(100) | yes |  |
| `address` | varchar(255) | yes |  |
| `bank_name` | varchar(100) | yes |  |
| `account_name` | varchar(100) | yes |  |
| `account_number` | varchar(40) | yes |  |
| `vat_status` | varchar(10) |  | 'VAT' |
| `created_by` | int | yes |  |
| `created_at` | timestamp |  | now() |
| `updated_at` | timestamp |  | now() |

- **Primary key** `vendors_pkey`: `PRIMARY KEY vendor_id`
- **Unique** `vendors_name_key`: `UNIQUE name`
- **Foreign key** `vendors_created_by_fkey`: `FOREIGN KEY created_by REFERENCES users(user_id)`
- **Check** `vendors_vat_status_check`: `CHECK ((vat_status IN ('VAT', 'Non-VAT')))`

### vendor_documents

| Column | Type | Null | Default |
|---|---|---|---|
| `document_id` | serial |  |  |
| `vendor_id` | int |  |  |
| `doc_type` | varchar(40) |  |  |
| `file_path` | varchar(400) |  |  |
| `original_name` | varchar(255) |  |  |
| `uploaded_by` | int | yes |  |
| `uploaded_at` | timestamp |  | now() |

- **Primary key** `vendor_documents_pkey`: `PRIMARY KEY document_id`
- **Foreign key** `vendor_documents_uploaded_by_fkey`: `FOREIGN KEY uploaded_by REFERENCES users(user_id)`
- **Foreign key** `vendor_documents_vendor_id_fkey`: `FOREIGN KEY vendor_id REFERENCES vendors(vendor_id)`
- **Check** `vendor_documents_doc_type_check`: `CHECK ((doc_type IN ('Company Profile', 'Bank Details', 'Certificate of Registration', 'Other')))`

### quotes

| Column | Type | Null | Default |
|---|---|---|---|
| `quote_id` | serial |  |  |
| `material_id` | int |  |  |
| `vendor_id` | int |  |  |
| `quoted_on` | date |  |  |
| `product_name` | varchar(150) |  |  |
| `description` | varchar(255) | yes |  |
| `category` | varchar(60) | yes |  |
| `unit_price` | numeric(12,2) |  |  |
| `quantity` | numeric(12,2) |  |  |
| `status` | varchar(12) |  | 'Open' |
| `created_by` | int | yes |  |
| `created_at` | timestamp |  | now() |

- **Primary key** `quotes_pkey`: `PRIMARY KEY quote_id`
- **Foreign key** `quotes_created_by_fkey`: `FOREIGN KEY created_by REFERENCES users(user_id)`
- **Foreign key** `quotes_material_id_fkey`: `FOREIGN KEY material_id REFERENCES materials(material_id)`
- **Foreign key** `quotes_vendor_id_fkey`: `FOREIGN KEY vendor_id REFERENCES vendors(vendor_id)`
- **Check** `quotes_quantity_check`: `CHECK (quantity > 0)`
- **Check** `quotes_status_check`: `CHECK ((status IN ('Open', 'Selected', 'Not Selected')))`
- **Check** `quotes_unit_price_check`: `CHECK (unit_price > 0)`

### purchases

| Column | Type | Null | Default |
|---|---|---|---|
| `purchase_id` | serial |  |  |
| `quote_id` | int |  |  |
| `material_id` | int |  |  |
| `vendor_id` | int |  |  |
| `quantity` | numeric(12,2) |  |  |
| `unit_price` | numeric(12,2) |  |  |
| `approved_by` | int | yes |  |
| `approved_at` | timestamp |  | now() |

- **Primary key** `purchases_pkey`: `PRIMARY KEY purchase_id`
- **Unique** `purchases_quote_id_key`: `UNIQUE quote_id`
- **Foreign key** `purchases_approved_by_fkey`: `FOREIGN KEY approved_by REFERENCES users(user_id)`
- **Foreign key** `purchases_material_id_fkey`: `FOREIGN KEY material_id REFERENCES materials(material_id)`
- **Foreign key** `purchases_quote_id_fkey`: `FOREIGN KEY quote_id REFERENCES quotes(quote_id)`
- **Foreign key** `purchases_vendor_id_fkey`: `FOREIGN KEY vendor_id REFERENCES vendors(vendor_id)`
- **Check** `purchases_quantity_check`: `CHECK (quantity > 0)`
- **Check** `purchases_unit_price_check`: `CHECK (unit_price > 0)`

### deliveries

| Column | Type | Null | Default |
|---|---|---|---|
| `delivery_id` | serial |  |  |
| `purchase_id` | int | yes |  |
| `vendor_id` | int |  |  |
| `material_id` | int |  |  |
| `quantity` | numeric(12,2) |  |  |
| `unit_price` | numeric(12,2) |  |  |
| `delivered_on` | date |  |  |
| `receipt_no` | varchar(60) |  |  |
| `receipt_file_path` | varchar(400) | yes |  |
| `physical_only` | boolean |  | false |
| `filed_location` | varchar(150) | yes |  |
| `recorded_by` | int | yes |  |
| `created_at` | timestamp |  | now() |

- **Primary key** `deliveries_pkey`: `PRIMARY KEY delivery_id`
- **Foreign key** `deliveries_material_id_fkey`: `FOREIGN KEY material_id REFERENCES materials(material_id)`
- **Foreign key** `deliveries_purchase_id_fkey`: `FOREIGN KEY purchase_id REFERENCES purchases(purchase_id)`
- **Foreign key** `deliveries_recorded_by_fkey`: `FOREIGN KEY recorded_by REFERENCES users(user_id)`
- **Foreign key** `deliveries_vendor_id_fkey`: `FOREIGN KEY vendor_id REFERENCES vendors(vendor_id)`
- **Check** `chk_deliveries_receipt`: `CHECK (((physical_only AND (filed_location IS NOT NULL)) OR ((NOT physical_only) AND (receipt_file_path IS NOT NULL))))`
- **Check** `deliveries_quantity_check`: `CHECK (quantity > 0)`
- **Check** `deliveries_unit_price_check`: `CHECK (unit_price >= 0)`

### equipment

| Column | Type | Null | Default |
|---|---|---|---|
| `equipment_id` | serial |  |  |
| `name` | varchar(120) |  |  |
| `type` | varchar(20) |  |  |
| `ownership` | varchar(10) |  |  |
| `rental_company` | varchar(150) | yes |  |
| `rental_rate` | numeric(12,2) | yes |  |
| `rental_rate_unit` | varchar(10) | yes |  |
| `return_date` | date | yes |  |
| `status` | varchar(15) |  | 'Available' |
| `next_maintenance` | date | yes |  |
| `created_by` | int | yes |  |
| `created_at` | timestamp |  | now() |
| `updated_at` | timestamp |  | now() |

- **Primary key** `equipment_pkey`: `PRIMARY KEY equipment_id`
- **Foreign key** `equipment_created_by_fkey`: `FOREIGN KEY created_by REFERENCES users(user_id)`
- **Check** `chk_equipment_rental`: `CHECK (((ownership = 'Owned') OR ((rental_company IS NOT NULL) AND (rental_rate IS NOT NULL) AND (rental_rate_unit IS NOT NULL))))`
- **Check** `equipment_ownership_check`: `CHECK ((ownership IN ('Owned', 'Rented')))`
- **Check** `equipment_rental_rate_check`: `CHECK (rental_rate >= 0)`
- **Check** `equipment_rental_rate_unit_check`: `CHECK ((rental_rate_unit IN ('Day', 'Week', 'Month')))`
- **Check** `equipment_status_check`: `CHECK ((status IN ('Available', 'Under Repair', 'Retired')))`
- **Check** `equipment_type_check`: `CHECK ((type IN ('Tool', 'Heavy Equipment', 'Vehicle')))`

### maintenance_logs

| Column | Type | Null | Default |
|---|---|---|---|
| `log_id` | serial |  |  |
| `equipment_id` | int |  |  |
| `done_on` | date |  |  |
| `kind` | varchar(12) |  |  |
| `work_done` | varchar(500) |  |  |
| `cost` | numeric(12,2) |  | 0 |
| `done_by` | varchar(150) | yes |  |
| `recorded_by` | int | yes |  |
| `created_at` | timestamp |  | now() |

- **Primary key** `maintenance_logs_pkey`: `PRIMARY KEY log_id`
- **Foreign key** `maintenance_logs_equipment_id_fkey`: `FOREIGN KEY equipment_id REFERENCES equipment(equipment_id)`
- **Foreign key** `maintenance_logs_recorded_by_fkey`: `FOREIGN KEY recorded_by REFERENCES users(user_id)`
- **Check** `maintenance_logs_cost_check`: `CHECK (cost >= 0)`
- **Check** `maintenance_logs_kind_check`: `CHECK ((kind IN ('Preventive', 'Repair')))`

## Subcontractors (006)

### subcontractors

| Column | Type | Null | Default |
|---|---|---|---|
| `subcontractor_id` | serial |  |  |
| `business_name` | varchar(150) |  |  |
| `trade_name` | varchar(150) | yes |  |
| `dti_sec_no` | varchar(60) | yes |  |
| `tin` | varchar(30) | yes |  |
| `primary_name` | varchar(100) | yes |  |
| `primary_role` | varchar(80) | yes |  |
| `primary_phone` | varchar(30) | yes |  |
| `primary_mobile` | varchar(30) | yes |  |
| `primary_email` | varchar(100) | yes |  |
| `secondary_name` | varchar(100) | yes |  |
| `secondary_role` | varchar(80) | yes |  |
| `secondary_phone` | varchar(30) | yes |  |
| `secondary_mobile` | varchar(30) | yes |  |
| `secondary_email` | varchar(100) | yes |  |
| `office_address` | varchar(255) | yes |  |
| `yard_address` | varchar(255) | yes |  |
| `bank_name` | varchar(100) | yes |  |
| `account_name` | varchar(100) | yes |  |
| `account_number` | varchar(40) | yes |  |
| `pcab_license_no` | varchar(60) | yes |  |
| `pcab_classification` | varchar(60) | yes |  |
| `pcab_expiry` | date | yes |  |
| `created_by` | int | yes |  |
| `created_at` | timestamp |  | now() |
| `updated_at` | timestamp |  | now() |

- **Primary key** `subcontractors_pkey`: `PRIMARY KEY subcontractor_id`
- **Unique** `subcontractors_business_name_key`: `UNIQUE business_name`
- **Foreign key** `subcontractors_created_by_fkey`: `FOREIGN KEY created_by REFERENCES users(user_id)`

### subcontractor_trades

| Column | Type | Null | Default |
|---|---|---|---|
| `subcontractor_id` | int |  |  |
| `trade` | varchar(60) |  |  |

- **Primary key** `subcontractor_trades_pkey`: `PRIMARY KEY (subcontractor_id, trade)`
- **Foreign key** `subcontractor_trades_subcontractor_id_fkey`: `FOREIGN KEY subcontractor_id REFERENCES subcontractors(subcontractor_id) ON DELETE CASCADE`

### sub_jobs

| Column | Type | Null | Default |
|---|---|---|---|
| `job_id` | serial |  |  |
| `subcontractor_id` | int |  |  |
| `project_id` | int |  |  |
| `scope` | varchar(1000) |  |  |
| `contract_amount` | numeric(14,2) |  |  |
| `start_date` | date |  |  |
| `end_date` | date |  |  |
| `retention_pct` | numeric(5,2) |  | 10 |
| `created_by` | int | yes |  |
| `created_at` | timestamp |  | now() |
| `updated_at` | timestamp |  | now() |

- **Primary key** `sub_jobs_pkey`: `PRIMARY KEY job_id`
- **Foreign key** `sub_jobs_created_by_fkey`: `FOREIGN KEY created_by REFERENCES users(user_id)`
- **Foreign key** `sub_jobs_project_id_fkey`: `FOREIGN KEY project_id REFERENCES projects(project_id)`
- **Foreign key** `sub_jobs_subcontractor_id_fkey`: `FOREIGN KEY subcontractor_id REFERENCES subcontractors(subcontractor_id)`
- **Check** `sub_jobs_check`: `CHECK (end_date >= start_date)`
- **Check** `sub_jobs_contract_amount_check`: `CHECK (contract_amount > 0)`
- **Check** `sub_jobs_retention_pct_check`: `CHECK (((retention_pct >= 0) AND (retention_pct < 100)))`
- **Index** `ix_sub_jobs_project`: `project_id`

### sub_job_stages

| Column | Type | Null | Default |
|---|---|---|---|
| `stage_id` | serial |  |  |
| `job_id` | int |  |  |
| `stage_no` | int |  |  |
| `name` | varchar(80) |  |  |
| `percent` | numeric(5,2) |  |  |
| `reached_on` | date | yes |  |
| `reached_by` | int | yes |  |

- **Primary key** `sub_job_stages_pkey`: `PRIMARY KEY stage_id`
- **Unique** `sub_job_stages_job_id_stage_no_key`: `UNIQUE (job_id, stage_no)`
- **Foreign key** `sub_job_stages_job_id_fkey`: `FOREIGN KEY job_id REFERENCES sub_jobs(job_id)`
- **Foreign key** `sub_job_stages_reached_by_fkey`: `FOREIGN KEY reached_by REFERENCES users(user_id)`
- **Check** `sub_job_stages_percent_check`: `CHECK (((percent > 0) AND (percent <= 100)))`

### sub_job_documents

| Column | Type | Null | Default |
|---|---|---|---|
| `document_id` | serial |  |  |
| `job_id` | int |  |  |
| `file_path` | varchar(400) |  |  |
| `original_name` | varchar(255) |  |  |
| `uploaded_by` | int | yes |  |
| `uploaded_at` | timestamp |  | now() |

- **Primary key** `sub_job_documents_pkey`: `PRIMARY KEY document_id`
- **Foreign key** `sub_job_documents_job_id_fkey`: `FOREIGN KEY job_id REFERENCES sub_jobs(job_id)`
- **Foreign key** `sub_job_documents_uploaded_by_fkey`: `FOREIGN KEY uploaded_by REFERENCES users(user_id)`

### sub_progress

| Column | Type | Null | Default |
|---|---|---|---|
| `progress_id` | serial |  |  |
| `job_id` | int |  |  |
| `percent` | numeric(5,2) |  |  |
| `progress_date` | date |  |  |
| `notes` | varchar(500) | yes |  |
| `file_path` | varchar(400) | yes |  |
| `original_name` | varchar(255) | yes |  |
| `created_by` | int | yes |  |
| `created_at` | timestamp |  | now() |

- **Primary key** `sub_progress_pkey`: `PRIMARY KEY progress_id`
- **Foreign key** `sub_progress_created_by_fkey`: `FOREIGN KEY created_by REFERENCES users(user_id)`
- **Foreign key** `sub_progress_job_id_fkey`: `FOREIGN KEY job_id REFERENCES sub_jobs(job_id)`
- **Check** `sub_progress_percent_check`: `CHECK (((percent >= 0) AND (percent <= 100)))`

### sub_vouchers

| Column | Type | Null | Default |
|---|---|---|---|
| `voucher_id` | serial |  |  |
| `voucher_no` | varchar(20) |  |  |
| `job_id` | int |  |  |
| `stage_id` | int | yes |  |
| `kind` | varchar(10) |  |  |
| `gross_amount` | numeric(14,2) |  |  |
| `retention_amount` | numeric(14,2) |  | 0 |
| `payable_amount` | numeric(14,2) |  |  |
| `status` | varchar(10) |  | 'Waiting' |
| `prepared_by` | int | yes |  |
| `prepared_at` | timestamp |  | now() |
| `decided_by` | int | yes |  |
| `decided_at` | timestamp | yes |  |

- **Primary key** `sub_vouchers_pkey`: `PRIMARY KEY voucher_id`
- **Unique** `sub_vouchers_voucher_no_key`: `UNIQUE voucher_no`
- **Foreign key** `sub_vouchers_decided_by_fkey`: `FOREIGN KEY decided_by REFERENCES users(user_id)`
- **Foreign key** `sub_vouchers_job_id_fkey`: `FOREIGN KEY job_id REFERENCES sub_jobs(job_id)`
- **Foreign key** `sub_vouchers_prepared_by_fkey`: `FOREIGN KEY prepared_by REFERENCES users(user_id)`
- **Foreign key** `sub_vouchers_stage_id_fkey`: `FOREIGN KEY stage_id REFERENCES sub_job_stages(stage_id)`
- **Check** `sub_vouchers_check`: `CHECK (((kind = 'Stage') = (stage_id IS NOT NULL)))`
- **Check** `sub_vouchers_gross_amount_check`: `CHECK (gross_amount > 0)`
- **Check** `sub_vouchers_kind_check`: `CHECK ((kind IN ('Stage', 'Retention')))`
- **Check** `sub_vouchers_payable_amount_check`: `CHECK (payable_amount > 0)`
- **Check** `sub_vouchers_retention_amount_check`: `CHECK (retention_amount >= 0)`
- **Check** `sub_vouchers_status_check`: `CHECK ((status IN ('Waiting', 'Approved', 'Rejected', 'Paid')))`
- **Unique index** `ux_sub_vouchers_retention`: `job_id WHERE ((kind = 'Retention') AND (status <> 'Rejected'))`
- **Unique index** `ux_sub_vouchers_stage`: `stage_id WHERE (status <> 'Rejected')`

### sub_payouts

| Column | Type | Null | Default |
|---|---|---|---|
| `payout_id` | serial |  |  |
| `voucher_id` | int |  |  |
| `job_id` | int |  |  |
| `paid_on` | date |  |  |
| `amount` | numeric(14,2) |  |  |
| `reference_no` | varchar(60) | yes |  |
| `method` | varchar(20) |  |  |
| `created_by` | int | yes |  |
| `created_at` | timestamp |  | now() |

- **Primary key** `sub_payouts_pkey`: `PRIMARY KEY payout_id`
- **Unique** `sub_payouts_voucher_id_key`: `UNIQUE voucher_id`
- **Foreign key** `sub_payouts_created_by_fkey`: `FOREIGN KEY created_by REFERENCES users(user_id)`
- **Foreign key** `sub_payouts_job_id_fkey`: `FOREIGN KEY job_id REFERENCES sub_jobs(job_id)`
- **Foreign key** `sub_payouts_voucher_id_fkey`: `FOREIGN KEY voucher_id REFERENCES sub_vouchers(voucher_id)`
- **Check** `sub_payouts_amount_check`: `CHECK (amount > 0)`
- **Check** `sub_payouts_method_check`: `CHECK ((method IN ('Bank Transfer', 'Check', 'Cash')))`
- **Index** `ix_sub_payouts_job`: `job_id`

## Receipts (007)

### receipts

| Column | Type | Null | Default |
|---|---|---|---|
| `receipt_id` | serial |  |  |
| `receipt_no` | varchar(60) | yes |  |
| `purchase_date` | date |  |  |
| `merchant` | varchar(150) |  |  |
| `amount` | numeric(14,2) |  |  |
| `category` | varchar(40) |  |  |
| `expense_type` | varchar(10) |  |  |
| `project_id` | int | yes |  |
| `file_path` | varchar(400) | yes |  |
| `original_name` | varchar(255) | yes |  |
| `physical_only` | boolean |  | false |
| `filed_location` | varchar(150) | yes |  |
| `no_receipt` | boolean |  | false |
| `no_receipt_reason` | varchar(500) | yes |  |
| `status` | varchar(10) |  | 'Waiting' |
| `uploaded_by` | int | yes |  |
| `uploaded_at` | timestamp |  | now() |
| `decided_by` | int | yes |  |
| `decided_at` | timestamp | yes |  |
| `updated_at` | timestamp |  | now() |

- **Primary key** `receipts_pkey`: `PRIMARY KEY receipt_id`
- **Foreign key** `receipts_decided_by_fkey`: `FOREIGN KEY decided_by REFERENCES users(user_id)`
- **Foreign key** `receipts_project_id_fkey`: `FOREIGN KEY project_id REFERENCES projects(project_id)`
- **Foreign key** `receipts_uploaded_by_fkey`: `FOREIGN KEY uploaded_by REFERENCES users(user_id)`
- **Check** `chk_receipts_copy`: `CHECK (((no_receipt AND (no_receipt_reason IS NOT NULL) AND (file_path IS NULL) AND (NOT physical_only)) OR ((NOT no_receipt) AND (receipt_no IS NOT NULL) AND ((physical_only AND (filed_location IS NOT NULL)) OR ((NOT physical_only) AND (file_path IS NOT NULL))))))`
- **Check** `chk_receipts_project`: `CHECK (((expense_type = 'Project') = (project_id IS NOT NULL)))`
- **Check** `receipts_amount_check`: `CHECK (amount > 0)`
- **Check** `receipts_expense_type_check`: `CHECK ((expense_type IN ('Project', 'Overhead')))`
- **Check** `receipts_status_check`: `CHECK ((status IN ('Waiting', 'Approved', 'Rejected')))`
- **Index** `ix_receipts_date`: `purchase_date`
- **Index** `ix_receipts_project`: `project_id WHERE (status = 'Approved')`

### change_orders

| Column | Type | Null | Default |
|---|---|---|---|
| `change_order_id` | serial |  |  |
| `co_no` | varchar(20) |  |  |
| `project_id` | int |  |  |
| `requested_on` | date |  |  |
| `description` | varchar(1000) |  |  |
| `cost_change` | numeric(14,2) |  | 0 |
| `days_change` | int |  | 0 |
| `file_path` | varchar(400) | yes |  |
| `original_name` | varchar(255) | yes |  |
| `status` | varchar(10) |  | 'Waiting' |
| `created_by` | int | yes |  |
| `created_at` | timestamp |  | now() |
| `decided_by` | int | yes |  |
| `decided_at` | timestamp | yes |  |

- **Primary key** `change_orders_pkey`: `PRIMARY KEY change_order_id`
- **Unique** `change_orders_co_no_key`: `UNIQUE co_no`
- **Foreign key** `change_orders_created_by_fkey`: `FOREIGN KEY created_by REFERENCES users(user_id)`
- **Foreign key** `change_orders_decided_by_fkey`: `FOREIGN KEY decided_by REFERENCES users(user_id)`
- **Foreign key** `change_orders_project_id_fkey`: `FOREIGN KEY project_id REFERENCES projects(project_id)`
- **Check** `change_orders_check`: `CHECK (((cost_change <> 0) OR (days_change <> 0)))`
- **Check** `change_orders_status_check`: `CHECK ((status IN ('Waiting', 'Approved', 'Rejected')))`
- **Index** `ix_change_orders_project`: `project_id WHERE (status = 'Approved')`

## Website (010)

### inquiries

| Column | Type | Null | Default |
|---|---|---|---|
| `inquiry_id` | serial |  |  |
| `received_at` | timestamp |  | now() |
| `full_name` | varchar(100) |  |  |
| `email` | varchar(150) | yes |  |
| `phone` | varchar(40) | yes |  |
| `project_location` | varchar(255) | yes |  |
| `service_required` | varchar(100) | yes |  |
| `message` | varchar(4000) |  |  |
| `status` | varchar(12) |  | 'New' |
| `notes` | varchar(2000) | yes |  |
| `updated_by` | int | yes |  |
| `updated_at` | timestamp | yes |  |

- **Primary key** `inquiries_pkey`: `PRIMARY KEY inquiry_id`
- **Foreign key** `inquiries_updated_by_users_fkey`: `FOREIGN KEY updated_by REFERENCES users(user_id)`
- **Check** `chk_inquiries_contact`: `CHECK (((email IS NOT NULL) OR (phone IS NOT NULL)))`
- **Check** `inquiries_status_check`: `CHECK ((status IN ('New', 'Contacted', 'Closed')))`
- **Index** `ix_inquiries_received`: `(received_at DESC)`
- **Index** `ix_inquiries_status`: `status`

## System

### app_settings

| Column | Type | Null | Default |
|---|---|---|---|
| `key` | varchar(60) |  |  |
| `value` | text |  |  |
| `updated_by` | int | yes |  |
| `updated_at` | timestamp |  | now() |

- **Primary key** `app_settings_pkey`: `PRIMARY KEY key`
- **Foreign key** `app_settings_updated_by_fkey`: `FOREIGN KEY updated_by REFERENCES users(user_id)`

### schema_versions

| Column | Type | Null | Default |
|---|---|---|---|
| `version` | int |  |  |
| `script_name` | varchar(200) |  |  |
| `applied_at` | timestamp |  | now() |

- **Primary key** `schema_versions_pkey`: `PRIMARY KEY version`

<!-- schema:end -->
