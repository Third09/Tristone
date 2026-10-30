# Tri-Stone Construction Services: Office App + Website API

Read this first. It carries everything decided so far, so a new session (or another PC) can continue without re-asking.

## How the owner wants to work

- **Short replies, one step at a time.** The owner (Shem) gets overwhelmed by long answers and big lists. Give one small step, then wait. Mention future concerns only when that phase comes.
- **Step-by-step guidance:** say exactly which program to open and where to click.
- **No styling yet.** Screens are plain, default WinForms, left-aligned. The owner styles them later from a Figma design. Don't add colors, themes, logos or custom fonts.
- **Keep all three guides updated as features are built** (see "Docs" below): Developer Guide (short: setup, structure and the main rules of each part; no full source files, small snippets only. The old 187-page full-code version is in docs/_archive), User Manual (how to use each screen, short steps), Feature Guide (for staff: how and why each feature works, rules, examples, messages; added 2026-10-30), Database Guide (owner's study guide: DB basics, every table and link, money calculation, DFD stores -> real tables, practice SELECT queries; update it with each new DB script), UI Elements Guide (buttons/controls per screen).
- Localhost only until the very end. Linux server, network and installers are Phase 6.
- Ask before decisions that are the owner's to make. When Claude picks a sensible default, say so plainly so the owner can object.

## What it is

- **Office app:** one WinForms app (.NET 10), `src/Company.OfficeApp`. It connects **directly** to PostgreSQL, with no API in between.
- **Company.Api:** ASP.NET Core Web API, used **only** by the public website (projects, inquiries, page content). Still the template; not built yet.
- **Company.Shared:** EF Core models + `AppDbContext`, used by both.
- **Website:** separate repo (`C:\Users\shemm\tristone_website`, GitHub Third09/tristone_website), plain HTML/CSS/JS following a Figma design. Only the Contact page is ours to change: add the Project Location field and POST to the API.
- **Final deployment (Phase 6):** PostgreSQL + API on a Linux Mint laptop (192.168.1.50); 5 Windows office PCs run the app over the LAN. Plan: self-contained .exe + Inno Setup Setup.exe; first run asks for the server address (Database Settings screen); `install-server.sh` for the server; a shared folder for scans/photos.
- **Hosting plan (owner-approved 2026-10-27):** the public website goes on **Netlify** (free, commercial use allowed, private repo OK, auto-deploys from the `tristone_website` GitHub repo). Not Vercel (free plan is non-commercial), not self-hosted on the office laptop. The laptop is a dedicated server (PostgreSQL + shared files folder + API + nightly backups), never used as anyone's PC; the WinForms app runs only on the Windows PCs. The website reaches the API through **Cloudflare Tunnel** (outbound only, no router ports opened, works behind CGNAT); only the API is exposed, never the database. nginx optional, in front of the API only. Network: 8-port unmanaged gigabit switch, everything wired, server on a DHCP reservation at 192.168.1.50, ideally on a UPS.
- **Phase 6 security (owner asked 2026-10-27; direct-to-DB kept, no pfSense):** port 5432 open only to the office LAN (`ufw` + `pg_hba.conf`), never the internet; the app uses its own limited DB role, not `postgres`; automatic daily backups copied off the server; strong passwords for the DB and every Admin.

## Folder layout

```
TriStone.slnx                  open in Visual Studio
src/Company.Api|OfficeApp|Shared|WebsiteAdmin|Launcher
                               Launcher = Tri-Stone (TriStone.exe): the one Sign In; opens the office app or the
                               Website Admin app. WebsiteAdmin = a library (its screens) that references OfficeApp.
database/NNN_name.sql          numbered, idempotent (IF NOT EXISTS) scripts
database/setup-database.ps1    creates construction_db if missing, runs unapplied scripts (tracked in schema_versions)
database/setup-database.cmd    double-click version
docs/                          Project Outline.docx (the living spec), UI Elements Guide.docx,
                               User Manual.docx, Feature Guide.docx, Developer Guide.docx, Database Guide.docx,
                               Testing Log.md, diagrams/*.drawio (DFD + flowchart)
                               (outline changes: docs/_tools/outline/partN.ps1, latest part_oct30.ps1)
docs/_source/*.md              text the Word guides are built from; <!-- include: path --> pulls real code
docs/_tools/                   guides/ (build-guides.cmd), ui-guide/ (data.js, data2.js, build.js),
                               outline/ (outline_insert.ps1 + partN.ps1 + rebuild_outline.ps1),
                               designer-gen/ (gen_designer.py + specs/), render.ps1 (docx -> PNG check)
```

## Build, database, test

- Build: `dotnet build TriStone.slnx`. Run: set **Company.Launcher** as startup project, F5. The **only** program is `Apps\Tri-Stone\TriStone.exe` (OutputPath in Company.Launcher.csproj, owner request 2026-10-30; `Apps/` is git-ignored). Company.OfficeApp and Company.WebsiteAdmin are libraries (no Program.cs), so nobody can open one app alone and get the wrong Sign In.
- Database: PostgreSQL 18, database `construction_db`. Create/update: `database\setup-database.cmd`.
- psql without a password prompt: put `localhost:5432:*:postgres:<password>` in `%APPDATA%\postgresql\pgpass.conf`, then `"C:\Program Files\PostgreSQL\18\bin\psql.exe" -w -h localhost -U postgres -d construction_db`. Never write the real password in files or Git.
- First Admin is inserted by hand (Developer Guide chapter 6). On this project it's `ITmorilla` (Shem John Adrian P. Morilla). Not linked to an employee yet. Password is weak and must be changed before real use.
- **One account list for both apps (owner decision 2026-10-30, script 011):** Website Admin is a Level in `users` (User Accounts), no department, linked to an office employee like other logins; office Admins create and manage them. The separate website accounts (scripts 008/009: `website_accounts`, `is_main`, the app's own sign-in and Accounts page) were merged into `users` and removed. Password/username rules live in `Company.Shared/AccountRules`. `UserLevels.HasNoDepartment(level)` = Admin or Website Admin; `User.CanOpenWebsiteAdmin` = Website Admin or Admin.
- The app's own DB settings are per PC: `C:\ProgramData\TriStone\OfficeApp\dbsettings.json`, with the password encrypted by DPAPI.
- **Testing approach used so far:** a throwaway console app (net10.0-windows, references Company.OfficeApp) that runs real checks against the real DB, run on an STA thread with `WindowsFormsSynchronizationContext` + `Application.Run()`, with forms shown via `ShowDialog`. Test data is named `zz_...` (users) / `ZZTEST...` (records) and deleted afterwards. Cleanup: a `cleanup.sql` deletes everything named zz_/ZZTEST in foreign-key order (child tables first, users last), plus any test files in the storage folder. Test windows appear on the real screen.
- **Two PCs.** Home (main) PC has the real database. Work PC has its own fresh database (test data only). Every test run on the work PC goes into `docs/Testing Log.md` (date, PC, what, result), and anything that still needs the real data goes under "Check at home". Tick items off there after checking at home.
- Commit messages: write them with a bash heredoc (`git commit -F -`). PowerShell here-strings with quotes break.

## Code conventions

- Pages are `UserControl`s loaded into `MainForm.pnlContent` by `MainForm.ShowPage` (switch on `AppPage`). Big pages with tabs keep each tab in its own UserControl (e.g. `Pages/Hr/EmployeesTab`).
- Rules live in `Services/*Service.cs` (static, async, return `string? error`); pages call them. Services re-check permissions (`Access.CanSee` / Admin), not just the UI.
- Control names follow the UI Elements Guide (btn/txt/lbl/cbo/dgv/num/dtp/chk...).
- Designer files: plain designer code. Big ones can be generated once with `docs/_tools/designer-gen`. **Never regenerate after the owner edits a form in the VS designer.**
- **Hidden grids:** a grid loaded while its tab is hidden selects row 0 on first show; every list page calls `RecordForm.NoAutoSelect(grid, on => filling = on, RestoreSelection)` in its constructor (found on the Website Admin Inquiries tab 2026-10-30; office tabs fixed too).
- **Grids never auto-select** (owner-reported bug 2026-10-30: HR "Save" overwrote the first employee). After setting `DataSource`: `filling = true`, `ClearSelection()`, re-select only the intended id, `filling = false`; `SelectionChanged` returns while `filling`. **Add = pop-up form; page form = edit only** (locked when nothing is selected); **Save on edit asks Yes/No listing the changes**. Done on every list page through `UI/RecordForm` (Projects, User Accounts, Materials, Vendors, Equipment, Sub Directory, Jobs, Change Orders): the Add pop-up is the same page made with `addOnly: true` (`ShowFormOnly` + `AddDialog` + `Added` event; its list load returns early), `SetEnabled` locks the form, `ConfirmSave` lists changes (pass private field names). Employees uses its own `EmployeeEntryForm`. New list pages (Attendance, Website Admin...) must follow the same pattern.
- Main window: borderless, maximized to the working area, no ControlBox (owner request); leave via Sign Out.
- Grids: read the selection from `SelectedRows`, not `CurrentRow` (bug fixed in ch. 15). After a reload, re-select a row with `Responsive.SelectRow(grid, (XRow r) => r.Id == id)` (uses the first visible cell, so hidden columns can't crash it).
- Responsive (ch. 16), every list page: `Responsive.FitGrid(this, grid)` in the constructor; search boxes via `Responsive.OnTypingPause` (not `TextChanged` in the designer); loads use `LatestOnly` tickets + `Responsive.Busy(this)`. App is PerMonitorV2 DPI-aware and MainForm opens maximized.
- Every add/edit/delete writes `audit_logs` through `AuditService.Add` (department = which Edit History tab shows it). Never store passwords, hashes, security answers, government ID numbers or bank account numbers in it. `Pages/Shared/EditHistoryTab` shows it per department.
- Project revenue/expenses only through `Services/ProjectFinance` (Projects page, report and Dashboard all use it); add each new money source there and to `ProjectService.CountLinkedRecordsAsync`. Payroll and equipment repairs are overhead, not project expenses.
- Approve buttons/actions: `Access.IsApprover(user, dept)` = Admins + that department's Supervisor; services re-check with `RequireApprover`.
- Uploaded files: `FileStore` (folder from `app_settings.file_storage_path`, DB keeps relative paths; pdf/images/Word, 25 MB max).
- designer-gen: `FORM = True` in a spec makes a pop-up window instead of a UserControl; `"PARENT": "pnlX"` puts a control inside a Panel.
- Dashboard money uses the completed-contract method (a project counts when Done); overhead counts by date. Keep `DashboardService` and `ProjectFinance` the only places that add money up.
- Dates: `DATE` -> `DateOnly`; timestamps are plain `timestamp` (AppDbContext maps DateTime to "timestamp without time zone", which is required). Money: `NUMERIC`, shown with `ProjectService.Money` (en-PH, ₱).
- Excel export: `ExcelExport` (ClosedXML). Printing: `PrintDocument` + `PrintPreviewDialog` ("Microsoft Print to PDF" for PDFs).
- New DB change = new numbered script. Never edit an applied script.

## Decisions (owner-approved)

**Roles and login (Part 0)**
- One login for the whole app (and, since 2026-10-30, for the Website Admin app too). Each account has a Level (Admin / Supervisor / Employee / Website Admin) and, except Admins and Website Admins, ONE Department (HR & Payroll, Inventory, Subcontractors, Receipts). Plan: 1 Employee + 1 Supervisor per department, several Admins (a second one asks, it doesn't block).
- Sidebar by role (`Services/Access.cs`): Admin = everything; Supervisor = Dashboard (own department's numbers) + own department + My Account; Employee = own department + My Account. Projects, User Accounts = Admin only. Website Admin level = no office pages (Tri-Stone opens the Website Admin app for them). Tri-Stone routing: Website Admin -> Website Admin app; Admin -> asks (Office App / Website Admin / Sign Out, `AppChoiceForm`); everyone else -> office app.
- Only Admins create accounts and change passwords. Employees/Supervisors can't change their own password. Forgot Password = request to Admins (badge, "Password requests (N)" for now); Admin sets a new password and gives it in person (no temporary password, no forced change). Admins recover with a security question (set in My Account).
- Lock after 5 wrong passwords (Admin unlocks). Statuses: Active / Locked / Disabled. "Still there?" popup after 5 minutes idle, auto sign-out after 60 s (`TRISTONE_IDLE_SECONDS` shortens it in Debug).
- Each login belongs to one Active office employee (never site workers); name and department come from the employee. Administration staff only get Admin logins. Old accounts may stay unlinked.
- Claude-chosen defaults the owner accepted: passwords 8+ chars with a letter and a number; usernames `^[A-Za-z0-9._-]{3,50}$`, unique ignoring case; no self-disable/self-demote; last active Admin protected (**not yet tested**: needs a separate test DB, never disable ITmorilla to test).

**Projects (Part 1b)** Admin only; the one shared project list. Fields: name, site, client, sector (Residential / Commercial / Industrial / Repairs / Renovations), contract price, start, optional target end, status (Pending / Ongoing / Done / Cancelled), show on website. Completion date auto-set when Done. Revenue = contract price + approved change orders. Expenses = receipts + materials + subcontractor payouts (payroll and equipment repairs are company-wide overhead). Delete only while nothing links to it (`CountLinkedRecordsAsync`: extend it as Parts 3-5 are built; `Summarize` expenses are 0 until then).

**Dashboard (Part 1)** KPI cards (completed projects, gross revenue, operational expenses, net revenue), status bar chart, Top Performing table (revenue, expenses, profit, margin %, sortable, highest profit first), Log History (Admin), date filters (month / quarter / year / all / custom). Supervisors see their department only. Charts: ScottPlot (not in the default Toolbox).

**HR & Payroll (Part 2)** Page with 7 tabs (Settings and Edit History are Admin + HR Supervisor only). Built so far: **Employees** (office staff + site workers; departments Administration / HR & Payroll / Inventory / Subcontractors / Receipts / Site; the pay schedule follows the department: Site = weekly on Saturday, others = 15th and 30th; dated daily-rate history; unique Biometric ID; Inactive turns logins off, Reactivate doesn't turn them back on; government deductions switch, off by default).
**Attendance built 2026-10-30** (script 012, `ScannerFile` + `AttendanceRules` + `AttendanceService`, `Pages/Hr/AttendanceTab` + `FixScanForm`): upload the scanner's "All Report.xls" (sheet "Attendance Logs", read with ExcelDataReader); scanner ID = Biometric ID; each scan stored once; days worked out (Present / Late / Missing Scan / Absent / Rest Day Work); HR fixes with a reason; HR Supervisor/Admin approve overtime. Rules Claude proposed and the owner approved 2026-10-30: lunch return after 1:00 counts as late, leaving before 5:00 = early minutes, OT from 30 min, repeat scans within 3 min count once, Sunday scans = Rest Day Work; rules are constants in `AttendanceRules` until the Settings tab (holidays not handled yet: a holiday shows Absent).
Still to build: Payroll (office: 15th covers 26th-10th, 30th covers 11th-25th; Feb -> last day; Sunday/holiday -> day before; site: Friday upload, Saturday counts next week; only scanned/validated days paid), rules: 8-5 Mon-Sat, 12-1 unpaid lunch, on time until 8:15, late = exact minutes (daily rate / 8 / 60), absent = no pay, OT after 5 PM only if HR approves at ₱100/hr, missing scans flagged for HR; cash advances (HR Employee records, HR Supervisor approves, fixed deduction per payday, carries over, never negative); payslips printed + PDF. Workers are NOT assigned to projects.

**Inventory (Part 3)** Materials (name, category, unit, price, stock, minimum; low-stock alert); usage to a project = Employee records, Supervisor approves (stock down, cost to project); deliveries = no approval, stock up, receipt attached (scan or "physical copy only" + receipt no. + filed location), stored in Receipts as "Stock purchase" so it isn't double counted; equipment list (owned/rented) + maintenance log + next-maintenance reminder (no location tracking); vendors + papers; price comparison (Employee enters quotes, Supervisor picks the approved purchase).

**Subcontractors (Part 4)** Directory (outline fields) + PCAB expiry reminder; no ratings for now; job = sub + project + scope + amount + dates + docs; progress %; payment by fixed stages per job (must total 100%); retention default 10% per job, released right after the job is finished and checked; Employee prepares the voucher, Supervisor approves; payouts are project expenses.

**Receipts (Part 5)** Each receipt is Project or Company overhead; Employee uploads, Supervisor approves; no-receipt expenses are flagged "needs review" with a reason and approved by the Receipts Supervisor; change orders: Receipts Employee logs them, budget impact preview, Admin approves, then revenue and end date update; CO-001 numbering; printable form.

**Website Admin built so far:** sidebar layout like the office app (owner request 2026-10-30), same idle sign-out, Inquiries page (`inquiries`, script 010: New / Contacted / Closed + internal notes, Website Admins and Admins can follow up, never deleted, Copy Email for replying from Gmail, "Inquiries (N new)" on its sidebar button). Messages arrive through the API later; until then the website uses Netlify Forms to email Gmail.
**Website Admin (Part 6)** **Separate screens, opened through Tri-Stone** (owner decisions 2026-10-30): not a page of the office app; `src/Company.WebsiteAdmin` (library) in the same solution, same database, same Sign In (Tri-Stone launcher). Website Admin logins open it; Admins choose. The office app's Website Admin sidebar button and `AppPage.WebsiteAdmin` were removed. Everything on the website editable; projects come from the Projects "Show on website" tick + website-only details/photos and per-field visibility; inquiries saved in the DB (New / Contacted / Closed + notes) and a copy emailed to the company Gmail (not set up yet).

## Status (as of 2026-10-30)

Built and tested (Developer Guide chapters): 1-6 setup, DB, first Admin; 7-8 DB settings + sign-in; 9 Forgot Password; 10 sidebar + idle; 11 My Account; 12 User Accounts; 13 Projects (+ Excel export, print report); 14 HR Employees; 15 logins linked to employees; 16 responsive pages (fit any screen, search waits for typing, DB warm-up); 17 Inventory & Vendors (all 7 tabs); 18 Subcontractors (directory, jobs + stages + retention, progress, vouchers SV-0001, payouts, voucher print); 19 Receipts & change orders (vault incl. stock purchases read from deliveries, needs review, CO-001 Admin approval, reports, TableReport/PDF); 20 Dashboard (completed-contract KPIs, department view, ScottPlot status chart, top projects, log history).
DB scripts applied: 001 login/security, 002 projects, 003 employees, 004 one login per employee, 005 inventory (+ app_settings), 006 subcontractors, 007 receipts + change orders, 008 website accounts, 009 website main account flag (both merged away by 011), 010 inquiries (website Contact form messages), 011 merge website accounts into users (Website Admin level; tables website_accounts / website_login_logs dropped after copying accounts, passwords, sign-in history and inquiry follow-ups), 012 attendance (imports, scans, days).

2026-10-27: UI Elements Guide synced to Parts 3-5 + Dashboard; Developer Guide shortened (187 -> 9 pages).
2026-10-30 (work PC, all logged in docs/Testing Log.md):
- Safe add/edit on every list page (`RecordForm`): no auto-select (incl. hidden tabs), Add opens a pop-up, Save asks Yes/No listing changes; main window full screen with no corner buttons; Materials dropdown-clearing bug fixed.
- New guides: Feature Guide (staff), Database Guide (owner's study guide). Testing Log for the two-PC setup.
- Website Admin: its own screens (`Company.WebsiteAdmin`), sidebar layout, Inquiries page (script 010). One account list + one Sign In: Tri-Stone launcher (`Company.Launcher`, the only program, `Apps\Tri-Stone\TriStone.exe`), Website Admin level in User Accounts (script 011). Office app and Website Admin are libraries.
- Home PC: pulled and scripts 008-012 applied 2026-10-30. Still to do there: the "Check at home" list in docs/Testing Log.md.
- Owner's Visual Studio designer edits on `Pages/Hr/EmployeesTab` were committed 2026-10-30 at the owner's request. Don't regenerate that designer file.

**Page Content (Website Admin): waiting on owner decisions (explained 2026-10-30, no code yet).** The website repo (`Updated/html/index.html` = Services, "What We Build" projects, About Us; `contact.html`) has all text and ~19 images/videos typed into the HTML. Plan: (1) Page Content screen in Website Admin + a new DB script for editable items per page/section (text or image), seeded from the current website text, same RecordForm pattern; safe to build any time. (2) Website side needs a small label on each editable text/image (e.g. `services.title`); original HTML text stays as the fallback. Owner must decide:
1. May we add those labels to pages other than Contact (or does the friend add them)? Owner may ask the friend first.
2. How changes reach the site: **A Live** (site asks the API on every visit; instant, but the laptop must be on, images served from it, brief flicker) or **B Publish button** (Claude's recommendation: Website Admin triggers a Netlify rebuild that bakes text + images into the site; works with the laptop off, fast, good for Google, ~1-2 min delay, needs a one-time Netlify build step).
3. How much is editable: everything, or **selected parts** (recommended: section titles, descriptions, service items, About Us text, main photos; menus/footer/buttons fixed).
Build order: screen + table first, then the website labels, then the API / Publish piece.

Next: Payroll (uses `attendance_days`: only Present/Late/fixed days paid) and cash advances, HR Settings (work hours, holidays), the remaining Website Admin pages (Page Content, Announcements, Website Projects, Document Vault, Website Traffic), then the API + website Contact form, then Phase 6 installers.
