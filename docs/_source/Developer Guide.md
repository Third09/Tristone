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

# Coming next

- Chapter 9: Forgot Password (request to Admins, Admin security question)
- Chapter 10: Main window sidebar by role, and the "Still there?" idle check
- Later chapters: each part of the app, then the installers (Phase 6)
