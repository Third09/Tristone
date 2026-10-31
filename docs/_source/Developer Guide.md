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

## 5.1 Projects (`Pages/ProjectsPage`, `Services/ProjectService`, Admin only)

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
| Change Orders | `ChangeOrderService.ListAsync`, `PreviewAsync`, `CreateAsync`, `UpdateAsync`, `ApproveAsync`, `RejectAsync`, `PrintDataAsync` | Budget preview = current price (contract + approved COs) → revised; end date ± days. Approve/Reject Admin only. CO-001 numbering. `ChangeOrderPrint` form for the client's signature. |
| Reports | `ReceiptReports.BuildAsync(type, project, from, to)` | Detailed list, weekly or monthly summary of approved receipts; `TableReport` pages them and `SaveAsPdf` prints to "Microsoft Print to PDF". |

# Chapter 10: The Website Admin app (Part 6)

## 10.1 One program, two sets of screens

`src/Company.Launcher` (Tri-Stone, `Apps\Tri-Stone\TriStone.exe`) is the only executable. It shows the office Sign In, then opens a main window by level: Website Admin → `Company.WebsiteAdmin/Forms/MainForm`; Admin → `AppChoiceForm` (Office App / Website Admin / Sign Out); others → the office `MainForm`. After Sign Out it returns to Sign In. `Company.OfficeApp` and `Company.WebsiteAdmin` are class libraries with no `Program.cs`; in Visual Studio, **Company.Launcher** must be the startup project.

The Website Admin main window copies the office layout: sidebar (Page Content, Announcements, Website Projects, Document Vault, Inquiries, Website Traffic, Sign Out), title bar text, the same idle check. Pages not built yet show a placeholder.

## 10.2 Logins

Website Admin is a level in `users` (script 011), created in User Accounts like any other login, without a department. `User.CanOpenWebsiteAdmin` (Website Admin or Admin) is re-checked by every Website Admin service.

## 10.3 Inquiries (`Pages/InquiriesTab`, `Services/WebsiteInquiries`)

`ListAsync(status, search, from, to)` (newest first, New rows in bold), `CountsAsync` for "Inquiries (N new)" on the sidebar, `UpdateAsync(id, status, notes)` with the Yes/No list of changes. Copy Email puts the address on the clipboard for replying from Gmail. Rows are never deleted.

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

**Not tested yet:** the "last active Admin" protection (needs a separate test database), the scanner employee file on the real scanner, and payroll on real attendance (both under "Check at home").

# Coming next

- Website Admin app: Page Content, Announcements, Website Projects, Document Vault, Website Traffic
- The API and the website Contact form
- Phase 6: the server and the installers
