# Tri-Stone Office App: Developer Guide

How the Tri-Stone Construction Services system is put together: the tools, the database, how the code is organized, and the main rules of each part. It explains the design, not every line. The code itself is in `src\`. The older full-length guide with every source file printed out is kept in `docs\_archive`.

> **What we are building.** One Windows desktop app (WinForms, .NET 10) for the office, connected directly to a PostgreSQL database. A small ASP.NET Core Web API sits next to the database only for the public website. Everything runs on `localhost` while building; it moves to the Linux Mint server (`192.168.1.50`) at the very end.

| Part | What it is | Status |
|---|---|---|
| 0 | Login & Security | Built |
| 1 / 1b | Dashboard / Projects | Built |
| 2 | HR & Payroll (and User Accounts) | Employees built; Attendance and Payroll next |
| 3 | Inventory & Vendors | Built |
| 4 | Subcontractors | Built |
| 5 | Receipts | Built |
| 6 | Website Admin (a separate desktop app with its own accounts) | Not started |

# Chapter 1: Setting up a development PC

## 1.1 Tools

| Tool | Version | Why |
|---|---|---|
| Visual Studio | 2026, ".NET desktop development" + "ASP.NET and web development" | Writing and designing the app |
| .NET SDK | 10.0 | Builds the app and the API |
| PostgreSQL + pgAdmin 4 | 18 | The database |
| Git | 2.48 or newer | Change history |
| Node.js + Microsoft Word | Node 22 or newer | Only for rebuilding these Word guides |

## 1.2 The project folder

```
TriStone\
├── TriStone.slnx              open this in Visual Studio
├── src\
│   ├── Company.OfficeApp\     the desktop app (WinForms)
│   ├── Company.Api\           Web API, for the public website only
│   └── Company.Shared\        data classes + database context, used by both
├── database\                  numbered SQL scripts + setup-database.cmd
└── docs\                      outline, guides, diagrams, and the tools that build them
```

The website must never touch the database itself, which is why the API is a separate project. `Company.Shared` holds the classes both apps need, so a change is made once.

## 1.3 Getting it running

1. Install the tools above.
2. Create `%APPDATA%\postgresql\pgpass.conf` with one line, `localhost:5432:*:postgres:YourPassword` (Notepad, **Save as type: All Files**). This lets the scripts log in without asking.
3. Double-click `database\setup-database.cmd`. It creates `construction_db` and runs every script.
4. Add the first Admin (section 2.3).
5. Open `TriStone.slnx`, set **Company.Launcher** (Tri-Stone) as the startup project and press **F5**. The first run asks for the database settings.

Every build puts the finished program in one easy folder at the top of the project: **`Apps\Tri-Stone\TriStone.exe`**, the **only** program. It signs everyone in and opens the office screens or the Website Admin screens (chapter 10). Make a desktop shortcut to it. The folder is rebuilt every time and isn't saved in Git.

**Forgotten PostgreSQL password:** in `C:\Program Files\PostgreSQL\18\data\pg_hba.conf` change `scram-sha-256` to `trust`, restart the `postgresql-x64-18` service, run `ALTER USER postgres WITH PASSWORD '...';` in pgAdmin, then change it back and restart again.

# Chapter 2: The database

## 2.1 Numbered scripts

The database is never built by clicking in pgAdmin. Every change is a numbered script in `database\`, and `setup-database.ps1` runs the ones not run yet, each as all-or-nothing. A table called `schema_versions` remembers which have run. Any PC, and later the server, gets exactly the same database.

**Rule:** a script that has run is never edited. A change means a new script with the next number.

| Script | Adds |
|---|---|
| 001 | `users`, `login_logs`, `password_requests`, `audit_logs` |
| 002 | `projects` |
| 003 | `employees`, `employee_rates` |
| 004 | one login per employee (unique index) |
| 005 | `app_settings`, materials, usage, vendors, quotes, purchases, deliveries, equipment, maintenance |
| 006 | subcontractors, trades, jobs, stages, documents, progress, vouchers, payouts |
| 007 | `receipts`, `change_orders` |
| 008 | `website_accounts`, `website_login_logs` (the Website Admin app's own accounts; merged into `users` by 011) |
| 009 | `website_accounts.is_main`: main accounts manage the other Website Admin accounts (removed by 011) |
| 010 | `inquiries`: website Contact form messages and their follow-up |
| 011 | One account list: the Website Admin level in `users`; website accounts from 008/009 moved in, their tables removed |

## 2.2 Conventions

- Dates without a time are `DATE` (C# `DateOnly`). Timestamps are plain `timestamp`.
- Money is `NUMERIC(14,2)`, never floating point, so pesos add up exactly.
- Important rules are also checked by the database itself (for example, an Employee must have a department and an Admin must not), so a bug in the app can't break them.

## 2.3 The first Admin

There is no sign-up screen, so the first Admin is added once in pgAdmin (**Tools > Query Tool** on `construction_db`):

```sql
INSERT INTO users (username, password_hash, full_name, level)
VALUES ('YourUsername', crypt('YourPassword', gen_salt('bf', 11)), 'Your Full Name', 'Admin');
```

Only a bcrypt hash is stored. Never write the real password in a file or in Git. Every other account is created inside the app.

# Chapter 3: How the app is organized

## 3.1 Start-up

`Program.cs`: check the database (show **Database Settings** if it can't connect), then **Login**, then the **Main window**. After Sign Out it goes back to Login.

Each PC keeps its own database settings in `C:\ProgramData\TriStone\OfficeApp\dbsettings.json`. The password in it is encrypted with Windows DPAPI, so the file is useless on another PC. This is what lets the same app work on `localhost` now and on the server later without rebuilding.

## 3.2 Where things live

| Folder | What goes there |
|---|---|
| `Company.Shared/Models` | One class per table |
| `Company.Shared/Data/AppDbContext.cs` | Maps the classes to the tables (the tables come from the SQL scripts, not from Entity Framework) |
| `Company.OfficeApp/Services` | **All the rules.** Static, async methods that return an error message or `null` |
| `Company.OfficeApp/Pages` | One `UserControl` per sidebar page; big pages keep each tab in its own subfolder (`Pages/Inventory`, `Pages/Receipts`...) |
| `Company.OfficeApp/Forms` | Windows: Login, Main, Database Settings, pop-ups |
| `Company.OfficeApp/UI` | `Responsive` helpers |

Pages only show data and call services. A service re-checks who is signed in before doing anything, so a rule can't be skipped by reaching the code another way.

## 3.3 Main window

A sidebar on the left, a top bar (page title, who is signed in, the Admin's notification button), and `pnlContent`, where `MainForm.ShowPage` loads the chosen page.

Who sees what is decided in one class, `Services/Access.cs`:

| Page | Admin | Supervisor | Employee |
|---|---|---|---|
| Dashboard | yes | own department's numbers | no |
| Projects, User Accounts | yes | no | no |
| HR & Payroll / Inventory / Subcontractors / Receipts | all | own department | own department |
| My Account | yes | yes | yes |

Approve buttons use `Access.IsApprover(user, department)`: Admins plus that department's Supervisor.

## 3.4 Shared building blocks

Every new page should reuse these instead of writing its own:

| Piece | What it does |
|---|---|
| `AuditService.Add` | Writes a row to `audit_logs` for every add, edit and delete. Shown in each department's **Edit History** tab (`Pages/Shared/EditHistoryTab`). Never log passwords, hashes, security answers, government ID numbers or bank account numbers. |
| `ProjectFinance` | The **only** place project revenue and expenses are added up. Projects, the project report and the Dashboard all use it, so they always agree. |
| `FileStore` | Saves uploaded scans into the storage folder (`app_settings.file_storage_path`); the database keeps only the relative path. PDF, images and Word, up to 25 MB. |
| `ExcelExport` | Every Export button (ClosedXML). |
| `TableReport` | Prints any list over several pages, or saves it as PDF. |
| `Responsive` | `FitGrid` makes tables grow with the window; `OnTypingPause` waits for the user to stop typing before searching; `Busy` shows the wait cursor; `SelectRow` highlights the record just saved after the table reloads. |

A new list page usually starts like this:

```csharp
Responsive.FitGrid(this, dgvProjects);
Responsive.OnTypingPause(txtProjectSearch, () => LoadProjectsAsync());
```

and after a save, the reload highlights the saved record again:

```csharp
Responsive.SelectRow(dgvProjects, (ProjectRow row) => row.ProjectId == id);
```

## 3.5 Rules of thumb for the code

- Read a grid's selection from `SelectedRows`, not `CurrentRow` (after "Add", the grid still has a current row).
- **Never let a grid select a row by itself.** After setting `DataSource`, call `ClearSelection()` (with a `filling` flag so `SelectionChanged` ignores the reload), then re-select only the record you mean. Otherwise the form silently edits the first row, and typing a "new" record overwrites it (bug found on Employees, 2026-10-30).
- **A grid loaded while its tab is hidden still selects its first row when the tab is first shown.** Every list page calls `RecordForm.NoAutoSelect(grid, on => filling = on, RestoreSelection)` in its constructor: the selection made while the grid first appears is ignored, then only the record being shown is re-selected.
- **Adding and editing are separate on every list page (`UI/RecordForm`):**
  - Add opens a pop-up that is a second copy of the same page made with `new XPage(addOnly: true)`: `RecordForm.ShowFormOnly` hides the list (everything above the form's title label) and record-only parts, and `RecordForm.AddDialog` shows it until the page raises `Added(id)`. Each form exists once. The add-only copy never loads the list (its hidden filters fire on start-up).
  - The page form only edits the clicked record; `RecordForm.SetEnabled` locks it when nothing is selected.
  - Save snapshots `ReadForm()` when the record loads and calls `RecordForm.ConfirmSave(original, input, labels, privateFields)`, which lists the changes in a Yes/No message.
  - Employees uses its own `EmployeeEntryForm` pop-up (built first, same behaviour).
- The main window is borderless and maximized to the screen's working area (`FormBorderStyle = None`, `ControlBox = false`, `MaximizedBounds` set in the constructor), so it can't be resized, moved or closed by accident.
- To clear a dropdown, set `SelectedIndex = -1`, not `Text = ""`. Clearing only the text keeps the old choice selected underneath, and picking it again doesn't register (bug fixed on the Materials form).
- Control names follow the UI Elements Guide (`btn`, `txt`, `cbo`, `dgv`...).
- Screens stay plain, default WinForms until they are styled from the Figma design.
- Big `.Designer.cs` files can be generated once with `docs/_tools/designer-gen`. Never regenerate a form after it has been edited in Visual Studio's designer.

# Chapter 4: Login & Security (Part 0)

**Sign-in.** Usernames ignore capital letters. 5 wrong passwords lock the account until an Admin unlocks it. Every sign-in, sign-out, failure, lock and password change goes to `login_logs` with the PC name. After 5 minutes without activity a "Still there?" popup appears, and the app signs out by itself after 60 more seconds. In Debug builds, the `TRISTONE_IDLE_SECONDS` environment variable shortens the wait for testing.

**Passwords.** Only Admins create accounts and set passwords. Rules: 8+ characters with a letter and a number. **Forgot Password** sends a request to the Admins, who set a new password and give it in person. An Admin who forgets answers their own security question (set in **My Account**).

**User Accounts** (Admin only). Every login belongs to one Active office employee and takes its name and department from them. Administration staff only get Admin logins. You can't disable yourself or remove your own Admin level, and the last active Admin is protected.

# Chapter 5: Projects and Dashboard (Part 1)

**Projects** (Admin only) is the one shared project list; other departments only pick from it. The completion date is set automatically when the status becomes Done. A project can't be deleted once anything links to it (`ProjectService.CountLinkedRecordsAsync`); set it to Cancelled instead.

**Project money** (`ProjectFinance`):

- Revenue = contract price + approved change orders.
- Expenses = approved material use + approved project receipts + subcontractor payouts.
- Payroll and equipment repairs are company overhead, not project expenses.

**Dashboard** uses the completed-contract method: a project's revenue and expenses count in the period it is **finished**, and overhead counts by its own date. Supervisors only see their own department's expenses; revenue and net are hidden from them. The chart is ScottPlot (NuGet `ScottPlot.WinForms`). Log History (Admin only) reads `login_logs`.

# Chapter 6: HR & Payroll (Part 2)

**Built: Employees.** Office staff and site workers. The pay schedule follows the department: Site = weekly on Saturday, everyone else = the 15th and 30th. The daily rate is kept as a dated history (`employee_rates`), so past payslips keep the rate they were paid with. Biometric IDs are unique. Marking someone Inactive turns their login off; Reactivate doesn't turn it back on.

**Still to build:** Attendance (waiting for the fingerprint scanner's sample export file), Payroll, Cash Advances, Payslips.

# Chapter 7: Inventory & Vendors (Part 3)

- **Stock is never typed in** after a material is added. It goes down only through approved usage and up only through deliveries.
- **Usage:** the Employee records, the Supervisor approves. Then the stock drops and the cost goes to the project.
- **Price comparison:** the Supervisor approves one quote as the purchase; the other open quotes close.
- **Deliveries:** no approval needed. Each needs a scan, or "physical copy only" with where the paper is filed. Receipts shows them as "Stock purchase" without copying them, so they are never counted twice.
- **Equipment:** owned or rented, with a maintenance log and a next-maintenance reminder. Repair costs are overhead.

# Chapter 8: Subcontractors (Part 4)

- A **job** = subcontractor + project + scope + amount + dates + retention % (default 10%).
- **Payment stages** must total 100%. The last stage takes any rounding centavos, so the stages always add up to the contract exactly.
- **Billing order:** the Employee marks a stage reached and prepares the voucher (SV-0001...), the Supervisor approves, then the payout is recorded. Once billing starts, the amount and stages are locked.
- **Retention** is released only when the job is 100% done and every stage is paid.
- Payouts are project expenses. PCAB licenses expiring within 30 days are flagged.

# Chapter 9: Receipts & change orders (Part 5)

- Each receipt is either for a **Project** or **Company overhead**. The Employee adds, the Supervisor approves, and only approved receipts count.
- **No receipt:** needs a reason and waits in **Needs Review** for the Supervisor.
- **Change orders** (CO-001...): logged by Receipts staff, approved by an **Admin only**. On approval the cost change is added to the project's revenue and the days move its target end date.
- Reports print over several pages or save as PDF (`TableReport`).

# Chapter 10: The Website Admin app (Part 6)

## 10.1 Separate screens, one Sign In

The public website is managed in **separate screens**, not in the office app (owner decisions, 30 Oct 2026), but with **one account list** and **one Sign In** for everyone.

- **Tri-Stone** (`src/Company.Launcher`, built to `Apps\Tri-Stone\TriStone.exe`) is the one program people open. It shows the office app's Sign In window, then opens the right app for the login:

  | Login level | Opens |
  |---|---|
  | Website Admin | the Website Admin app |
  | Admin | asks: Office App, Website Admin, or Sign Out (`AppChoiceForm`) |
  | Supervisor, Employee | the office app |

  After Sign Out (or the idle sign-out) it returns to Sign In.
- `src/Company.WebsiteAdmin` is a **library** with the Website Admin screens. It references the office app, so it uses the same sign-in, session, database settings and screen helpers.
- The main window is full screen with no title bar and looks like the office app: a sidebar on the left (Page Content, Announcements, Website Projects, Document Vault, Inquiries, Website Traffic, Sign Out), the page title and who is signed in at the top, and the chosen page in the middle. Same "Still there?" idle check. Pages are filled in one by one.
- `src/Company.OfficeApp` is a library too (no Program.cs): Tri-Stone is the only program, so nobody can open one app alone and get the wrong Sign In (owner request, 30 Oct 2026). In Visual Studio, **Company.Launcher** must be the startup project.

To run it from Visual Studio: right-click **Company.Launcher > Set as Startup Project**, then press **F5**.

## 10.2 Website Admin logins

**Website Admin** is a **Level** in User Accounts, next to Admin, Supervisor and Employee (script 011). Office Admins create and manage these logins like any other: linked to an office employee (any office department, including Administration), and, like Admins, without a department. All the usual rules apply: password rules, lock after 5 wrong passwords, Forgot Password goes to the Admins, every sign-in in `login_logs`.

`UserLevels.HasNoDepartment(level)` is true for Admin and Website Admin; `User.CanOpenWebsiteAdmin` is true for Website Admins and Admins, and every Website Admin service re-checks it.

**History:** at first the Website Admin app had its own accounts (scripts 008 and 009, with "main" accounts managing the rest). Script 011 moved those accounts into `users` as Website Admins, keeping their passwords, sign-in history and inquiry follow-ups, and then removed the separate tables.

## 10.3 Inquiries

Website Contact form messages land in `inquiries` (script 010); the API will add them later. Website Admins and Admins can follow them up: status New / Contacted / Closed and internal notes (never shown on the website). Inquiries are never deleted, so nothing a client sent is lost. The database refuses an inquiry with neither an email nor a phone number. `Services/WebsiteInquiries` lists, counts and saves; the sidebar button shows "Inquiries (N new)".

# Chapter 11: Testing

**Two PCs.** The home PC has the real database. The work PC has its own fresh database with test data only. Every test run on the work PC is written in `docs\Testing Log.md`, and anything that still needs the real data is listed there under "Check at home".

Each part was tested with a small throwaway console program that runs real checks against the real database and opens the real windows. Test data is named `zz_...` (users) or `ZZTEST...` (records) and deleted afterwards with a cleanup script, child tables first.

| Part | Checks passed |
|---|---|
| Sign-in, Forgot Password, idle, My Account, User Accounts | 85 |
| Projects | 23 |
| Employees + linked logins | 52 |
| Responsive pages | 11 |
| Inventory | 38 |
| Subcontractors | 56 |
| Receipts | 55 |
| Dashboard | 24 |
| Row re-select after saving (Projects, Materials, Employees) | 11 |
| Add pop-ups, no auto-select, confirm before saving (Employees + 8 list pages) | 86 |
| Website Admin app: sign-in, lock-out, main window | 16 |
| Website Admin accounts (main only; replaced by script 011) | 22 |
| Website Admin Inquiries | 18 |
| Office tabs opened through the sidebar: nothing auto-selected | 10 |
| Website Admin app sidebar | 9 |
| One Sign In (Tri-Stone): routing by level, Admin choice, office app refusing Website Admin logins | 13 |
| Website Admin level in User Accounts; who may follow up inquiries | 10 |

**Not tested yet:** the "last active Admin" protection. It needs a separate test database; never disable the real Admin account to test it.

# Coming next

- HR & Payroll: Attendance, Payroll, Cash Advances
- Website Admin app: the remaining website pages (Page Content, Announcements, Website Projects, Document Vault, Website Traffic); then the API and the website Contact form
- Phase 6: the server and the installers
