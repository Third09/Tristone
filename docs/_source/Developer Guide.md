# Tri-Stone: Developer Guide

How the Tri-Stone Construction Services system is put together: the tools, the code structure, and, page by page, which classes, service methods, tables and rules are behind each screen. The database design itself (tables, constraints, how scripts are applied, how money is derived) is in the **Database Guide**. The older full-length guide with every source file printed out is kept in `docs\_archive`.

> **What we are building.** One Windows desktop app (WinForms, .NET 10) for the office, connected directly to a PostgreSQL database. A small ASP.NET Core Web API sits next to the database only for the public website. Everything runs on `localhost` while building; it moves to the Linux Mint server (`192.168.1.50`) at the very end.

| Part | What it is | Status |
|---|---|---|
| 0 | Login & Security | Built |
| 1 / 1b | Dashboard / Projects | Built |
| 2 | HR & Payroll (Employees, Attendance, Payroll, Payslip History, Cash Advances, Settings) | Built |
| 3 | Inventory & Vendors | Built |
| 4 | Subcontractors | Built |
| 5 | Receipts | Built |
| 6 | Website Admin (separate screens, same Sign In) | Inquiries built; other pages next |

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
│   ├── Company.Launcher\      Tri-Stone: the one program people open
│   ├── Company.OfficeApp\     the office screens and their rules (WinForms library)
│   ├── Company.WebsiteAdmin\  the Website Admin screens (WinForms library)
│   ├── Company.Api\           Web API, for the public website only
│   └── Company.Shared\        data classes + database context, used by all
├── database\                  numbered SQL scripts + setup-database.cmd
├── docs\                      outline, guides, diagrams, testing log, tools
└── Apps\Tri-Stone\            built by every build: TriStone.exe (not saved in Git)
```

The website must never touch the database itself, which is why the API is a separate project. `Company.Shared` holds the classes everything needs, so a change is made once. The office and Website Admin screens are libraries; Tri-Stone is the only program people open.

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
| 012 | `attendance_imports`, `attendance_scans`, `attendance_days` |
| 013 | `holidays`, `cash_advances`, `payroll_runs`, `payslips`, `cash_advance_deductions` |

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

`Company.Launcher/Program.cs` (Tri-Stone): check the database (show **Database Settings** if it can't connect), then **Sign In**, then the right **Main window**: the office one, or the Website Admin one for Website Admin logins (Admins choose, chapter 10). After Sign Out it goes back to Sign In.

Each PC keeps its own database settings in `C:\ProgramData\TriStone\OfficeApp\dbsettings.json`. The password in it is encrypted with Windows DPAPI, so the file is useless on another PC. This is what lets the same app work on `localhost` now and on the server later without rebuilding.

## 3.2 Where things live

| Folder | What goes there |
|---|---|
| `Company.Shared/Models` | One class per table |
| `Company.Shared/Data/AppDbContext.cs` | Maps the classes to the tables (the tables come from the SQL scripts, not from Entity Framework) |
| `Company.OfficeApp/Services` | **All the rules.** Static, async methods that return an error message or `null` |
| `Company.OfficeApp/Pages` | One `UserControl` per sidebar page; big pages keep each tab in its own subfolder (`Pages/Hr`, `Pages/Inventory`, `Pages/Subcontractors`, `Pages/Receipts`, `Pages/Shared`) |
| `Company.OfficeApp/Resources` | Embedded files (the scanner's blank Employee Form) |
| `Company.OfficeApp/Forms` | Windows: Login, Main, Database Settings, pop-ups |
| `Company.OfficeApp/UI` | `Responsive` and `RecordForm` helpers (used by both sets of screens) |
| `Company.WebsiteAdmin` | The Website Admin screens (`Forms/MainForm`, `Pages/InquiriesTab`) and their rules (`Services/WebsiteInquiries`) |
| `Company.Launcher` | Tri-Stone: start-up, Sign In routing, the Admin's "which app" window |

Pages only show data and call services. A service re-checks who is signed in before doing anything, so a rule can't be skipped by reaching the code another way.

## 3.3 Main window

A sidebar on the left, a top bar (page title, who is signed in, the Admin's notification button), and `pnlContent`, where `MainForm.ShowPage` loads the chosen page.

Pages with tabs (HR & Payroll, Inventory, Subcontractors, Receipts) keep their `TabControl`, but its tab strip is hidden (`UI/HiddenTabs`), and `MainForm.ShowSubPages` lists the tabs as sub-items (`btnNavTab...`, `Theme.SetActiveSubNav`) under the page's sidebar button (owner request 2026-11-03). The tabs a user may not see are already removed by the page, alert titles ("Materials (2 low)") update the sub-item, and code that uses `SelectedTab` / `SelectedIndexChanged` works as before. A new page with tabs gets this for free: put its `TabControl` directly on the page.

Who sees what is decided in one class, `Services/Access.cs`:

| Page | Admin | Supervisor | Employee |
|---|---|---|---|
| Dashboard | view | own department's numbers | no |
| Projects | view | Receipts Supervisor edits | no |
| User Accounts | edits | no | no |
| HR & Payroll / Inventory / Subcontractors / Receipts | view all | own department: edit, approve, delete | own department: edit |
| My Account | yes | yes | yes |

- `CanSee` (sidebar, `RequirePage` for reads), `CanEdit` (`RequireEdit`), `CanDelete` (`RequireDelete`: Supervisor; also Mark Inactive / Reactivate), `IsApprover` (`RequireApprover`: that department's Supervisor only, never Admins), `CanSeeHistory` (Edit History tabs: Admins + that Supervisor).
- `Access.LockButtons(page, AppPage)` at the end of each page's constructor hides buttons by name: Export/Print/View/Open/Preview/Report/Zoom/Close/Cancel/Refresh and `...Pdf` stay; Approve/Reject/Finalize are left to the page (`IsApprover`); Delete/MarkInactive/Reactivate need `CanDelete`; any other button needs `CanEdit`. **Name new buttons by this rule.** When the user can't edit, it calls `RecordForm.MarkReadOnly`, so `RecordForm.SetEnabled` keeps the form fields locked.

## 3.4 Shared building blocks

Every new page should reuse these instead of writing its own:

- `UI/Theme` (the look, like a CSS file): Figma colors as constants; `Theme.Enable()` (Launcher `Main`) styles every window when it opens and every control added later; buttons by name prefix (`KindOf`: Main orange, Danger red, Publish green, Nav sidebar); `SetActiveNav` marks the open page. Only colors/borders/flat style change, never sizes. A color, font or back color set in the designer is kept (`ShouldSerialize` check). Buttons are drawn rounded by the theme (`Round`, optional icon with `SetIcon`). `UI/ThemeControls`: `Card` (rounded white section), `InputFrame` (rounded frame around a borderless TextBox), `DropZone` (dashed picture box: click or drag a file), `StatusBox` (orange / green / red message). Tables (`StyleGrid`): lines between all cells, a gray header, every other row shaded, a gray area under the last row, a border drawn on the parent just outside the table, and "No records to show" when empty. `SetSelected` = light-orange "chosen" button (About Us sections). Pages redone like the Figma (all Website Admin pages and the Sign In window; the office pages use `ROOMY = True` in their specs: `gen_designer.roomy` spreads them out x1.15 / y1.32 with a 24/16 margin, 36px buttons and bigger section titles; hand-made designers got the same once through `roomy_designer.py`). Tabs are drawn flat by the theme (`StyleTabs`) use these with docked layouts and Segoe UI 14 section titles; the sidebar has the logo (`Resources/logo-dark.png`, embedded) and Segoe MDL2 icons.

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

Tables: `users`, `login_logs`, `password_requests`, `audit_logs` (Database Guide 4.1).

## 4.1 Sign In (`Forms/LoginForm`, `Services/AuthService`)

`AuthService.SignInAsync(username, password)` returns a `SignInResult`:

1. Find the user by `lower(username)`. Unknown name → "Failed Login" (details "Unknown username", `username_tried` kept) and the message "Wrong username or password."
2. `status = Disabled` or `Locked` → "Failed Login" logged with the reason and a message to contact an Admin.
3. `BCrypt.Verify(password, password_hash)`. Wrong → `failed_login_count + 1`, logged as "Wrong password (n of 5)", message "Wrong username or password. n attempts left..."; at 5 → `status = Locked` and "Account Locked" logged.
4. Right → `failed_login_count = 0`, `last_login_at = now`, "Login" logged with the PC name, `Session.Start(user)`.

`AuthService.SignOutAsync(idle)` logs "Logout" or "Idle Sign Out". The idle check (`IdleMonitor`, an `IMessageFilter` counting mouse and keyboard messages) opens `IdleWarningForm` after 5 minutes; no answer in 60 s signs out.

## 4.2 Forgot Password (`Forms/ForgotPasswordForm`, `Services/PasswordService`)

- Employees and Supervisors: `RequestResetAsync` adds a Waiting `password_requests` row (one open request per person) and logs "Password Request". Admins see "Password requests (N)" (`CountWaitingRequestsAsync`) and handle them in `NotificationsForm` → `SetNewPasswordForm` → `SetNewPasswordAsync` (hash, unlock, request Done, "Password Changed" logged) or `DismissRequestAsync`.
- Admins: `ResetWithSecurityAnswerAsync` compares the bcrypt hash of the trimmed, lower-cased answer; 10 wrong answers in total block the security-question route.
- Password rules (`Company.Shared/AccountRules.CheckNewPassword`): 8+ characters with a letter and a number. `GeneratePassword` makes one that passes.

## 4.3 My Account (`Pages/MyAccountPage`, `Services/AccountService`)

Every login sees its details. Admins can also `ChangeOwnPasswordAsync` and `SetSecurityQuestionAsync`, both requiring the current password. Others ask an Admin (owner rule).

## 4.4 User Accounts (`Pages/UserAccountsPage`, `Services/UserAdminService`, Admin only)

- The list (`ListAsync(search, status)`) and an edit form; **Create Account** opens the add-only copy of the page (chapter 3.5).
- A login is created from an employee (`LinkableEmployeesAsync`: Active office staff without a login, never Site). Name comes from the employee; department from `DepartmentFor(level, employee.department)` (none for Admin / Website Admin; Administration staff can only be Admin or Website Admin).
- Username: `^[A-Za-z0-9._-]{3,50}$`, unique ignoring case. A second Supervisor or Employee in the same department asks to confirm (`OthersInSameRoleAsync`), it doesn't block.
- `UnlockAsync`, `DisableAsync`, `EnableAsync`. You can't disable or demote yourself; the last active Admin can't be disabled or demoted.
- Audited with department NULL (Admin-only history).

# Chapter 5: Projects and Dashboard (Part 1)

## 5.1 Projects (`Pages/ProjectsPage`, `Services/ProjectService`, Receipts Supervisor edits, Admins view)

- Grid: `ListAsync(search, status, sector)` with name, client, sector, status, contract price, revenue and expenses from `ProjectFinance.ForProjectsAsync` (one grouped query per source for all listed projects).
- Form fields map 1:1 to `projects`. `CheckInput`: name and site required, price ≥ 0, target end not before start. When status becomes Done, `completed_date = today`; any other status clears it.
- The summary panel (`SummarizeAsync`): days text (`DaysText`: "Day 12", "Starts in 3 days", "Finished in 140 days", "Cancelled"), revenue, expenses, profit.
- **Delete** only when `CountLinkedRecordsAsync` (material usages + sub jobs + receipts + change orders) is 0; otherwise the message says to set it to Cancelled.
- **Export** (ClosedXML) and **Print Report** (`ProjectReport`, a `PrintDocument`).

## 5.2 Dashboard (`Pages/DashboardPage`, `Services/DashboardService`)

- Period: `DashboardService.Range` turns This Month / Quarter / Year / All Time / Custom into dates.
- `KpisAsync(from, to, department)`: completed projects in the period, their revenue and expenses (`ProjectFinance`), plus overhead receipts (`ReceiptService.OverheadTotalAsync`) and repairs (`EquipmentService.MaintenanceCostAsync`) by date; payroll is not added (payroll is overhead and isn't split per project). A Supervisor's department is forced inside the service (`LockedDepartment`), not only by the disabled dropdown.
- `StatusCountsAsync` → ScottPlot bar chart (`FormsPlot.Plot.Add.Bars`, manual tick labels).
- `TopProjectsAsync`: projects active in the period, profit to date, sortable by clicking a header (sorted in memory, `SortGlyphDirection` shows the order).
- `LogHistoryAsync` (Admin only): `login_logs` joined to `users`, filtered by action group and name/username, newest 2,000.

# Chapter 6: HR & Payroll (Part 2)

`Pages/HrPayrollPage` hosts seven tab controls from `Pages/Hr`. Settings and Edit History are removed for non-approvers (`!IsAdmin && Level != Supervisor`). Audit department: `HR & Payroll`.

## 6.1 Employees (`EmployeesTab`, `EmployeeEntryForm`, `Services/EmployeeService`)

- `EmployeesTab.Designer.cs` contains the owner's Visual Studio edits: edit it in the designer only, never regenerate it.
- Add: `EmployeeEntryForm` → `CreateAsync` inserts the employee and the first `employee_rates` row together. Edit: `UpdateAsync`; a new daily rate with a date adds a rate row instead of overwriting (history). `pay_schedule` is derived from the department (the CHECK rule enforces the pairing).
- `MarkInactiveAsync` also disables every linked login; `ReactivateAsync` doesn't re-enable them.
- Government ID numbers are kept but audited only as `*_on_file`. Changes to name or department are copied to the linked login.

## 6.2 Attendance (`AttendanceTab`, `FixScanForm`, `Services/ScannerFile`, `AttendanceRules`, `AttendanceService`, `ScannerEmployeeFile`)

- **Upload Scanner File** → `AttendanceService.ImportAsync(path)`:
  - `ScannerFile.Read` opens the file with ExcelDataReader (`CodePagesEncodingProvider` registered for old .xls), finds the sheet named or titled "Attendance Logs", reads the range "dd/MM/yyyy ~ dd/MM/yyyy" (if day/month is ambiguous, the first printed weekday decides), then one block per "ID" row: day-number row → column-to-date map, weekday row, then every `hh:mm` in the cells below. Excel numbers (fractions of a day) and DateTimes are accepted too.
  - Scanner ID → employee by `biometric_id` with leading zeros stripped; unknown IDs with scans become "rejected rows" (stored in `attendance_imports.problems`, shown by **View Rejected Rows**).
  - New scans inserted; existing (employee, minute) skipped. Then `RecomputeAsync(from, to)`.
- **Recompute** loads `HrSettingsService.LoadAsync()` into `AttendanceRules.Current`, the holidays in range and the finalized payroll periods, then for every Active employee with a Biometric ID and every date: skip fixed rows and locked days; scans → `AttendanceRules.FromScans`; work day without scans (before today, not a holiday, from the first rate date) → Absent; else delete the non-fixed row.
- **Rules** (`AttendanceRules`): merge scans < 3 min apart; 4+ scans → first = in, last = out, lunch = nearest to break start / break end; fewer → every in-order placement into the 4 slots is tried and the one closest to the usual times wins; rest days with 2-3 scans → first/last. Late = (in − start if in > start + grace) + (break in − break end if later); early = end − out if earlier; overtime = out − end when ≥ the minimum. Status: Missing Scan (< 4 times), Late, Present, Rest Day Work.
- **Grid**: `ListAsync(from, to, filter, search)`; Absent / Missing Scan rows MistyRose, overtime Waiting LightYellow (`CellFormatting`); nothing auto-selected (`RecordForm.NoAutoSelect`).
- **Fix Missing Scan** (`FixScanForm`): time pickers with check boxes (`ShowCheckBox`), reason required, times must be in order (`CheckFix`), Yes/No list of changes (`RecordForm.ConfirmSave`), then `FixAsync` sets the times, recomputes the day from them and stores `fix_reason/fixed_by/fixed_at`. Refused inside a finalized payroll.
- **Approve / Reject Overtime**: approver only (`Access.RequireApprover`), status must be Waiting, refused inside a finalized payroll.
- **Make Scanner Employee File** (`ScannerEmployeeFile.SaveAsync`): Active employees with a numeric Biometric ID, sorted by ID; department numbers in `DepartmentNumbers`. The embedded scanner template (`Resources/ScannerEmployeeForm.xls`) is read with ExcelDataReader and rewritten as a normal BIFF8 .xls with NPOI (the scanner's own file can't be opened by NPOI): all values and merged cells of both sheets, E6 = today, row 7 day numbers, row 8 weekdays, people from row 9 (ID, name, department, authority 0).

## 6.3 Payroll (`PayrollTab`, `Services/PayPeriods`, `PayrollService`, `PayslipPrint`)

- Pay group dropdown → `PayrollService.PeriodsAsync(group)`: `PayPeriods.Between` for 4 months back to 2 weeks ahead, plus any older run; the dropdown opens on the latest period that has ended.
- `GetAsync(period)` returns the run (if any), its rows and the warnings: unfixed Missing Scan and Waiting overtime (these set `Blocked`), "period not over yet", work days with no attendance rows at all ("upload the scanner file").
- **Compute** (`ComputeAsync`, any HR user): creates or reuses the Draft run, deletes its payslips and rebuilds them (`FillAsync`, Database Guide 4.5). Refused when the run is Final.
- **Finalize** (`FinalizeAsync`, approver only, confirmation message): refused while blocked; in one transaction recomputes, writes `cash_advance_deductions`, lowers `cash_advances.balance` (Paid Off at 0), sets the run Final with who/when.
- Government shares: `PayrollService.Government(settings, dailyRate, gross, group)`, tax `AnnualTax` (TRAIN, 2023 onward). Both are public and pure, so tests call them directly.
- **Print Payslips / Save as PDF**: `PayrollService.PayslipsAsync(ids)` → `PayslipPrint` (one page per payslip; `SaveAsPdf` prints to "Microsoft Print to PDF" with `PrintToFile`). Double-click a row for one payslip. **Export**: ClosedXML.

## 6.4 Payslip History (`PayslipHistoryTab`)

`HistoryAsync(runId, search)` lists payslips of Final runs only; the payroll filter comes from `FinalRunsAsync`. The page reloads its filter when the tab is selected, so a payroll finalized on the Payroll tab appears straight away. View (preview), Print (`PrintDialog`), Save as PDF.

## 6.5 Cash Advances (`CashAdvancesTab`, `CashAdvanceEntryForm`, `Services/CashAdvanceService`)

- **New Cash Advance** pop-up → `CreateAsync`: Active employee, amount > 0, per payday > 0 and not more than the amount, reason required; saved Waiting with `balance = amount`.
- Approve / Reject (approver only, Waiting only). Active advances are taken by payroll Finalize, oldest first.
- Selecting an advance shows `DeductionsAsync` (pay date, period, deducted, balance after).

## 6.6 Settings (`HrSettingsTab`, `Services/HrSettings`, `HrSettingsService`; Admin and HR Supervisor)

- `HrSettings` is one immutable record; `LoadAsync` reads the `hr.*` keys and falls back to `HrSettings.Default` per key; `Check()` validates (break inside the work hours, at least one work day, ranges).
- **Save Settings**: Yes/No list of changes → `SaveAsync` upserts every key, writes one audit row with the before/after maps, then `AttendanceService.RecomputeRecentAsync()` (last 120 days; finalized and fixed days untouched).
- Holidays: `AddHolidayAsync` (one per date, Regular / Special) and `DeleteHolidayAsync` also recompute attendance.

# Chapter 7: Inventory & Vendors (Part 3)

`Pages/InventoryPage` hosts seven tabs from `Pages/Inventory`; the Materials and Equipment tab titles carry their alert counts ("Materials (2 low)", "Equipment (1 due)"). Audit department: `Inventory`.

| Tab | Service methods | Specifics |
|---|---|---|
| Materials | `MaterialService.ListAsync(search, category, lowOnly)`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`, `LowStockCountAsync` | Starting stock only on create; `UpdateAsync` never touches `stock_qty`. Below minimum = MistyRose, zero = LightCoral. Delete refused when usages, quotes or deliveries exist. |
| Material Usage | `SubmitUsageAsync`, `ListUsageAsync(status, projectId)`, `ApproveUsageAsync`, `RejectUsageAsync`, `ProjectHistoryAsync` | Submit refuses more than the current stock and Cancelled projects. Approve (approver) re-checks stock, subtracts it, fixes `cost = quantity × unit_price`. |
| Deliveries | `PurchasingService.ListPurchasesAsync(undeliveredOnly)`, `CheckDelivery`, `RecordDeliveryAsync`, `ListDeliveriesAsync` | Picking a purchase fills vendor, material and price. Scan saved through `FileStore` ("receipts" folder) or paper + filed location. Stock up, `unit_price` = delivered price. |
| Equipment | `EquipmentService.ListAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`, `AddLogAsync`, `DeleteLogAsync`, `MaintenanceDueCountAsync` | Rented needs company, rate and unit. `next_maintenance <= today` = due (LightYellow). A log entry can move the next date. Delete refused once logs exist (use Retired). |
| Vendors | `CreateVendorAsync`, `UpdateVendorAsync`, `DeleteVendorAsync`, `UploadDocumentAsync`, `ListDocumentsAsync` | Bank account number audited only as on file. Papers: Company Profile, Bank Details, Certificate of Registration, Other. |
| Price Comparison | `ListQuotesAsync`, `AddQuoteAsync`, `DeleteQuoteAsync`, `ApproveQuoteAsync` | Open quotes first, cheapest first (Honeydew). Approve (approver) inserts the purchase and closes the other open quotes for that material. |
| Edit History | `AuditService.ListAsync(department, ...)` | Shared `Pages/Shared/EditHistoryTab`. |

