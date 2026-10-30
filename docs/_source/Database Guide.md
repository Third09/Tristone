# Tri-Stone Database: Study Guide

A guide to understanding the Tri-Stone database: what a database is, how this one is built, what every table holds, how the tables connect, and how the Data Flow Diagram (DFD) relates to it. Read it from the start if you're new to databases; each chapter builds on the one before.

> Everything here matches the real database (`construction_db`, PostgreSQL 18) as of scripts 001-011: **29 tables**.

# 1. What a database is

A **database** is an organized store of information. Ours is a **relational** database: the information lives in **tables**, and tables are **related** (linked) to each other.

A table is like a spreadsheet:

- each **column** is one kind of information (a name, a price, a date);
- each **row** is one record (one project, one employee, one receipt).

Example: part of the `projects` table.

| project_id | project_name | sector | contract_price | status |
|---|---|---|---|---|
| 1 | Santos Residence | Residential | 2500000.00 | Ongoing |
| 2 | Bodega Repair | Repairs / Renovations | 180000.00 | Done |

The **database server** (PostgreSQL) is the program that stores the tables, keeps them safe, and answers questions about them. The office app on each PC sends it requests like "give me all ongoing projects" or "save this receipt".

**Why not just Excel files?** Five people can work at the same time without overwriting each other; the database refuses bad data by itself; and one question can combine many tables at once (for example, "total expenses of each project").

# 2. The key ideas

## 2.1 Primary key: each row's ID

Every table has a column that uniquely identifies each row, usually named `something_id` (`project_id`, `employee_id`). This is the **primary key**.

- The database gives it out automatically: 1, 2, 3...
- It never changes and is never reused, even if the row is deleted.
- Names can repeat or change; IDs never do. That's why links use IDs, not names.

## 2.2 Foreign key: the links between tables

A **foreign key** is a column that stores **another table's ID**. It's how tables are connected.

Example: a receipt for a project stores that project's ID.

```
projects                         receipts
+------------+-----------------+  +------------+--------+------------+
| project_id | project_name    |  | receipt_id | amount | project_id |
+------------+-----------------+  +------------+--------+------------+
| 1          | Santos Residence| <-----------  | 10 | 1250.00 | 1     |
+------------+-----------------+  +------------+--------+------------+
```

The database **guarantees** the link is valid: you can't save a receipt for project 99 if project 99 doesn't exist, and you can't delete project 1 while receipts point to it. That's why the app says "can't delete, it has records; set it to Cancelled instead".

**One-to-many:** one project has **many** receipts, but each receipt belongs to **one** project. Almost every link in this database is one-to-many.

## 2.3 NULL: "nothing here"

