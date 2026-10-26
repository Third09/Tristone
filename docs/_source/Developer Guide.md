# Tri-Stone Office App: Developer Guide

How the Tri-Stone Construction Services system is built, step by step, with the real code.
This guide grows as each part is built. Code shown here is pulled straight from the project files every time the guide is rebuilt, so it always matches the project.

> **What we are building.** One Windows desktop app (WinForms, .NET 10) for the office, connected directly to a PostgreSQL database. A small ASP.NET Core Web API sits next to the database only for the public website (projects, inquiries, page content). Everything runs on `localhost` while building; it moves to the Linux Mint server (`192.168.1.50`) at the very end.

| Part | What it is |
|---|---|
| 0 | Login & Security |
| 1 / 1b | Dashboard / Projects |
| 2 | HR & Payroll (and User Accounts) |
| 3 | Inventory & Vendors |
| 4 | Subcontractors |
| 5 | Receipts |
| 6 | Website Admin |

# Chapter 1: Tools you need

Install these once on the development PC.

| Tool | Version used | Why |
|---|---|---|
| Visual Studio | 2026, workloads "ASP.NET and web development" + ".NET desktop development" | Writing and designing the app |
| .NET SDK | 10.0 | Builds the app and the API |
| PostgreSQL + pgAdmin 4 | PostgreSQL 18 | The database (pgAdmin is the visual manager) |
| Git | 2.48 or newer | Keeps a history of every change |
| Node.js | 22 (optional) | Only for rebuilding these Word guides |
| Microsoft Word | any recent | Only for rebuilding these Word guides |

**Tip: forgotten PostgreSQL password.** Edit `C:\Program Files\PostgreSQL\18\data\pg_hba.conf` as administrator, change `scram-sha-256` to `trust` on the `local` and `host ... 127.0.0.1/32` and `::1/128` lines, restart the `postgresql-x64-18` service (Win + R, `services.msc`), set a new password in pgAdmin with `ALTER USER postgres WITH PASSWORD '...';`, then change the lines back to `scram-sha-256` and restart the service again.

# Chapter 2: The project folder

Everything lives in one folder: `Desktop\TriStone`.

```
TriStone\
├── TriStone.slnx              open this in Visual Studio
├── src\
│   ├── Company.Api\           Web API (for the public website only)
│   ├── Company.OfficeApp\     the desktop app (WinForms)
│   └── Company.Shared\        code both of them use (data classes)
├── database\
│   ├── 001_login_security.sql numbered scripts that build the database
│   ├── setup-database.ps1     runs any scripts not run yet
│   └── setup-database.cmd     double-click version of the above
├── docs\                      outline, UI guide, diagrams, these guides
│   ├── _source\               the text these Word guides are built from
│   └── _tools\                small scripts that rebuild the Word files
└── installer\                 filled in at the end (Phase 6)
```

**Why three projects?**

- `Company.OfficeApp` is what office staff use. It talks to the database directly.
- `Company.Api` is only for the public website, which must never touch the database itself.
- `Company.Shared` holds the classes both need (for example `Project`), so a change is made once.

# Chapter 3: Creating the solution

The solution was first created in Visual Studio (Create a new project, ASP.NET Core Web API, then Add, New Project, Windows Forms App), then moved into `Desktop\TriStone\src`. The shared project and the solution file were added from a terminal opened in the `TriStone` folder:

```powershell
# a class library that both apps reference
dotnet new classlib -n Company.Shared -o src\Company.Shared -f net10.0

# one solution file that lists all three projects
dotnet new sln -n TriStone --format slnx
dotnet sln TriStone.slnx add src\Company.Shared\Company.Shared.csproj src\Company.Api\Company.Api.csproj src\Company.OfficeApp\Company.OfficeApp.csproj

# both apps can use the shared code
dotnet add src\Company.Api reference src\Company.Shared
dotnet add src\Company.OfficeApp reference src\Company.Shared

# check everything compiles
dotnet build TriStone.slnx
```

The same can be done in Visual Studio: right-click the solution, **Add > New Project > Class Library**, then right-click each app project, **Add > Project Reference**, and tick `Company.Shared`.

The resulting solution file:

<!-- include: TriStone.slnx -->

**Target frameworks.** The API and the shared library use `net10.0`. The desktop app uses `net10.0-windows` because WinForms only runs on Windows.

# Chapter 4: The database

## 4.1 How the database is built

The database is never built by clicking around in pgAdmin. Instead, every change is a **numbered SQL script** in the `database` folder (`001_...`, `002_...`). This means:

- any PC (or the Linux server later) gets the exact same database by running the scripts;
- there is a written record of every table and why it exists;
- tables are added part by part as each part of the app is built.

A small table called `schema_versions` remembers which scripts have already run, so each one runs only once.

## 4.2 Letting tools log in without typing the password