# Chapter 8: Subcontractors (Part 4)

`Pages/SubcontractorsPage`, tabs in `Pages/Subcontractors`. Audit department: `Subcontractors`.

| Tab | Service methods | Specifics |
|---|---|---|
| Directory | `SubcontractorService.ListAsync(search, trade)`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`, `PcabDueCountAsync` | Trades saved as `subcontractor_trades` rows (replaced on save). PCAB expired = MistyRose, within 30 days = LightYellow; the tab title counts them. TIN and bank account audited as on file. Delete refused with jobs. |
| Jobs & Progress | `SubJobService.ListAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`, `UploadDocumentAsync`, `AddProgressAsync`, `ListProgressAsync` | Editable stages grid (`BindingList<StageLine>`); total must be exactly 100. `IsLockedAsync` (a stage reached or a live voucher) locks subcontractor, project, amount, retention and stages. Job status Ongoing / Finished (100%) / Paid in Full. |
| Billing & Payouts | `SubBillingService.GetAsync`, `MarkStageReachedAsync`, `PrepareVoucherAsync`, `PrepareRetentionVoucherAsync`, `ApproveVoucherAsync`, `RejectVoucherAsync`, `RecordPayoutAsync`, `PrintDataAsync` | `StageAmounts` puts rounding centavos on the last stage. Voucher numbers SV-0001 (last + 1, unique). Payout must equal `payable_amount`; bank transfer and check need a reference. `VoucherPrint` prints with the bank details. |

# Chapter 9: Receipts & change orders (Part 5)

`Pages/ReceiptsPage`, tabs in `Pages/Receipts`. Audit department: `Receipts`. The Needs Review tab title shows the waiting count.

| Tab / window | Service methods | Specifics |
|---|---|---|
| Receipt Vault | `ReceiptService.ListAsync(filter)`, `ApproveAsync(ids)`, `RejectAsync(ids)` | Multi-select approve/reject (all must be Waiting). Deliveries are unioned in as read-only "Stock purchase" rows (`DeliveryId` set). Total line shows all and "approved, without stock purchases". |
| Receipt Entry (pop-up) | `CheckInput`, `CreateAsync`, `UpdateAsync`, `AttachLateReceiptAsync` | Same form for new, edit (Waiting/Rejected only; saving returns it to Waiting) and "attach a late receipt" (only the copy fields enabled; decision kept). |
| Receipt Viewer (pop-up) | `ViewAsync` | Image scans in a zoomable `PictureBox` inside a scrolling `Panel` (loaded as a copy so the file isn't locked); PDF/Word open in their own program; one-page summary sheet (`ReceiptSheetPrint`). |
| Needs Review | `NeedsReviewAsync`, `NeedsReviewCountAsync` | `no_receipt` rows, Waiting first. |
| Change Orders | `ChangeOrderService.ListAsync`, `PreviewAsync`, `CreateAsync`, `UpdateAsync`, `ApproveAsync`, `RejectAsync`, `PrintDataAsync` | Budget preview = current price (contract + approved COs) → revised; end date ± days. Approve/Reject: Receipts Supervisor (`RequireApprover`). CO-001 numbering. `ChangeOrderPrint` form for the client's signature. |
| Reports | `ReceiptReports.BuildAsync(type, project, from, to)` | Detailed list, weekly or monthly summary of approved receipts; `TableReport` pages them and `SaveAsPdf` prints to "Microsoft Print to PDF". |

## 9.1 Quotations (scripts 023, 024)

`Pages/QuotationsPage` (own sidebar page, `AppPage.Quotations`, icon E8A5; designer from `specs/QuotationsPage.py`). `Access.CanSee`: Receipts department + Admins; `Access.Department(page)` gives "Receipts" for Projects and Quotations in refusal messages. Audit department `Receipts`, table `quotations`. Script 024 reshaped it to the company's quotation paper.

| Part | Service methods | Specifics |
|---|---|---|
| List + form | `QuotationService.ListAsync(status, search)`, `GetAsync` (quotation, scope, cost lines), `CreateAsync`, `UpdateAsync`, `DeleteAsync`, `InquiryChoicesAsync`, `DefaultsAsync` | RecordForm pattern (Add = pop-up with `addOnly`). Two editable grids (`dgvScope`, `dgvLines`, `AllowUserToAddRows`); both are replaced on save. `CheckInput` validates; only Draft/Rejected can change (saving a Rejected one sets Draft). `DefaultsAsync` = newest quotation's notes / payment / signatory, else `CompanyDefaults`. |
| Status | `SubmitAsync`, `ApproveAsync`, `RejectAsync`, `DeclineAsync`, `AcceptAsync` | Approve/Reject: `RequireApprover(Receipts)`. `AcceptAsync` calls `ProjectService.CreateAsync` (Pending, contract = total) and stores `project_id`. Buttons are enabled by status in `ShowAsync`. |
| Total | `QuotationService.Total(lines)` | Sum of the cost lines; used by the page, the list and the printout. |
| Print / PDF | `PrintDataAsync`, `QuotationPrint` (`PrintDocument`, `SaveAsPdf`) | Built as a list of blocks with heights, drawn page by page (a block never splits; the signature block is one block). Header image `Resources/quote-header.jpg` (`Theme.QuoteHeader`, embedded). DRAFT watermark until approved. |

# Chapter 10: The Website Admin app (Part 6)

## 10.1 One program, two sets of screens

`src/Company.Launcher` (Tri-Stone, `Apps\Tri-Stone\TriStone.exe`) is the only executable. It shows the office Sign In, then opens a main window by level: Website Admin → `Company.WebsiteAdmin/Forms/MainForm`; Admin → `AppChoiceForm` (Office App / Website Admin / Sign Out); others → the office `MainForm`. After Sign Out it returns to Sign In. `Company.OfficeApp` and `Company.WebsiteAdmin` are class libraries with no `Program.cs`; in Visual Studio, **Company.Launcher** must be the startup project.

The Website Admin main window copies the office layout: sidebar (Home, Projects, Services, About Us, Contact Us, Log out; from the Figma design, owner decisions 2026-11-01), the page title, who is signed in, **Publish Website** with the last publish, and the same idle check. Every editor implements `IWebsiteEditor.HasUnsavedChanges`; `MainForm` asks before leaving a page with unsaved edits, before Log out, and warns on Publish that unsaved edits are not included.

## 10.2 Logins

Website Admin is a level in `users` (script 011), created in User Accounts like any other login, without a department. `User.CanOpenWebsiteAdmin` (Website Admin or Admin) is re-checked by every Website Admin service.

## 10.3 Saving and publishing (`Services/WebsiteContentService`, `PublishService`, `WebsiteAccess`, `WebsiteImages`)

- **Save Changes** writes the database only (not live). **Publish Website** (`PublishService.PublishAsync`, owner decision 2026-11-03) pushes the content to the website's GitHub repository, and Netlify updates the live site in about a minute (no server needed):
  - `PublishService.BuildAsync` makes `site.json`: `home` (headline, subheadline, image), `contact` (address, phone, email, copyright; never the recipient email), visible `services`, visible `projects` with their showcase photos, `about` sections in order. Pictures are copied from the files folder with web-safe names and their addresses become `/Updated/content/images/<name>`. A missing picture stops the publish with its path.
  - `GitHubPublisher.PushAsync` makes `Updated/content` in the repository hold exactly those files as **one commit** through GitHub's REST API (blobs, tree, commit, move the branch): unchanged files are skipped (git blob hash), unused pictures are removed, and nothing changed = no commit. Errors come back as plain messages (bad or expired token, no access, branch not found, changed at the same moment).
  - On success `site.json` is also stored in `website_publishes` (Last published).
  - `PublishSettings` (per PC, `C:\ProgramData\TriStone\OfficeApp\publishsettings.json`): repository (default `Third09/tristone_website`), branch (default `Frontend`) and a GitHub token encrypted with DPAPI. Set in **Website Settings** (`Forms/PublishSettingsForm`, top bar; also opens on the first publish); **Test** calls `GitHubPublisher.TestAsync` (repository reachable, token may push, branch exists).
  - **Waiting count / Undo** (owner request 2026-11-03): `website_publishes.content` = site.json plus `_sources` (repo path -> stored picture path) and, for an undo, `_restored_from` (publish_id). `PublishService.WaitingCountAsync` counts Website audit rows of the website tables since the live version (after an undo: since the restored one); `MainForm.tmrWebsiteCheck` (15 s) shows it on the Publish button. `UndoAsync` republishes the second-newest row (pictures read back through `_sources`, `_` keys removed from site.json).
  - **Other extras**: `MainForm_KeyDown` (KeyPreview) = Ctrl+S on the nearest `btnSave*`; `WebsiteImages.Shrink` (1600 px / 1 MB, temp copy) in `StoreAsync`; every 12th `tmrWebsiteCheck` tick runs `InquiryImport.PullAsync` and shows `ntfInquiries` (NotifyIcon balloon, click = Contact Us); `WebServiceService.DuplicateAsync` (hidden copy after the original, renumbers sort_order).
  - **Preview Website** (`Forms/PreviewForm`, package `Microsoft.Web.WebView2`): `GitHubPublisher.CopySiteAsync` mirrors the repository's `Updated` folder (except `Updated/content`) into `C:\ProgramData\TriStone\WebsitePreview`, downloading only blobs whose git id changed (`.files.json`); no token needed while the repository is public. `PublishService.BuildAsync` output is written into `Updated/content`, then `SetVirtualHostNameToFolderMapping("tristone.preview", folder)` + navigate to `https://tristone.preview/Updated/html/index.html` (the pages use `/Updated/...` paths, so a real host is needed, not file://). WebView2 data in `%LOCALAPPDATA%\TriStone\WebView2`.
  - The website side: `Updated/js/content.js` (loaded after `gallery.js`) reads `/Updated/content/site.json` and fills the hero, contact details and footer, the service cards, the projects gallery (`galleryData`) and the About Us rows (picture and text take turns). No `site.json` or an empty picture = the page keeps what `index.html` has.