`NULL` means a value is **empty / not known**. Some columns may be NULL (a project's `client_name` is optional); others are **NOT NULL** (required, like `project_name`).

## 2.4 Check rules

A **check** is a rule the database enforces on every save, even if the app had a bug. Examples from our database:

| Table | Rule |
|---|---|
| `projects` | `status` must be Pending, Ongoing, Done or Cancelled |
| `projects` | `target_end_date` can't be before `start_date` |
| `materials` | `stock_qty` can't go below 0 |
| `users` | Admins and Website Admins have **no** department; everyone else **must** have one |
| `employees` | Department **Site** must have pay schedule **Site**; every other department must be **Office** |
| `receipts` | A **Project** receipt must have a project; an **Overhead** receipt must not |

## 2.5 Unique rules

A **unique** rule means no two rows may share that value: usernames, material names, vendor names, voucher numbers (SV-0001), change order numbers (CO-001), biometric IDs.

Some are **partial**, meaning they only apply to some rows. Example: a subcontractor stage can have only **one voucher that isn't Rejected**. A rejected voucher doesn't count, so a new one can be made.

## 2.6 Data types used

| Type | What it holds | Example |
|---|---|---|
| `integer` | Whole numbers; all IDs | `42` |
| `character varying(n)` | Text up to *n* characters | `varchar(150)` for names |
| `text` | Text of any length | settings values |
| `numeric(14,2)` | **Exact** money: up to 14 digits, 2 after the point | `2500000.00` |
| `numeric(12,2)` | Exact quantities and prices | `12.50` bags |
| `date` | A day, no time | `2026-10-30` |
| `timestamp` | Day and time | `2026-10-30 08:15:00` |
| `boolean` | Yes/no | `true` |
| `jsonb` | A small bundle of named values | the old/new values in `audit_logs` |

**Why numeric for money, not decimal-point "float" numbers?** Floats are approximate (0.1 + 0.2 = 0.30000000000000004). Numeric is exact, so pesos always add up to the centavo.

# 3. How this database is built

## 3.1 Numbered scripts

The database is never built by clicking around in pgAdmin. Every change is a **numbered SQL script** in the `database` folder:

| Script | Creates |
|---|---|
| `001_login_security.sql` | `users`, `login_logs`, `password_requests`, `audit_logs` |
| `002_projects.sql` | `projects` |
| `003_employees.sql` | `employees`, `employee_rates`, and the link from `users` to `employees` |
| `004_one_login_per_employee.sql` | The rule "one login per employee" |
| `005_inventory.sql` | `app_settings`, `materials`, `material_usages`, `vendors`, `vendor_documents`, `quotes`, `purchases`, `deliveries`, `equipment`, `maintenance_logs` |
| `006_subcontractors.sql` | `subcontractors`, `subcontractor_trades`, `sub_jobs`, `sub_job_stages`, `sub_job_documents`, `sub_progress`, `sub_vouchers`, `sub_payouts` |
| `007_receipts.sql` | `receipts`, `change_orders` |
| `008_website_accounts.sql` | `website_accounts`, `website_login_logs` (the Website Admin app's own accounts at first; removed by 011) |
| `009_website_main_account.sql` | the `is_main` column on `website_accounts` (main accounts manage the others), removed by 011 |
| `010_inquiries.sql` | `inquiries` (website Contact form messages) |
| `011_merge_website_accounts.sql` | one account list: the Website Admin level in `users`; moves the website accounts in and drops their tables |

**`setup-database.cmd`** (double-click) creates the database if it's missing, then runs every script that hasn't run yet. The table **`schema_versions`** remembers which scripts already ran, so each runs only **once**.

**Why scripts?** Any PC (and the server later) gets **exactly** the same database by running them. It's also a written record of every table and why it exists.

**Golden rule:** a script that has run is **never edited**. A change is always a **new** script with the next number (008, 009...).

## 3.2 How the app talks to the database

```
  Office PC                                   Server
+-------------------------------+          +------------------+
| Tri-Stone app (WinForms)      |          | PostgreSQL       |
|  Pages (screens)              |          |  construction_db |
|    -> Services (the rules)    |  ---->   |   29 tables      |
|      -> Entity Framework Core | network  |                  |
|        (C# objects <-> rows)  |  <----   |                  |
+-------------------------------+          +------------------+
```

- **Entity Framework Core (EF Core)** translates between database rows and C# objects. A row in `projects` becomes a `Project` object in the code; saving the object writes the row.
- The classes that match the tables are in `src/Company.Shared/Models`. `AppDbContext` lists which class goes with which table.
- Each PC keeps the server address and database password in `C:\ProgramData\TriStone\OfficeApp\dbsettings.json`, with the password encrypted so it only works on that PC.
- The **website** will never talk to the database directly. It will go through the **API**, which only exposes what the website needs (chapter 9).

## 3.3 Who did it and when

Most tables carry the same "who and when" columns:

| Column | Meaning |
|---|---|
| `created_by` / `recorded_by` / `uploaded_by` / `prepared_by` / `requested_by` | The `user_id` of whoever made the record |
| `decided_by` / `approved_by` / `handled_by` | The `user_id` of whoever approved or rejected it |
| `created_at`, `decided_at`, `updated_at` | When it happened |

## 3.4 Files (scans and photos)

Files are **not** stored inside the database. They're copied into a storage folder (its location is in `app_settings`, key `file_storage_path`), and the database keeps only the **path** inside that folder, for example `receipts/2026/10/ab12cd_receipt.pdf`, plus the original file name. The database stays small and fast, and the folder can be backed up on its own.

# 4. The whole map

29 tables in 8 groups:

| Group | Tables |
|---|---|
| Login & security | `users`, `login_logs`, `password_requests`, `audit_logs` |
| Projects | `projects` |
| HR | `employees`, `employee_rates` |
| Inventory | `materials`, `material_usages`, `vendors`, `vendor_documents`, `quotes`, `purchases`, `deliveries`, `equipment`, `maintenance_logs` |
| Subcontractors | `subcontractors`, `subcontractor_trades`, `sub_jobs`, `sub_job_stages`, `sub_job_documents`, `sub_progress`, `sub_vouchers`, `sub_payouts` |
| Receipts | `receipts`, `change_orders` |
| Website Admin | `inquiries` (Website Admin logins are rows in `users`) |
| System | `app_settings`, `schema_versions` |

**`projects` is the center.** Everything that costs or earns money for a project points to it:

```
                         +-----------+
   material_usages ----> |           | <---- receipts
   sub_jobs -----------> | projects  | <---- change_orders
                         +-----------+

   employees <---- users          (a login belongs to an employee)
   employees <---- employee_rates (rate history)
```

Arrows point from the table that **stores the ID** to the table it **links to**.

# 5. Login & security tables

## users: the logins

| Column | Meaning |
|---|---|
| `user_id` | ID |
| `employee_id` | Which employee this login belongs to (empty only for old accounts like the first Admin). **Unique**: one login per employee. |
| `username` | Unique sign-in name |
| `password_hash` | The password, **scrambled** with bcrypt. The real password is never stored, so nobody can read it, not even an Admin. |
| `full_name` | Copied from the employee |
| `level` | Admin / Supervisor / Employee / Website Admin |
| `department` | HR & Payroll / Inventory / Subcontractors / Receipts; empty for Admins and Website Admins (check rule) |
| `status` | Active / Locked / Disabled |
| `failed_login_count` | Wrong passwords in a row; 5 locks the account |
| `security_question`, `security_answer_hash` | Admins' password recovery; the answer is scrambled like a password |
| `last_login_at` | Last successful sign-in |

**How a password check works:** when you sign in, the app scrambles what you typed the same way and compares the two scrambled results. If they match, the password is right. The scrambling can't be reversed.

## login_logs: the sign-in diary

One row per sign-in, sign-out, idle sign-out, failed attempt, lock, unlock, password change or password request: `user_id`, `username_tried` (even unknown names), `action`, `details`, `computer_name`, `logged_at`. The Dashboard's Log History reads this table.

## Website Admin logins

There is **one** list of logins for both the office app and the Website Admin screens: `users`. A Website Admin is a row with `level` = Website Admin and no department (script 011). At first the Website Admin app had its own tables (`website_accounts`, `website_login_logs`, scripts 008 and 009); script 011 copied those accounts (with their passwords), their sign-in history and their inquiry follow-ups into `users`, `login_logs` and `inquiries`, then removed the old tables.

## inquiries: website Contact form messages

`inquiry_id`, `received_at`, `full_name`, `email`, `phone`, `project_location`, `service_required` (optional), `message`, `status` (New / Contacted / Closed), `notes` (internal), `updated_by` (the login in `users` who last changed it: a Website Admin or an Admin), `updated_at`. Check rule: an email **or** a phone number must be given. Rows are never deleted.

## password_requests

Forgot Password requests: `user_id`, `requested_at`, `status` (Waiting / Done / Dismissed), `handled_by`, `handled_at`. The waiting ones are the Admin's "Password requests (N)".

## audit_logs: the edit history

One row per add/edit/delete anywhere in the app: `user_id` (who), `department` (which Edit History tab shows it), `table_name`, `record_id`, `record_name`, `action` (Added / Edited / Deleted), `old_values` and `new_values` (jsonb), `changed_at`.

Example `new_values` for an edited project:

```json
{ "status": "Done", "completed_date": "2026-10-30" }
```

Private values (passwords, government IDs, bank account numbers) are never written here.

# 6. Projects and HR tables

## projects

`project_id`, `project_name`, `site_location`, `client_name`, `sector` (Residential / Commercial / Industrial / Repairs / Renovations), `contract_price`, `start_date`, `target_end_date`, `completed_date` (set by the app when status becomes Done), `status` (Pending / Ongoing / Done / Cancelled), `show_on_website`.

Check rules: price ≥ 0; target end and completed date not before the start.

## employees

`employee_id`, `full_name`, `contact_number`, `address`, `position`, `department` (Administration / HR & Payroll / Inventory / Subcontractors / Receipts / Site), `pay_schedule` (Office / Site, must match the department), `pay_grade`, `biometric_id` (unique), `gov_deductions` (yes/no), `sss_no`, `philhealth_no`, `pagibig_no`, `tin`, `status` (Active / Inactive).

## employee_rates: daily rate history

`rate_id`, `employee_id` → employees, `daily_rate` (> 0), `effective_from`. Unique per employee and date.

The rate on any day = the row with the **latest `effective_from` that isn't after that day**.

```
employee_rates for Juan
effective_from | daily_rate
2026-01-01     |  600.00
2026-11-02     |  650.00

Rate on 2026-10-30 -> 600.00   (2026-11-02 hasn't started yet)
Rate on 2026-11-05 -> 650.00
```

This is why old payslips never change when someone gets a raise.

## Attendance (script 012)

- **attendance_imports**: one row per uploaded scanner file: `file_name`, `period_from` / `period_to` (the range printed in the file), `scans_added`, `scans_skipped` (already there), `rows_rejected`, `problems` (why), `imported_by` → users.
- **attendance_scans**: every fingerprint scan: `employee_id` → employees, `scan_time`, `import_id` → attendance_imports. **Unique per employee and time**, which is why uploading the same file twice adds nothing.
- **attendance_days**: one row per employee per day, worked out from the scans: `time_in`, `break_out`, `break_in`, `time_out`, `late_minutes`, `early_minutes`, `ot_minutes`, `ot_status` (None / Waiting / Approved / Rejected), `status` (Present / Late / Missing Scan / Absent / Rest Day Work), and when HR fixed it by hand: `fix_reason`, `fixed_by`, `fixed_at`. Unique per employee and date.

```sql
-- Who was late in October, and by how much
SELECT e.full_name, d.work_date, d.time_in, d.late_minutes
FROM attendance_days d JOIN employees e ON e.employee_id = d.employee_id
WHERE d.work_date BETWEEN '2026-10-01' AND '2026-10-31' AND d.late_minutes > 0
ORDER BY d.work_date;

-- Overtime still waiting for the HR Supervisor
SELECT e.full_name, d.work_date, d.ot_minutes
FROM attendance_days d JOIN employees e ON e.employee_id = d.employee_id
WHERE d.ot_status = 'Waiting';
```

## Payroll (script 013)

- **holidays**: `holiday_date` (the key), `name`, `kind` (Regular = paid when not worked; Special = no work, no pay).
- **cash_advances**: `employee_id` → employees, `given_on`, `amount`, `per_payday`, `balance` (what's still owed), `reason`, `status` (Waiting / Active / Paid Off / Rejected).
- **payroll_runs**: one per pay group and period: `pay_group` (Office / Site), `period_from`, `period_to`, `pay_date`, `status` (Draft / Final). Unique per group and start date.
- **payslips**: one per employee per run: `daily_rate`, `days_paid`, `basic_pay`, `holiday_days`, `holiday_pay`, `late_minutes`, `late_deduction`, `ot_minutes`, `ot_pay`, `gross_pay`, `sss`, `philhealth`, `pagibig`, `tax`, `cash_advance`, `net_pay` (never below 0).
- **cash_advance_deductions**: what each payslip took from each advance: `advance_id`, `payslip_id`, `amount`, `balance_after`.
- Work hours, overtime rate and government rates live in **app_settings** with keys starting `hr.` (e.g. `hr.ot_rate`).

```sql
-- Net pay of every payslip in finalized payrolls, newest first
SELECT r.pay_date, r.pay_group, e.full_name, p.gross_pay, p.net_pay
FROM payslips p
JOIN payroll_runs r ON r.run_id = p.run_id
JOIN employees e ON e.employee_id = p.employee_id
WHERE r.status = 'Final'
ORDER BY r.pay_date DESC, e.full_name;

-- Who still owes cash advances
SELECT e.full_name, c.amount, c.balance FROM cash_advances c
JOIN employees e ON e.employee_id = c.employee_id
WHERE c.status = 'Active';
```

# 7. Inventory tables

```
vendors <--- vendor_documents
   ^
   |---- quotes ----> materials <---- material_usages ----> projects
   |       ^              ^
   |---- purchases -------|  (purchase.quote_id -> the chosen quote)
   |       ^              |
   +---- deliveries ------+  (delivery.purchase_id -> the purchase it fills)

equipment <--- maintenance_logs
```

| Table | Holds | Notes |
|---|---|---|
| `materials` | name (unique), category, unit, `unit_price`, `stock_qty`, `minimum_stock` | Stock can't go below 0 |
| `material_usages` | material, project, quantity, date, `status` (Waiting / Approved / Rejected), `cost` | `cost` is filled in **when approved**: quantity × price at that moment |
| `vendors` | name (unique), contacts, bank details, `vat_status` (VAT / Non-VAT) | |
| `vendor_documents` | vendor, `doc_type`, file path | |
| `quotes` | material, vendor, product, price, quantity, `status` (Open / Selected / Not Selected) | Approving one sets it Selected and the others Not Selected |
| `purchases` | the selected `quote_id` (unique), material, vendor, quantity, price | One purchase per quote |
| `deliveries` | purchase (optional), vendor, material, quantity, price, receipt no., scan **or** paper + filed location | Check rule: needs a scan, or paper with a location |
| `equipment` | name, `type` (Tool / Heavy Equipment / Vehicle), `ownership` (Owned / Rented), rental details, `status` (Available / Under Repair / Retired), `next_maintenance` | Check rule: rented needs company, rate and rate unit |
| `maintenance_logs` | equipment, date, `kind` (Preventive / Repair), work done, `cost`, done by | Costs are company overhead |

**How stock stays correct:** the app changes `materials.stock_qty` only in two places, inside the same save as the record that explains it:

- a usage is approved → stock goes down;
- a delivery is saved → stock goes up.

"Inside the same save" (a **transaction**) means both happen or neither does, so the stock and its history can never disagree.

# 8. Subcontractor tables

```
subcontractors <--- subcontractor_trades
      ^
      |
   sub_jobs ----> projects
      ^
      |--- sub_job_stages <--- sub_vouchers <--- sub_payouts
      |--- sub_job_documents       |
      |--- sub_progress            +--> sub_jobs
```

| Table | Holds | Notes |
|---|---|---|
| `subcontractors` | business name (unique), trade name, DTI/SEC, TIN, two contacts, addresses, bank details, PCAB license and expiry | |
| `subcontractor_trades` | one row per ticked trade | A many-values list for one subcontractor |
| `sub_jobs` | subcontractor, project, scope, `contract_amount` (> 0), dates, `retention_pct` (0 to under 100) | |
| `sub_job_stages` | job, `stage_no`, name, `percent`, `reached_on` | Unique per job and stage number |
| `sub_job_documents` | job, file | Contract and scope papers |
| `sub_progress` | job, `percent` (0-100), date, notes, file | The latest row is the job's progress |
| `sub_vouchers` | `voucher_no` (unique, SV-0001), job, stage (empty for retention), `kind` (Stage / Retention), `gross_amount`, `retention_amount`, `payable_amount`, `status` (Waiting / Approved / Rejected / Paid) | Partial unique rules: one non-rejected voucher per stage, and one per job's retention |
| `sub_payouts` | voucher (unique: paid once), job, date, amount, reference, `method` (Bank Transfer / Check / Cash) | These are the project's subcontractor expenses |

**Why `subcontractor_trades` is its own table:** one subcontractor can have many trades. A database column holds one value, so a list becomes its own table with one row per item. This is called **normalization**.

# 9. Receipts and change orders

## receipts

`receipt_id`, `receipt_no`, `purchase_date`, `merchant`, `amount` (> 0), `category`, `expense_type` (Project / Overhead), `project_id`, the copy (`file_path` + `original_name`, or `physical_only` + `filed_location`, or `no_receipt` + `no_receipt_reason`), `status` (Waiting / Approved / Rejected).

Two check rules do a lot of work here:

- `expense_type` = Project **exactly when** `project_id` is filled;
- exactly **one** kind of copy: a scan, paper with a location, or "no receipt" with a reason.

**Stock purchases aren't in this table.** The Receipt Vault reads delivery receipts straight from `deliveries` and shows them as "Stock purchase". Storing them once means they can never be counted twice.

## change_orders

`co_no` (unique, CO-001), `project_id`, `requested_on`, `description`, `cost_change` (+/-), `days_change` (+/-), signed document, `status`. Check rule: the cost change or the days change must be non-zero.

# 10. How the money is calculated

The app adds money up in exactly one place (`ProjectFinance` in the code). In database terms, for one project:

| Part | Comes from |
|---|---|
| Contract price | `projects.contract_price` |
| + Change orders | sum of `change_orders.cost_change` where status = Approved |
| **= Revenue** | |
| Materials | sum of `material_usages.cost` where status = Approved |
| + Receipts | sum of `receipts.amount` where Project and Approved |
| + Subcontractors | sum of `sub_payouts.amount` (through `sub_jobs.project_id`) |
| **= Expenses** | |
| **Profit** | Revenue - Expenses |

**Overhead** (Dashboard only): `receipts` where Overhead and Approved + `maintenance_logs.cost`, counted by date.

The same idea written as SQL, giving every project's revenue and expenses (read only):

```sql
SELECT p.project_name,
       p.contract_price
         + COALESCE((SELECT SUM(cost_change) FROM change_orders c
                     WHERE c.project_id = p.project_id AND c.status = 'Approved'), 0) AS revenue,
       COALESCE((SELECT SUM(cost) FROM material_usages u
                 WHERE u.project_id = p.project_id AND u.status = 'Approved'), 0)
     + COALESCE((SELECT SUM(amount) FROM receipts r
                 WHERE r.project_id = p.project_id AND r.status = 'Approved'), 0)
     + COALESCE((SELECT SUM(o.amount) FROM sub_payouts o JOIN sub_jobs j ON j.job_id = o.job_id
                 WHERE j.project_id = p.project_id), 0) AS expenses
FROM projects p;
```

`COALESCE(x, 0)` means "use 0 when there's nothing to add up".

# 11. The Data Flow Diagram (DFD)

## 11.1 What a DFD is

A DFD shows **how information moves**: who sends what, which process handles it, and where it's stored. It doesn't show screens or code. The file is `docs\diagrams\Desktop_App_DFD.drawio` (open it at **app.diagrams.net**). It has 7 pages.

| Symbol | Means | Example |
|---|---|---|
| Rectangle | **External entity**: someone or something outside the system | Employee, Client, Biometrics Device |
| Rounded box with a number | **Process**: work the system does | 3.0 Materials & Inventory Management |
| Open-ended box with D-number | **Data store**: where information is kept | D3.1 Material Inventory |
| Arrow with a label | **Data flow**: information moving | "Receipt scan & metadata" |

## 11.2 The levels

- **Level 0 (Context):** the whole system as **one** process (0, the Desktop Management Supersystem), surrounded by the outside world: the Admin user, employees, the biometrics device, clients, subcontractors, suppliers/vendors, external accounting and the website system.
- **Level 1:** that one process split into the five main parts:

| Process | Name | In the app |
|---|---|---|
| 1.0 | Main Dashboard & Analytics | Dashboard page |
| 2.0 | Employee & Payroll Management (HRIS) | HR & Payroll + User Accounts |
| 3.0 | Materials & Inventory Management | Inventory page |
| 4.0 | Subcontractor & Vendor Management | Subcontractors page |
| 5.0 | Receipt & Tracking | Receipts page |

- **Level 2:** one page per part, splitting it into smaller processes (for example 2.2 System Access & User Account Management, 2.5 Absence & Tardy Tracking, 4.3 Scope Assignment & Progress Tracking).

## 11.3 DFD data stores → real tables

A data store in the DFD is a **concept**; in the database it can be one table or several.

| DFD store | Real tables |
|---|---|
| D1 Central Database | The whole `construction_db` |
| D1.1 Project & Financial Records | `projects` + the money tables in chapter 10 |
| D1.2 Website Traffic Records | *Not built (website part)* |
| D2.1 Employee Records | `employees`, `employee_rates` |
| D2.2 User Accounts & Roles | `users` (all logins, including Website Admins), `login_logs`, `password_requests` |
| D2.3 Attendance & Site Schedule Records | `attendance_imports`, `attendance_scans`, `attendance_days` |
| D2.4 Tax Brackets & Statutory Tables | `app_settings` (`hr.*` rates) + the TRAIN tax table in the code (`PayrollService.AnnualTax`), `holidays` |
| D2.5 Payroll Records | `payroll_runs`, `payslips`, `cash_advances`, `cash_advance_deductions` |
| D3.1 Material Inventory | `materials`, `material_usages`, `deliveries` |
| D3.2 Equipment Records | `equipment` |
| D3.3 Site Deployment Records | *Not built: equipment location tracking was dropped (owner decision)* |
| D3.4 Maintenance Logs | `maintenance_logs` |
| D3.5 Quotation Records | `quotes`, `purchases`, `vendors`, `vendor_documents` |
| D4.1 Subcontractor Records | `subcontractors`, `subcontractor_trades` (ratings were dropped for now) |
| D4.2 Scope & Progress Records | `sub_jobs`, `sub_job_stages`, `sub_job_documents`, `sub_progress` |
| D4.3 Billing & Payout Records | `sub_vouchers`, `sub_payouts` |
| D5.1 Receipt Records | `receipts` (+ delivery receipts read from `deliveries`) |
| D5.2 Change Order Records | `change_orders` |
| D5.3 Project & Contract Records | `projects` (the same table as D1.1) |
| *(not in the DFD)* | `audit_logs` (edit history), `app_settings`, `schema_versions` |

**Where the DFD and the built app differ:** the DFD was drawn before some decisions were made. Site deployment tracking and subcontractor ratings were dropped, and the DFD's "approved / pending / denied" account statuses became Active / Locked / Disabled. Website Admin is no longer part of the office app: it has its own screens, opened by the same Tri-Stone Sign In with Website Admin logins (one account list). If the DFD is used for a presentation, update those parts so it matches.

## 11.4 Reading one flow end to end

Example, **a material used on a project** (process 3.0):

1. *Employee* → "material usage entry" → **3.0** → saved in **D3.1** as a `material_usages` row, status Waiting.
2. *Supervisor* approves → **3.0** lowers `materials.stock_qty` and fills `material_usages.cost`.
3. **1.0 Dashboard** reads **D1.1** → the project's expenses now include that cost.

# 12. Try it yourself in pgAdmin

1. Open **pgAdmin 4**, then **Servers > PostgreSQL 18 > Databases > construction_db**.
2. Click **Tools > Query Tool**.
3. Paste a query and press **F5**.

> **Only use `SELECT` queries** (they only read). Never run `UPDATE`, `DELETE` or `DROP` on the real database: they change or destroy data permanently, skip the app's rules and leave no edit history. Practice on the work PC's test database.

```sql
-- 1. All tables and how many rows each has
SELECT relname AS table_name, n_live_tup AS rows
FROM pg_stat_user_tables ORDER BY relname;

-- 2. Every login with its level and status
SELECT username, full_name, level, department, status FROM users;

-- 3. The last 20 sign-in events
SELECT logged_at, username_tried, action, details, computer_name
FROM login_logs ORDER BY logged_at DESC LIMIT 20;

-- 4. Projects that are still running
SELECT project_name, client_name, contract_price, start_date
FROM projects WHERE status = 'Ongoing' ORDER BY start_date;

-- 5. Materials below their minimum stock
SELECT name, stock_qty, minimum_stock, unit
FROM materials WHERE stock_qty < minimum_stock;

-- 6. JOIN: each receipt with its project's name
SELECT r.receipt_no, r.amount, r.status, p.project_name
FROM receipts r
LEFT JOIN projects p ON p.project_id = r.project_id;

-- 7. GROUP BY: approved receipts per category
SELECT category, COUNT(*) AS receipts, SUM(amount) AS total
FROM receipts WHERE status = 'Approved'
GROUP BY category ORDER BY total DESC;

-- 8. Which scripts have built this database
SELECT * FROM schema_versions ORDER BY version;
```

**Words in those queries:**

| Word | Means |
|---|---|
| `SELECT ... FROM` | Which columns, from which table |
| `WHERE` | Only rows that match |
| `ORDER BY ... DESC` | Sort, biggest/newest first |
| `LIMIT 20` | Only the first 20 rows |
| `JOIN ... ON` | Combine two tables through their link (foreign key) |
| `LEFT JOIN` | Keep the left table's rows even when there's no match (overhead receipts have no project) |
| `GROUP BY` | Make one result row per group (per category) |
| `COUNT`, `SUM` | Count rows / add up values |

In pgAdmin you can also browse visually: open **Schemas > public > Tables**, right-click a table, then **View/Edit Data > First 100 Rows** to look (don't edit there), or **Properties > Columns** and **Constraints** to see its columns and rules.

# 13. Keeping it safe (Phase 6)

When the database moves to the office server:

- It's reachable **only** from the office network, never from the internet.
- The app gets its **own limited database account** (read and write the app's tables only), not the all-powerful `postgres` account.
- **Automatic daily backups**, copied off the server. A backup is a file (made with `pg_dump`) that can rebuild the whole database.
- **Strong passwords** for the database and every Admin.

# 14. Glossary

| Word | Meaning |
|---|---|
| **Database / DBMS** | The organized store of data / the program that runs it (PostgreSQL) |
| **Schema** | The design of the database: its tables, columns, links and rules |
| **Table, row, column** | A list of records / one record / one kind of information |
| **Primary key (PK)** | A row's permanent unique ID |
| **Foreign key (FK)** | A column holding another table's ID; a link |
| **Constraint** | A rule the database enforces (NOT NULL, CHECK, UNIQUE, FK) |
| **Index** | A lookup aid that makes searches fast; unique indexes also enforce uniqueness |
| **NULL** | Empty / unknown value |
| **Transaction** | Several changes saved as one: all or nothing |
| **Normalization** | Splitting data into tables so each fact is stored once |
| **SQL** | The language used to ask the database questions and give it changes |
| **Hash** | A one-way scramble; used for passwords |
| **jsonb** | A column type holding a small bundle of named values |
| **EF Core** | The library that turns rows into C# objects and back |
| **Migration / script** | A numbered file that changes the database's design |
| **DFD** | Data Flow Diagram: how information moves between people, processes and stores |
| **ERD** | Entity-Relationship Diagram: the tables and their links (the box-and-arrow maps in chapters 4, 7 and 8) |