`psql` (PostgreSQL's command-line tool) reads saved passwords from `%APPDATA%\postgresql\pgpass.conf`. One line, in the format `host:port:database:user:password`:

```
localhost:5432:*:postgres:YourPassword
```

Save it with Notepad, **Save as type: All Files**, so it doesn't get a `.txt` ending. Only your Windows account can read it.

## 4.3 The setup script

Double-click `database\setup-database.cmd`, or run `.\setup-database.ps1` in PowerShell. It finds `psql`, creates `construction_db` if it doesn't exist, then runs every numbered script that hasn't been applied yet, each inside one transaction (all or nothing).

<!-- include: database/setup-database.ps1 -->

The double-click launcher simply starts the script and keeps the window open:

<!-- include: database/setup-database.cmd -->

## 4.4 Script 001: Login & Security

Four tables:

| Table | Holds |
|---|---|
| `users` | One login per office staff member: username, bcrypt password hash, level (Admin / Supervisor / Employee), department, status (Active / Locked / Disabled), wrong-password counter, Admin security question |
| `login_logs` | Every sign in, sign out, idle sign out, failed attempt, lock, unlock, password change and password request (shown in Dashboard > Log History) |
| `password_requests` | Forgot Password requests from Employees and Supervisors; the waiting ones drive the Admin's notification badge |
| `audit_logs` | Who changed what, when, with old and new values (every department's Edit History tab) |

Rules the database enforces by itself (so even a bug in the app can't break them):

- `level` must be Admin, Supervisor or Employee; `status` must be Active, Locked or Disabled.
- Supervisors and Employees **must** have one department; Admins **must not** have one (`chk_users_department`).
- Passwords are stored only as bcrypt hashes. `pgcrypto` provides `crypt()` and `gen_salt('bf')` for making the first Admin; the app uses the BCrypt.Net library, which reads the same format.

<!-- include: database/001_login_security.sql -->

## 4.5 Checking the rules

The rules were tested inside a transaction that is undone at the end (`BEGIN ... ROLLBACK`), so no test data is left behind:

```sql
BEGIN;
-- works: a valid Admin
INSERT INTO users (username, password_hash, full_name, level)
VALUES ('t_admin', crypt('x', gen_salt('bf')), 'Test Admin', 'Admin');

-- fails (chk_users_department): an Employee without a department
INSERT INTO users (username, password_hash, full_name, level) VALUES ('t_emp', 'h', 'T', 'Employee');

-- password check: right password = true, wrong = false
SELECT password_hash = crypt('x', password_hash) AS right_pw,
       password_hash = crypt('y', password_hash) AS wrong_pw
FROM users WHERE username = 't_admin';
ROLLBACK;
```

(Each failing statement needs its own transaction or a `SAVEPOINT`, because PostgreSQL stops a transaction at the first error.)

# Chapter 5: Git

Git keeps a history of every change so mistakes can be undone. The project folder is a Git repository. Files that are generated automatically (build output, Visual Studio cache, downloaded packages, local passwords) are listed in `.gitignore` so they are never saved in the history.

<!-- include: .gitignore -->

Everyday use, in a terminal in the `TriStone` folder:

```powershell
git status                      # what changed
git add .                       # include all changes in the next save point
git commit -m "Add Projects page"   # save point with a short description
git log --oneline               # list of save points
```

Visual Studio has the same in the **Git Changes** window (View > Git Changes).

# Chapter 6: The first Admin account

The app has no sign-up screen (only Admins create accounts), so the very first Admin is added directly in the database, once. Open pgAdmin, click `construction_db`, then **Tools > Query Tool**, and run (with your own username, name and password):

```sql
INSERT INTO users (username, password_hash, full_name, level)
VALUES ('YourUsername', crypt('YourPassword', gen_salt('bf', 11)), 'Your Full Name', 'Admin');
```

- `crypt(..., gen_salt('bf', 11))` stores a bcrypt hash (cost 11), never the password itself. The hash starts with `$2a$11$`.
- `department` is left empty because Admins don't belong to one.
- The real password is **never** written in a script file or saved in Git.

Check it worked (the last column must be `t` for true):

```sql
SELECT username, level, status, password_hash = crypt('YourPassword', password_hash) AS password_works
FROM users;
```

After this, every other account is created by an Admin inside the app (User Accounts page).

# Chapter 7: Connecting the desktop app to the database

## 7.1 Packages

Added with `dotnet add package` (or in Visual Studio: right-click the project, **Manage NuGet Packages**):

| Project | Package | Why |
|---|---|---|
| Company.Shared | `Npgsql.EntityFrameworkCore.PostgreSQL` 10.0.3 | Entity Framework Core for PostgreSQL: read and save rows as C# objects |
| Company.Shared | `BCrypt.Net-Next` 4.2.0 | Checking and making bcrypt password hashes |

```powershell
dotnet add src\Company.Shared package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add src\Company.Shared package BCrypt.Net-Next
```

## 7.2 Data classes (Company.Shared/Models)

One C# class per table. `[Table]` and `[Column]` tie each class and property to the exact table and column names from the SQL scripts (which use lower_case_with_underscores). Fixed lists like levels and statuses are constants, so the code never has typos like "Admn".

<!-- include: src/Company.Shared/Models/User.cs -->

<!-- include: src/Company.Shared/Models/LoginLog.cs -->

<!-- include: src/Company.Shared/Models/PasswordRequest.cs -->

## 7.3 The database context

`AppDbContext` is the connection Entity Framework uses. The tables are **created by the SQL scripts, not by Entity Framework**, so this class only maps classes to tables.

<!-- include: src/Company.Shared/Data/AppDbContext.cs -->

> **Bug found while testing.** Npgsql treats `DateTime` as "timestamp *with* time zone" and refuses local times, but our tables use plain `timestamp`. The `ConfigureConventions` block tells it every `DateTime` is a plain `timestamp`. Error before the fix: *Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp with time zone'*.

## 7.4 Per-PC database settings

Each PC saves where the database is in `C:\ProgramData\TriStone\OfficeApp\dbsettings.json`. That's what lets the same installed app work on `localhost` now and on `192.168.1.50` in the office later, without rebuilding.

The database password in that file is encrypted with **Windows DPAPI** (`ProtectedData`, built into .NET 10 for Windows). Only that PC can decrypt it; a copied file is useless elsewhere.

`TestConnectionAsync` turns PostgreSQL's error codes into plain messages: `28P01` = wrong password, `3D000` = database missing, socket/timeout = server unreachable.

<!-- include: src/Company.OfficeApp/Data/DbSettings.cs -->

`Db` is the one place pages get a database connection from:

<!-- include: src/Company.OfficeApp/Data/Db.cs -->

## 7.5 Database Settings window

Shown on the first run on a new PC, or whenever the database can't be reached (the flowchart's "Database connected? No" branch). **Save** only saves settings that actually connect. Layout is plain for now; it will be styled from the Figma design.

<!-- include: src/Company.OfficeApp/Forms/DatabaseSettingsForm.cs -->

The `.Designer.cs` file next to it holds the control positions; open the form in Visual Studio's designer to see or move them.

# Chapter 8: Signing in

## 8.1 The sign-in rules (AuthService)

| Situation | What happens | Logged as |
|---|---|---|
| Username doesn't exist | "Wrong username or password." | Failed Login, "Unknown username" |
| Account Disabled | "This account is turned off..." | Failed Login, "Account disabled" |
| Account Locked | "This account is locked..." (even with the right password) | Failed Login, "Account locked" |
| Wrong password | Counter +1, "N attempts left" | Failed Login, "Wrong password (n of 5)" |
| 5th wrong password | Status becomes Locked | Failed Login + Account Locked |
| Right password | Counter reset to 0, last sign-in time saved | Login |
| Sign Out (button or closing the window) | Session cleared | Logout |

Usernames are matched without caring about upper/lower case. Every log row also stores the PC name (`Environment.MachineName`).

<!-- include: src/Company.OfficeApp/Services/AuthService.cs -->

`Session` remembers who is signed in on this PC; pages read it to decide what to show.

<!-- include: src/Company.OfficeApp/Services/Session.cs -->

## 8.2 Login window

Control names match the UI Elements Guide (Part A: Login Window). The layout is plain and left-aligned on purpose; it will be styled once the Figma design is ready. Extra control: **Database settings** link, to change the server address from the Login window.

<!-- include: src/Company.OfficeApp/Forms/LoginForm.cs -->

## 8.3 Main window (placeholder) and start-up

For now the main window only shows who is signed in and a **Sign Out** button. The sidebar and pages come next.

<!-- include: src/Company.OfficeApp/Forms/MainForm.cs -->

Start-up order: check the database (show Database Settings if needed), then Login, then Main window; after Sign Out, back to Login.

<!-- include: src/Company.OfficeApp/Program.cs -->

## 8.4 Testing

The rules were tested against the real database with a temporary Employee account (`zz_test`), which was deleted afterwards together with its log rows. 14 checks passed:

- Database settings: right settings connect; wrong port, wrong password and missing database each give the right plain message; the saved password is encrypted.
- Sign in: unknown username refused; right password works even with different letter case; sign out clears the session; wrong passwords count down 4, 3, 2, 1; the 5th locks the account; a locked account is refused even with the right password.
- `login_logs` received one correct row per attempt, with the PC name.

# Chapter 9: Forgot Password

## 9.1 The rules

From the Project Outline (Part 0): only Admins change passwords; Employees and Supervisors can't reset their own.

| Who forgot | What happens |
|---|---|
| Employee / Supervisor | Forgot Password sends a **request** to the Admins (`password_requests`, status Waiting). A second request while one is waiting is not duplicated. Works even if the account is Locked. |
| Admin | Answers their **security question**, then types a new password. Wrong answers are counted; after 10 failures in total only another Admin can reset it. |
| Admin with no security question set | Told to ask another Admin. |
| Disabled account | Refused: "This account is turned off." |

When an Admin sets a new password (or an Admin resets their own with the security question):

- the password is replaced with a new bcrypt hash;
- a **Locked** account becomes **Active** and the wrong-password counter goes back to 0;
- every waiting request from that person is marked **Done**, with which Admin handled it and when;
- `login_logs` gets *Password Changed* (and *Account Unlocked* if it was locked).

**New password rules:** at least 8 characters, with at least one letter and one number. The **Generate** button makes a random 10-character password without look-alike characters (no 0/O or 1/l/I), so it's easy to read out loud.

**Security answers** ignore spaces at the ends and capital letters ("Manila " = "manila"). They are stored as bcrypt hashes, like passwords.

## 9.2 The code

<!-- include: src/Company.OfficeApp/Services/PasswordService.cs -->

## 9.3 Forgot Password window

One window with three steps on top of each other (three `Panel`s); only one is visible at a time.

<!-- include: src/Company.OfficeApp/Forms/ForgotPasswordForm.cs -->

## 9.4 Admin side: requests and Set New Password

For now the main window has a **Password requests (N)** button, shown to Admins only. It becomes the notification bell in the top bar when the sidebar is built.

<!-- include: src/Company.OfficeApp/Forms/NotificationsForm.cs -->

<!-- include: src/Company.OfficeApp/Forms/SetNewPasswordForm.cs -->

The main window refreshes the count when it opens and after the Notifications window closes:

<!-- include: src/Company.OfficeApp/Forms/MainForm.cs -->

## 9.5 Testing

Tested against the real database with three temporary accounts (an Employee, a Locked Supervisor and an Admin with a security question), deleted afterwards with their log rows and requests. 22 checks passed, including:

- requests are sent once, even from a Locked account; the waiting count and list are right;
- password rules refuse short, letter-only and mismatched passwords; generated passwords pass;
- a non-Admin can't set passwords; an Admin setting one unlocks the account, resets the counter and closes the request; Dismiss closes a request without changing the password;
- the person signs in with the new password;
- an Employee can't use the security question; a wrong Admin answer is refused; a weak new password is refused; the right answer with extra spaces and capitals works.

# Chapter 10: Main window and the idle check

## 10.1 Who sees which page

All page rules live in one class, `Access`. The sidebar uses it to show or hide buttons, and `ShowPage` checks it again before opening anything.

| Page | Admin | Supervisor | Employee |
|---|---|---|---|
| Dashboard | yes | yes (own department's numbers) | no |
| Projects, Website Admin, User Accounts | yes | no | no |
| HR & Payroll / Inventory / Subcontractors / Receipts | all | own department only | own department only |
| My Account | yes | yes | yes |

The first page after signing in is the Dashboard if the person can see it, otherwise their department's page.

<!-- include: src/Company.OfficeApp/Services/Access.cs -->

## 10.2 The main window

Three areas, all docked so they resize with the window:

- `pnlSidebar`: a `FlowLayoutPanel` on the left. Buttons are stacked top to bottom, and hidden buttons take no space, so each role gets a tidy list.
- `pnlTopBar`: page title, the person's name, department and level, and (Admin only) the notification button.
- `pnlContent`: the chosen page loads here. Pages that aren't built yet show a placeholder.

**Docking order matters.** In the designer, controls added *last* dock *first*. The sidebar is added last so it takes the full height on the left, and the top bar then spans the rest.

**Closing with X.** The sign-out has to be written to the log *before* the window closes. Otherwise the app could exit before the row is saved. `MainForm_FormClosing` cancels the first close, writes the log, then closes again.

<!-- include: src/Company.OfficeApp/Forms/MainForm.cs -->

<!-- include: src/Company.OfficeApp/Pages/PlaceholderPage.cs -->

## 10.3 The "Still there?" idle check

- `IdleMonitor` is an `IMessageFilter`: it sees every keyboard and mouse message in the app (and lets them through untouched) and remembers the time of the last one.
- The main window's `tmrIdle` checks once a second. After 5 minutes without activity it opens the popup.
- The popup counts down 60 seconds. **Stay Signed In** resets the clock. **Sign Out** signs out normally. If nobody answers, it signs out by itself and the log says **Idle Sign Out**. The Login window then shows *"You were signed out after 5 minutes without activity."*

**Testing tip.** In Debug builds, set the environment variable `TRISTONE_IDLE_SECONDS` (for example to `20`) to see the popup without waiting 5 minutes. In Visual Studio: project **Properties > Debug > Open debug launch profiles UI > Environment variables**. Release builds always use 5 minutes.

<!-- include: src/Company.OfficeApp/Services/IdleMonitor.cs -->

<!-- include: src/Company.OfficeApp/Forms/IdleWarningForm.cs -->

The start-up loop passes the idle message to the Login window:

<!-- include: src/Company.OfficeApp/Program.cs -->

## 10.4 Testing

A test program opened the real main window as three temporary accounts and ran the idle check with a 3-second timeout (accounts and their log rows deleted afterwards). 9 checks passed:

- Admin sees all 9 pages and the notification button, and starts on Dashboard.
- HR Employee sees only HR & Payroll and My Account, starts on HR & Payroll, and has no notification button.
- Inventory Supervisor sees Dashboard, Inventory and My Account.
- The popup appears about 3 seconds after the last activity. **Stay** keeps the session (and the popup comes back after the next idle period). **Sign Out** signs out as a normal Logout. No answer signs out after 60 seconds as **Idle Sign Out**.

# Chapter 11: My Account

## 11.1 The rules

- Everyone sees their name, username, department and level.
- **Only Admins** can change their own password and set a security question. Employees and Supervisors see *"Only Admins can change passwords... ask an Admin"* instead; the service refuses them too, so the rule can't be bypassed.
- Both changes ask for the **current password** first, so nobody can change them on a PC someone left signed in.
- New passwords follow the same rules as everywhere (8+ characters, a letter and a number) and must differ from the current one.
- The security answer is stored as a bcrypt hash of the answer with spaces trimmed and in lower case, the same way Forgot Password checks it.

## 11.2 Edit history

Setting the security question writes one row to `audit_logs` (the first use of that table): table `users`, action *Edited*, the old and new **question**, and `"security_answer": "(changed)"`. The answer itself is never written anywhere readable. Password changes go to `login_logs` as *Password Changed*.

<!-- include: src/Company.Shared/Models/AuditLog.cs -->

## 11.3 The code

<!-- include: src/Company.OfficeApp/Services/AccountService.cs -->

The page (plain layout for now). `MainForm.ShowPage` now loads it for `AppPage.MyAccount`; every other page still shows the placeholder.

<!-- include: src/Company.OfficeApp/Pages/MyAccountPage.cs -->

## 11.4 Testing

17 checks against the real database, with a temporary Employee and Admin (deleted afterwards with all their log and history rows):

- Employee: password and security parts hidden, "ask an Admin" shown; the service refuses both changes.
- Admin: both parts shown. The security question is refused with a wrong current password, an empty question or an empty answer, and saved otherwise.
- Own password: refused if the current password is wrong, the new one is weak, or it's the same as the current one; otherwise changed, and signing in with it works.
- Full loop: the question set in My Account works in Forgot Password with the answer typed in capitals, and signing in after the reset works.
- The `audit_logs` row has the question but not the answer.

# Chapter 12: User Accounts

## 12.1 The rules

| Rule | Why |
|---|---|
| Only Admins can use this page or its service | Project Outline: only Admin creates accounts |
| Username: 3 to 50 letters, numbers, `.` `-` `_`; no spaces; unique ignoring capital letters | Sign-in matches usernames ignoring case, so "Juan" and "juan" can't both exist |
| Admins have no department; Supervisors and Employees have exactly one | Same rule the database enforces (`chk_users_department`) |
| First password follows the normal password rules | 8+ characters, a letter and a number |
| Second active person at the same department + level: the Admin is **asked**, not blocked | The plan is 1 Employee + 1 Supervisor per department, but a temporary replacement may be needed |
| You can't disable yourself or remove your own Admin level | Avoids locking yourself out by accident |
| The last active Admin can't be disabled or demoted | There must always be someone who can manage accounts |
| Unlock and Enable reset the wrong-password counter | A fresh start after an Admin steps in |

**For now the full name is typed.** Once HR & Payroll's Employee list exists, it becomes a dropdown of office staff and fills in `users.employee_id`.

Every create, edit, unlock, disable and enable writes an `audit_logs` row with the before/after **name, username, level, department and status**. Passwords and hashes are never included. Unlocks also write *Account Unlocked* to `login_logs`.

## 12.2 The code

<!-- include: src/Company.OfficeApp/Services/UserAdminService.cs -->

<!-- include: src/Company.OfficeApp/Pages/UserAccountsPage.cs -->

## 12.3 Testing

23 checks against the real database, with temporary accounts (deleted afterwards with their log and history rows):

- A non-Admin can't create accounts. A username with a space, an Admin with a department, an Employee without one, and a weak first password are all refused. The same username in different capitals is refused.
- A new account can sign in. A second Inventory Employee triggers the "already has" question. Renaming and changing the level shows up in the list and search.
- 5 wrong passwords lock the account, Unlock lets it sign in again, Disable stops sign-in ("turned off"), and Enable brings it back.
- An Admin can't disable themselves or remove their own Admin level.
- The page lists accounts, search filters as you type, and clicking a row loads it into the form with the password box greyed out.
- The edit history rows contain no password or hash.

**Not tested yet:** the "last active Admin" protection. Your real Admin account is always a second active Admin, and it shouldn't be disabled just for a test. It will be tested on a separate test database before installation.

# Chapter 13: Projects

## 13.1 The table (script 002)

The one shared list of company projects (DFD D1.1 / D5.3). Receipts, Inventory, Subcontractors and Website Admin will point to it. Added with the setup tool, which ran only the new script:

```
  skip  001_login_security.sql (already applied)
  run   002_projects.sql
```

<!-- include: database/002_projects.sql -->

Dates are `DATE` (no time) and map to C# `DateOnly`, so there are no time-zone surprises. Money is `NUMERIC(14,2)`, never floating point, so pesos add up exactly.

## 13.2 The rules

- **Admin only**, like the outline says. Other departments only pick projects from dropdowns.
- Name, site and sector are required. The price can't be negative, and the target end date can't be before the start date (also enforced by the database).
- **Completion date is automatic:** it's set to today when the status becomes *Done*, and cleared if the project is reopened. This gives "Finished in N days" and the website's completion date.
- **Days:** "Day 45" while running, "Starts in 10 days" if the start is in the future, "Finished in N days" when done, "Cancelled".
- **Revenue** = contract price for now (+ approved change orders when Receipts is built). **Expenses to date** = 0 for now; receipts, materials and subcontractor payouts are added as those parts are built. Payroll is company-wide overhead, not per project.
- **Delete** is allowed only while nothing points to the project (`CountLinkedRecordsAsync`, which grows with Parts 3–5). After that, use *Cancelled* so history stays.
- Every add, edit and delete goes to `audit_logs`. A shared helper, `AuditService`, now writes those rows for every page.

<!-- include: src/Company.Shared/Models/Project.cs -->

<!-- include: src/Company.OfficeApp/Services/ProjectService.cs -->

<!-- include: src/Company.OfficeApp/Services/AuditService.cs -->

## 13.3 Export to Excel

`ExcelExport` (using the **ClosedXML** package, added to Company.OfficeApp) saves any list as `.xlsx`, with a bold header row, the header frozen, auto-width columns, and real numbers and dates (not text), so Excel can add them up. Every page's Export button will use it.

<!-- include: src/Company.OfficeApp/Services/ExcelExport.cs -->

## 13.4 Print Report

`ProjectReport` draws a plain one-page report (details, days, revenue, expenses, profit, printed by and when) on a `PrintDocument`, shown in a print preview. To make a PDF, choose the **Microsoft Print to PDF** printer.

<!-- include: src/Company.OfficeApp/Services/ProjectReport.cs -->

## 13.5 The page

<!-- include: src/Company.OfficeApp/Pages/ProjectsPage.cs -->

## 13.6 Testing

23 checks against the real database, with a temporary Admin and Employee and projects named `ZZTEST...` (all deleted afterwards):

- An Employee can't add projects. Empty names, end before start, negative prices and unknown sectors are refused.
- Saved values come back exactly. Days show "Day 45". Money shows as ₱2,500,000.00.
- Marking *Done* sets today's completion date ("Finished in 45 days"), and reopening clears it. A future start shows "Starts in 10 days".
- Search (including by client), the status filter and the sector filter work.
- The Excel file has a header, the right number of rows, prices stored as numbers, and Yes/No for the website tick.
- The print report renders one page.
- The page's search, peso column, form loading and price limit (up to ₱9,999,999,999.99) work. Deleting an unused project works.
- Edit history: Added, Edited (Done with its date), Edited (reopened), Deleted.

# Chapter 14: HR & Payroll: Employees

## 14.1 The tables (script 003)

- `employees`: office staff **and** site workers. Department is one of Administration, HR & Payroll, Inventory, Subcontractors, Receipts or **Site**. The pay schedule follows the department (the database checks it): **Site** = paid every Saturday, everyone else = **Office**, paid the 15th and 30th.
- `employee_rates`: the **daily rate history**. A new rate starts on a date HR chooses, so past payslips keep the rate they were paid with. One rate per employee per date.
- `users.employee_id` now points to `employees`, so a login can belong to an employee. (The User Accounts page will offer a dropdown of office staff next.)

<!-- include: database/003_employees.sql -->

## 14.2 The rules

| Rule | Detail |
|---|---|
| Who | Admins and the HR department (Employee and Supervisor). Others are refused even if they get to the code. |
| Required | Full name, department, daily rate (more than 0, at most ₱100,000) |
| Biometric ID | Optional; up to 20 letters, numbers or dashes; can't belong to two people (the error says who has it) |
| Changing the rate | Type the new rate and the date it applies from. If a rate already starts on that date, it's corrected; otherwise a new row is added. Old rates stay. |
| Leaving | **Mark Inactive**: records kept, and any login of theirs is turned **off**. **Reactivate** brings the employee back, but the login stays off until an Admin turns it on. You can't mark yourself Inactive, or the last active Admin. |
| Government IDs | Optional. In Edit History only *whether* each is on file is recorded, never the number. |
| Tabs | Settings and Edit History are hidden for the HR Employee (Admin and HR Supervisor only). |

<!-- include: src/Company.Shared/Models/Employee.cs -->

<!-- include: src/Company.OfficeApp/Services/EmployeeService.cs -->

## 14.3 The page

`HrPayrollPage` holds the 7 tabs. Each tab's content is its own `UserControl` in `Pages/Hr`, so each file stays small. Only Employees is built so far.

<!-- include: src/Company.OfficeApp/Pages/HrPayrollPage.cs -->

<!-- include: src/Company.OfficeApp/Pages/Hr/EmployeesTab.cs -->

## 14.4 Generating big designer files

Forms with dozens of controls were getting long to write by hand, so `docs/_tools/designer-gen/gen_designer.py` writes the `.Designer.cs` file from a short list: control type, name, position, size and extra properties. The output is normal designer code, so Visual Studio's designer opens it like any other form.

```powershell
python docs\_tools\designer-gen\gen_designer.py docs\_tools\designer-gen\specs\EmployeesTab.py src\Company.OfficeApp\Pages\Hr\EmployeesTab.Designer.cs
```

**Use it only to create a form.** After someone moves things in Visual Studio's designer (for example when styling from Figma), regenerating would overwrite those changes. From then on, edit in the designer.

## 14.5 Testing

23 checks against the real database, with temporary accounts and employees named `ZZTEST...` (all deleted afterwards):

- An Inventory employee can't add employees. Missing department, zero rate and a biometric ID with a space are refused.
- An HR Employee adds a site worker (Site schedule) and office staff (Office schedule). A duplicate biometric ID is refused with the owner's name.
- The list shows today's rate. The schedule filter and search by biometric ID work.
- A raise from next week keeps today at ₱600 and next week at ₱650. Correcting next week to ₱700 replaces that row (2 rows in history). Editing details without changing the rate adds no row.
- Marking office staff Inactive turns their login off (sign-in says "turned off"). Reactivating leaves the login off.
- The HR Employee sees 5 tabs, and the HR Supervisor sees all 7.
- The tab's search, peso column, form loading and rate history display work.
- Edit history is filed under HR & Payroll, and no government ID number appears in it.

# Chapter 15: Linking logins to employees

## 15.1 What changed

Before HR & Payroll existed, the User Accounts page had a typed **Full name**. Now every new login is made **for an office employee**, picked from a dropdown, and the login takes that employee's **name** and **department**.

| Rule | Detail |
|---|---|
| Who can get a login | Active employees who are not site workers and don't have a login yet |
| One login per employee | Checked by the app, and by the database (script 004, a unique index on `users.employee_id`) |
| Department | Comes from the employee: none for Admins; otherwise the employee's department. Someone in *Administration* can only get an Admin login. |
| Old accounts | Accounts made before employees existed (like the first Admin) can stay unlinked, or be linked later. Once linked, they can't be unlinked. |
| HR changes flow through | Renaming an employee or moving them between login departments updates their login. Moving someone with a Supervisor or Employee login to *Site* or *Administration* is refused until the login is changed. |
| Username suggestion | First letter of the first name + surname: Juan Dela Cruz -> `jdelacruz`, Shem John Adrian P. Morilla -> `smorilla`. Surname words like *de, dela, de los, san, sta* are kept. |

<!-- include: database/004_one_login_per_employee.sql -->

The updated service (`LinkableEmployeesAsync`, `ResolveAsync`, `DepartmentFor`):

<!-- include: src/Company.OfficeApp/Services/UserAdminService.cs -->

<!-- include: src/Company.OfficeApp/Pages/UserAccountsPage.cs -->

## 15.2 Bug found while testing: "Add" jumped back to editing

On the User Accounts, Projects and Employees pages, clicking **Create Account / Add Project / Add Employee** could jump straight back to editing the first row. `ClearSelection()` removes the highlight, but the grid still has a `CurrentRow`, and the pages were reading `CurrentRow`. All three now read `SelectedRows` instead:

```csharp
// SelectedRows, not CurrentRow: after ClearSelection (Add ...) the grid still has a current row.
ProjectRow? Selected => dgvProjects.SelectedRows.Count > 0 ? dgvProjects.SelectedRows[0].DataBoundItem as ProjectRow : null;
```

## 15.3 Testing

29 checks against the real database (test data deleted afterwards):

- The dropdown lists only Active office staff without a login, as "Name (Department)".
- No employee, a site worker, an Inactive employee, or an Administration employee at Employee level are each refused.
- A login takes its name and department from the employee. An Administration employee as Admin gets no department. People with logins leave the dropdown (except on their own account). A second login for the same employee is refused by the app and by the database.
- An old unlinked account can stay unlinked, but a linked one can't be unlinked. Promoting to Supervisor keeps the department.
- Renaming an employee and moving them to Receipts updates their login. Moving a Supervisor's employee to Site is refused. An Admin's employee can move freely.
- The page: Create Account stays on "New account" (the bug above, now also checked on Projects and Employees). The username suggestion and the shown-only department work. Choosing Admin clears the department.
- Username suggestions: jdelacruz, adelossantos, smorilla, msanjuan, cher.

# Chapter 16: Responsive pages

"Responsive" here means two things: **fits any screen** and **never feels stuck**.

## 16.1 Fits any screen

- **Sharp on high-resolution screens:** `<ApplicationHighDpiMode>PerMonitorV2</ApplicationHighDpiMode>` in `Company.OfficeApp.csproj`. Text and controls scale correctly on laptop screens set to 125% or 150%, and when the window moves to another monitor.
- **Main window opens maximized** (`WindowState = Maximized`; minimum 800 x 500).
- **List pages use the space:** `Responsive.FitGrid(page, grid)` makes the table grow wider and taller when the window is bigger than the page's design size, and moves everything below the table down with it. When the window is *smaller*, nothing shrinks; the page scrolls (`AutoScroll`). It works out positions relative to the scroll position, so scrolling never misplaces controls.

## 16.2 Never feels stuck

- **Search waits for a pause in typing** (`Responsive.OnTypingPause`, 300 ms). Typing "santos" searches once, not six times. The search boxes' `TextChanged` handler was removed from the designer files and wired in code instead.
- **Only the newest results show** (`LatestOnly`). If the filters change quickly, an older, slower query can't finish last and overwrite the newer list.
- **Wait cursor while loading** (`Responsive.Busy`).
- **Database warm-up:** Entity Framework sets itself up on its first query (about 1-2 seconds). `Db.WarmUpInBackground()` runs one tiny query while the Sign In window is open, so the real sign-in is quick (measured 0.2 s afterwards).
- All database work was already `async`, so the window keeps responding while the database works.

<!-- include: src/Company.OfficeApp/UI/Responsive.cs -->

Each list page calls it in its constructor, for example:

```csharp
Responsive.FitGrid(this, dgvProjects);
Responsive.OnTypingPause(txtProjectSearch, () => LoadProjectsAsync());
```

and each load uses the ticket:

```csharp
var ticket = loads.Start();
using var busy = Responsive.Busy(this);
var rows = await ProjectService.ListAsync(...);
if (!loads.IsLatest(ticket)) return;   // a newer search already started
dgvProjects.DataSource = rows;
```

**Every new list page should do the same.**

## 16.3 Testing

11 checks (test data removed afterwards):

- After the warm-up, the first real query took 193 ms. The main window opens maximized.
- At 1600 x 1000, the tables on Projects, User Accounts and Employees grow to 1570 pixels wide and 400-570 tall, and the Save button moves down.
- At 800 x 500, nothing shrinks and each page scrolls (scroll area at least the design size).
- Typing "ZZTEST Project 2" letter by letter left the list unchanged until the pause, then showed 1 row.
- An older load is ignored once a newer one has started.

# Chapter 17: Inventory & Vendors

## 17.1 The tables (script 005)

| Table | Holds |
|---|---|
| `app_settings` | Settings every PC shares. First one: `file_storage_path`, the folder for uploaded scans (`C:\TriStoneFiles` while building; a shared network folder on the server in Phase 6) |
| `materials` | Name, category, unit, latest price, stock, minimum stock |
| `material_usages` | Material used on a project: Waiting / Approved / Rejected, with the cost fixed when approved |
| `vendors`, `vendor_documents` | Suppliers with bank details and VAT status; their papers (company profile, bank details, certificate of registration) |
| `quotes`, `purchases` | Price comparison: quotes from several vendors; the chosen one becomes a purchase |
| `deliveries` | What arrived, with the receipt: a scan, or "physical copy only" with where the paper is filed (the database refuses a delivery with neither) |
| `equipment`, `maintenance_logs` | Tools, heavy equipment and vehicles (owned or rented); maintenance and repair history |

<!-- include: database/005_inventory.sql -->

## 17.2 The rules

- **Stock is never typed in after a material is added.** It goes down only through *approved* usage and up only through deliveries. That keeps the stock trustworthy.
- **Usage:** the Inventory Employee records it; the Inventory Supervisor (or an Admin) approves. On approval the stock drops and the cost (quantity x the material's price at that moment) becomes that project's material expense. Using more than what's in stock is refused.
- **Low stock:** below the minimum shows light red; out of stock shows stronger red; the tab says "Materials (N low)".
- **Price comparison:** the cheapest open quote is highlighted; the Supervisor approves one as the purchase and the other open quotes for that material close.
- **Deliveries:** no approval (the purchase was already approved). Stock goes up and the material's price becomes the delivered price.
- **Equipment:** rented equipment needs the rental company and rate. When the next maintenance date arrives the row turns light yellow and the tab says "Equipment (N due)". A maintenance entry can move that date. Repair costs are company-wide overhead.
- **Deleting** a material, vendor or piece of equipment is refused once it has history; use the history-keeping alternative (Retired for equipment).
- **Who:** Admins and the Inventory department. Approve buttons only for the Supervisor and Admins. Edit History for the Supervisor and Admins.
- Vendor bank account numbers are kept out of the readable Edit History (only "on file: yes/no").

## 17.3 Shared pieces added for this part (used by later parts too)

**File storage.** `FileStore` copies an uploaded file into the storage folder under `folder\yyyy\MM\` with a random prefix, and the database keeps only that relative path. Only PDF, images and Word files up to 25 MB are accepted.

<!-- include: src/Company.OfficeApp/Services/FileStore.cs -->

**Who approves.** `Access.IsApprover(user, department)`: Admins and that department's Supervisor.

**Project money in one place.** `ProjectFinance` adds up each project's revenue and expenses; the Projects page, the project report and (later) the Dashboard all use it, so they always agree. Approved material use is its first source of expenses.

<!-- include: src/Company.OfficeApp/Services/ProjectFinance.cs -->

**Edit History tab.** One reusable tab (`Pages/Shared/EditHistoryTab`) used by every department, one row per changed field, filterable by date, person and record, exportable to Excel. HR & Payroll's Edit History tab now works too.

## 17.4 The code

<!-- include: src/Company.OfficeApp/Services/MaterialService.cs -->

<!-- include: src/Company.OfficeApp/Services/PurchasingService.cs -->

<!-- include: src/Company.OfficeApp/Services/EquipmentService.cs -->

The page holds 7 tabs, each its own UserControl in `Pages/Inventory`; a change in one tab refreshes the others that show the same data:

<!-- include: src/Company.OfficeApp/Pages/InventoryPage.cs -->

The designer generator now also makes pop-up windows (`FORM = True` in a spec), used for the Maintenance Entry window.

## 17.5 Testing

38 checks against the real database, with temporary accounts, a project, materials, vendors and equipment (all deleted afterwards, including the stored test file):

- Someone from Receipts can't add materials; duplicates, negative prices and using more than the stock are refused; low stock is detected.
- The Employee can't approve; the Supervisor approves (stock 100 -> 80, project expense 20 x 250 = 5,000, profit 95,000) and rejects; approving twice is refused.
- A project with material use and a material with history can't be deleted.
- Vendors (duplicates and bad emails refused); a vendor paper is copied into storage; a .exe is refused.
- Two quotes: the cheapest is first; the Employee can't approve; the Supervisor's approval closes the other quote.
- Deliveries: no receipt, or paper-only without a filed location, is refused; a scanned delivery raises stock 80 -> 130 and sets the price to 240 and marks the purchase delivered; a paper-only delivery is saved.
- Equipment: rented without a company is refused; a past maintenance date shows as due; a maintenance entry moves the reminder; its cost counts as overhead; equipment with history can't be deleted.
- Edit History has the changes but not the bank account number. The Inventory Employee sees 6 tabs, the Supervisor 7.

# Chapter 18: Subcontractors

## 18.1 The tables (script 006)

| Table | Holds |
|---|---|
| `subcontractors`, `subcontractor_trades` | The directory: company details, two contacts, addresses, bank details, PCAB license; one row per ticked trade |
| `sub_jobs`, `sub_job_stages` | A job = subcontractor + project + scope + amount + dates + retention %; its payment stages (must total 100%) |
| `sub_job_documents`, `sub_progress` | Contract/scope papers; progress updates (% done, date, notes, optional file) |
| `sub_vouchers` | Billing vouchers (SV-0001...) for a stage or for the retention release: Waiting / Approved / Rejected / Paid |
| `sub_payouts` | What was paid for an approved voucher (date, amount, method, reference) |

Two partial unique indexes make sure a stage (and a job's retention) has at most one voucher that isn't Rejected.

<!-- include: database/006_subcontractors.sql -->

## 18.2 The rules

- **Stages:** each pays contract x stage %; the last stage takes any rounding centavos so the stages always add up to the contract exactly. Retention = stage amount x retention %; payable = amount - retention.
- **Order of billing:** Employee marks a stage reached -> prepares the voucher -> Supervisor (or Admin) approves or rejects -> payout recorded for exactly the voucher's amount (bank transfer and check need a reference number). A rejected voucher sends the stage back to "Reached".
- **Retention release:** only when the latest progress is 100% and every stage is paid; it gets its own voucher and approval.
- **Locking:** once a stage is reached or a voucher exists, the subcontractor, project, amount, retention and stages can't change; scope and dates still can.
- **Money:** payouts (what was actually paid, including released retention) are the project's subcontractor expense in `ProjectFinance`. A project with subcontractor jobs can't be deleted.
- **PCAB:** expiring within 30 days = yellow row, expired = light red; the tab title says "Directory (N PCAB expiring)".
- **Deleting:** a subcontractor with jobs can't be deleted; a job only while it has no progress, papers or billing.
- The TIN and bank account number are kept out of the readable Edit History (only "on file: yes/no").

## 18.3 The code

<!-- include: src/Company.OfficeApp/Services/SubcontractorService.cs -->

<!-- include: src/Company.OfficeApp/Services/SubJobService.cs -->

<!-- include: src/Company.OfficeApp/Services/SubBillingService.cs -->

The voucher print layout (same approach as the project report):

<!-- include: src/Company.OfficeApp/Services/VoucherPrint.cs -->

The page holds 4 tabs, each its own UserControl in `Pages/Subcontractors`:

<!-- include: src/Company.OfficeApp/Pages/SubcontractorsPage.cs -->

## 18.4 Testing

56 checks against the real database (temporary accounts, project, subcontractors and files, all deleted afterwards):

- Someone from Receipts can't add subcontractors; duplicates, no trade ticked and bad emails are refused; trade filter; PCAB expiring in 10 days is flagged.
- Stages that add up to 90% are refused; stage amounts always add up to the contract after rounding.
- Stage 1 of a 100,000 job: 30,000 less 10% = 27,000. A voucher before the stage is reached is refused; the amount can't change after billing starts but the scope can.
- The Employee can't approve; the Supervisor approves; payout before approval, a wrong amount, a missing reference and paying twice are all refused.
- After stage 1: project expense 27,000, profit 973,000; retention held 3,000; remaining 73,000.
- Stage 2 rejected, then a new voucher approved and paid; stage 3 paid in cash.
- Retention release before 100% is refused; after a 100% progress update it releases 10,000; the job ends "Paid in Full" with 100,000 as project expense.
- The voucher prints with the bank details and approver. Deletes are blocked where history exists.
- Edit History has the changes without the TIN or account number. The Employee sees 3 tabs, the Supervisor 4; all tabs fill with data.

# Chapter 19: Receipts & change orders

## 19.1 The tables (script 007)

| Table | Holds |
|---|---|
| `receipts` | Every expense: receipt no., date, merchant, amount, category, Project (with project_id) or Overhead, the copy (scan file, or paper + where filed, or no receipt + reason), Waiting / Approved / Rejected |
| `change_orders` | CO-001...: project, date requested, description, cost change (+/-), days change (+/-), signed document, Waiting / Approved / Rejected |

Two checks in the database: a Project receipt must have a project (and an Overhead one must not); and every receipt has exactly one kind of copy (scan, paper with location, or "no receipt" with a reason).

Delivery receipts are **not** copied into `receipts`: the vault reads them straight from Inventory's `deliveries` and shows them as "Stock purchase". That way nothing is stored twice and stock purchases can never be counted as project expenses by mistake.

<!-- include: database/007_receipts.sql -->

## 19.2 The rules

- **Receipts:** the Receipts Employee adds; the Receipts Supervisor (or an Admin) approves or rejects, one or several at once. Only approved receipts count. Waiting or rejected ones can be edited (saving sends them back to Waiting); approved ones can't.
- **Money:** approved Project receipts are project expenses in `ProjectFinance`; approved Overhead receipts are company overhead (Dashboard).
- **No receipt:** needs a reason and shows under Needs Review until decided. Attaching a receipt later clears the flag and keeps the decision.
- **Change orders:** logged by the Receipts department, approved by an Admin only. On approval the cost change is added to revenue (`ProjectFinance.ChangeOrders`) and the days move the project's target end date (also written to the Admin's project history). The revised price can't go below zero; a change order needs a cost or days change.
- A project with receipts or change orders can't be deleted.

## 19.3 The code

<!-- include: src/Company.OfficeApp/Services/ReceiptService.cs -->

<!-- include: src/Company.OfficeApp/Services/ChangeOrderService.cs -->

Reports build a list of rows; `TableReport` prints any such list over as many pages as needed and can save it straight to PDF with Windows' "Microsoft Print to PDF" printer:

<!-- include: src/Company.OfficeApp/Services/ReceiptReports.cs -->

<!-- include: src/Company.OfficeApp/Services/TableReport.cs -->

`ProjectFinance` now has all four sources:

<!-- include: src/Company.OfficeApp/Services/ProjectFinance.cs -->

The page holds 5 tabs (Pages/Receipts) plus two pop-up windows: Receipt Entry (also used for "Attach Receipt") and Receipt Viewer.

<!-- include: src/Company.OfficeApp/Pages/ReceiptsPage.cs -->

## 19.4 Testing

55 checks against the real database (temporary accounts, project, receipts, a delivery and files, all deleted afterwards):

- Inventory staff can't add receipts; missing number, missing copy, paper without location, project expense without a project, future dates and no-receipt without a reason are refused.
- Waiting receipts don't count; the Employee can't approve; the Supervisor approves two at once and rejects one; approved ones can't be edited.
- The approved project receipt (1,250) counts, the rejected one doesn't, the overhead one counts as overhead; the approved no-receipt expense (80) brings it to 1,330; attaching its receipt later moves it to the vault, still approved.
- A delivery from Inventory shows as a 2,500 stock purchase, isn't shown under the project and isn't counted.
- Change orders: CO-001 format; preview 500,000 -> 550,000 and end date +10 days; Receipts staff and the Receipts Supervisor can't approve; the Admin's approval changes revenue and the end date; a rejected one changes nothing; below-zero prices refused.
- The CO form, receipt sheets with the scan, a 4-page report and a PDF all print. Weekly and monthly summaries add up.
- Employee sees 4 tabs, Supervisor 5; "Needs Review (1)"; the viewer shows the scan.

# Coming next

- Chapter 20: Dashboard
- Later chapters: each part of the app, then the installers (Phase 6)
