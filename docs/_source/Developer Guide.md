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

# Coming next

- Chapter 7: Connecting the desktop app to the database (connection settings, "Database Settings" screen)
- Chapter 8: The Login window and Main Window
- Later chapters: each part of the app, then the installers (Phase 6)