- `WebsiteAccess.Require()` = Website Admin or Admin (`User.CanOpenWebsiteAdmin`), re-checked by every service. Edit History department: `"Website"`.
- `WebsiteImages`: JPEG/PNG, 8 MB max, stored with `FileStore` under `website\yyyy\MM`; `LoadAsync` reads into memory so files are not locked.
- `WebsiteContentService.SaveAsync(what, texts, newImages)`: validates the length limits (`MaxLength`), required items (headline, headings, recipient email) and emails; writes only changed keys; one audit row listing the changed keys (pictures by file name).

## 10.4 Home (`Pages/HomeEditorPage`) and About Us (`Pages/AboutEditorPage`)

Home is a single-item editor over `website_content`: `home.hero_image`, `home.headline` (150), `home.subheadline` (300). About Us (script 015, owner request 2026-11-03) is a list over `website_about_sections` through `WebAboutService`: section cards made in code (`flpAboutSections`, wrap to more rows), **Add Section** = the same page with `addOnly: true` in `RecordForm.AddDialog`, Move Up/Down (`Ordering.Move`), Delete (refused for the last section). Each section: picture, `heading` (100) + `body` (2000), and with **Two parts** `heading2` + `body2`. Publish puts them in the snapshot as `about` (in order). A picked picture is only stored on Save; Remove Picture clears it. Save lists the changes (`RecordForm.ConfirmSave`); Discard Changes reloads.

## 10.5 Projects (`Pages/ProjectsEditorPage`, `AddWebProjectForm`, `Services/WebProjectService`)

- Featured projects (`website_projects`) are office projects with `show_on_website`; name, site and sector are read from `projects` (one list). Add New Project = pop-up listing projects not featured yet (`ChoicesAsync`).
- Per project: short description (100), Move Up/Down (`sort_order`, renumbered 1..n by `Ordering.Move`), Unpublish/Publish (`is_visible`), Delete (removes it and its photo rows from the website only; the stored files stay in the files folder).
- Photos (`website_project_photos`): up to 10, uploaded one by one with a progress bar (`AddPhotosAsync` returns how many were added and why the others were refused, so one bad file doesn't stop the rest), order, Showcase on/off (carousel), Remove. Photos save immediately; only the description uses Save Changes.

## 10.6 Services (`Pages/ServicesEditorPage`, `Services/WebServiceService`)

`website_services`: service name typed in `cboServiceName` (DropDown, 60; `WebsiteServiceNames` are suggestions; the CHECK was dropped by script 018); `icon` (scripts 019/020; `WebsiteServiceIcons.List` = key, label, fallback picture, website address: the site's 3 own icons + 26 Tabler Icons in the website repo's `Updated/images/service-icons`) is picked in the owner-drawn `cboServiceIcon` (PNG copies embedded from `Resources/icon-*.png`, made with Edge headless, `ServiceIconImages`); `WebsiteServiceNames.Suggestions` (29 names, each with its icon) fill the name list; site.json gets `icon_url` and `image` already falling back to the icon's picture; no picture = the website picture for that icon (`content.js` `SERVICE_LOOK`, Live Preview reads it from the Preview Website copy), no category (dropped by script 021, owner decision 2026-11-03), typed location (100), short description (100), one picture, order, visible. Standard list page: Add opens the same page as a pop-up (`addOnly`), the form is locked until a row is clicked, Save lists changes.

## 10.7 Contact Us (`Pages/ContactUsPage`, `InquiryForm`, `Services/WebsiteInquiries`)

- KPIs: total inquiries, pending (New), **website visits this month** (`WebsiteInquiries.VisitsThisMonthAsync` -> `InquiryImport.VisitsThisMonthAsync`: GET `?what=visits&key=...` on the Google Script, sums this month's days; falls back to counting `website_visits`. The website's `contact.js` sends `{action: "visit"}` with `navigator.sendBeacon` once per browser session, not on localhost / tristone.preview; the script's `recordVisit` adds 1 to today's row in the "Visits" tab under a lock. Owner chose a free counter over Netlify Analytics, which costs money.)
- Website Contact form (2026-11-03): `content.js` `fillServiceChoices` fills the service list from the published services + Other (the HTML keeps the 3 originals + Other as fallback); new optional `location` box; `contact.js` sends `location`, the script saves and emails it.
- Recipient email (`inquiry.recipient_email`, Website Admins and Admins) and company details (`contact.address`, `contact.phone`, `contact.email`, `contact.copyright`), each with its own Save.
- New inquiries come from the website's Google Apps Script (owner decision 2026-11-03; code in `docs/_tools/google-script/Code.gs`): `doPost` saves each Contact form message in the "Tri-Stone Inquiries" Google Sheet, then emails it; `doGet?key=...` returns all rows as JSON; `setup()` (run once by hand) makes the Sheet and the read key. `Services/InquiryImport.PullAsync` (Contact Us load and Refresh, `MainForm` load) reads them with the address and key from `PublishSettings` and inserts the ones whose `source_ref` (script 016, unique) isn't there yet, as New; not set up = does nothing. Problems come back as text and show next to the count.
- Recipient email: the script emails `RECIPIENT` (a script property) or `TO_EMAIL`. `InquiryImport.SetRecipientAsync` posts `{action: "settings", key, recipient}` to the script (Google answers with a redirect, which `HttpClient` follows); called by Contact Us > Save Email and by Website Settings > Save when the key or address changed. Script 017 corrected the seeded company email to tristoneconstructionservices@gmail.com.
- **Reply in Gmail** (`InquiryForm.btnReplyGmail`): opens `https://mail.google.com/mail/?view=cm&fs=1&to=...&su=...&body=...` in the browser (greeting with the service and the company phone); nothing is sent by the app.
- Inquiries: search, status, From/To, Reset Filter, Export to Excel; New rows in bold. Double-click or Open Inquiry opens `InquiryForm`: details, message, Copy Email, status (New / Contacted / Closed) and notes, saved after a Yes/No list. Rows are never deleted. The sidebar's Contact Us button shows "(N new)".

# Chapter 11: Testing

Each part is tested with a throwaway console program (net10.0-windows, references `Company.OfficeApp`) that signs in test users through `Session.Start`, calls the services against the real database and opens the real pages on an STA thread (`WindowsFormsSynchronizationContext` + `Application.Run`). Test logins are `zz_*`, records `ZZTEST*`; `cleanup.sql` deletes them child tables first. Runs on the work PC go into `docs\Testing Log.md`; anything that needs the real data is listed there under "Check at home".

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
| Row re-select after saving | 11 |
| Add pop-ups, no auto-select, confirm before saving | 86 |
| Website Admin app (sign-in, accounts, inquiries, sidebar, one Sign In) | 88 |
| Office tabs opened through the sidebar: nothing auto-selected | 10 |
| Attendance (rules, scanner file, fix, overtime) | 32 |
| Make Scanner Employee File | 9 |
| HR Settings, Cash Advances, Payroll, Payslip History | 47 |
| Access levels (Admin view-only, Employee / Supervisor) | 38 |
| Website Admin editors (Home, Projects, Services, About Us, Contact Us, Publish) | 43 |
| Publish to GitHub (site.json, pictures, one commit, errors) + website fill-in | 26 + browser check |

**Not tested yet:** the "last active Admin" protection (needs a separate test database), the scanner employee file on the real scanner, and payroll on real attendance (both under "Check at home").

# Chapter 12: The office server (Phase 6, `server/`)

Built 2026-11-03; not yet run on the real laptop. Plan as decided 2026-10-27: Linux Mint laptop at 192.168.1.50, PostgreSQL + shared files + backups, office LAN only, the app on its own limited login.

| File | What it does |
|---|---|
| `install-server.sh` | Run once with sudo. PostgreSQL 18 from apt.postgresql.org (Mint -> `UBUNTU_CODENAME`), `conf.d/tristone.conf` (`listen_addresses='*'`, scram), `pg_hba.conf` lines for `tristone_app` and `postgres` from the LAN only, postgres password, `setup-database.sh`, `tristone_app` LOGIN + password, Samba share `TriStoneFiles` (`/srv/tristone/files`, user `tristone`, `hosts allow` = LAN), `app_settings.file_storage_path` = `\\ADDR\TriStoneFiles`, `ufw` (deny incoming; 22, 5432 and Samba from the LAN), backup script + `/etc/cron.d/tristone-backup` (23:30). Passwords are asked (12+ characters), never stored in files. |
| `setup-database.sh` | Linux twin of `database/setup-database.ps1` (same `schema_versions` rules; scripts fed through stdin so the postgres user needn't read the home folder). `PSQL` / `DATABASE` can be overridden (tested from Git Bash on Windows). |
| `tristone-backup.sh` | `/usr/local/bin/tristone-backup`: `pg_dump -Fc` + `files.tar.gz` into `/srv/tristone/backups/YYYY-MM-DD`, keeps `KEEP_DAYS` (14), copies to `COPY_TO` (USB) from `/etc/tristone-backup.conf`. |
| `restore-database.sh` | Replaces construction_db with a `.dump` (asks YES, saves the current one first, `pg_restore --no-owner --no-acl`), runs newer scripts and 022 again, re-points `file_storage_path`. |
| `export-database.cmd` | Home PC: `pg_dump -Fc` of construction_db next to the file, to move the real data. |
| `connect-files.cmd` | Office PC: stores the share password in Windows Credential Manager (`cmdkey`) so `FileStore` can reach `\\SERVER\TriStoneFiles`. |

Script **022_app_role.sql** makes `tristone_app` (NOLOGIN until the server turns it on): CONNECT, USAGE on `public`, SELECT/INSERT/UPDATE/DELETE on all tables, USAGE/SELECT/UPDATE on sequences, default privileges for tables later scripts create (as postgres), and no writing to `schema_versions`. The app needs nothing more (it never changes table structure). Each PC keeps its own `dbsettings.json` (DPAPI) and Website Admin `publishsettings.json`.

**Windows server instead (`server/windows`, owner request 2026-11-03):** `install-server.cmd` (asks for admin rights) runs `install-server.ps1`: PostgreSQL 18 through winget (`PostgreSQL.PostgreSQL.18`, unattended, `--superpassword`) unless installed; appends `listen_addresses='*'` + scram to `postgresql.conf` and the two LAN lines to `pg_hba.conf` (network guessed from the adapter with a gateway); firewall rule "Tri-Stone database (PostgreSQL)" (TCP 5432, `LocalSubnet`); `database\setup-database.ps1`; `tristone_app` login; local user `tristone` + SMB share `TriStoneFiles` on `C:\TriStoneFiles` (network set to Private, File and Printer Sharing on); `file_storage_path`; `tristone-backup.ps1` copied to `C:\ProgramData\TriStone\Server` with a pgpass file only SYSTEM/Administrators can read; scheduled task "Tri-Stone nightly backup" 23:30 as SYSTEM (`C:\TriStoneBackups\YYYY-MM-DD\construction_db.dump` + `files.zip`, `backup.json`: `CopyTo`, `KeepDays`); sleep turned off on AC. `restore-database.cmd` / `.ps1` = the Windows twin of `restore-database.sh` (`TRISTONE_*` environment variables let tests run it unattended on a scratch database).

# Coming next

- The API and the website Contact form
- Phase 6: run the server kit on the real server PC

**Two editions (`installer/`, owner decision 2026-11-04):** the same code; `edition.json` next to `TriStone.exe` decides (`Data/AppEdition`; no file = development, as before). `build.ps1 -Edition Office|Demo` (started by `build-office.cmd [version] [address]` and `build-demo.cmd [version]`) runs a self-contained `dotnet publish` into `installer/publish` and compiles with Inno Setup 6 into `installer/output`.

- **Office** (`TriStone.iss`, Program Files, admin): `edition.json` fixes host, port, database and `tristone_app`; `DbSettings.Load` forces them over the saved file, and `Exists` = a password is saved. `DatabaseSettingsForm` allows only the password and never shows the saved one; Sign In hides the Database settings link. The password is typed once per PC and stored with DPAPI, never in the Setup file. `{commonappdata}\TriStone` gets users-modify so all Windows users on the PC share it.
- **Demo** (`TriStoneDemo.iss`, per-user, no admin; own AppId, can sit next to Office): carries `pgsql/` (postgres, initdb, pg_ctl, psql, pg_restore, their DLLs, `lib/*.dll`, `share` minus locale, the VC++ runtime DLLs) and `demo/demo.dump` (`build.ps1`: all numbered scripts + `installer/demo/demo_seed.sql` on scratch database `zz_demo_build`, dumped, dropped; dates relative to the build day). `Company.Launcher/DemoDatabase`: first start `initdb` (UTF8, locale C, trust) in `%LOCALAPPDATA%\TriStone Demo\pgdata`, then `pg_ctl` on 127.0.0.1:54329, `pg_restore`, `file_storage_path` -> `...\files`, `ready.txt`; `Program.Main` holds a single-instance mutex and stops the database in `finally`; `--reset-demo` (Start menu) deletes the folder. `DbSettings` is in-memory and never saved. Publish and Undo refuse, `PublishSettings.Load` ignores the PC's saved token and key and `Save` does nothing, Publish / Undo / Website Settings buttons are hidden. Preview Website stays (it needs internet the first time). Uninstall deletes the demo's data.
