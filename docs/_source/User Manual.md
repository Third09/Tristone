# Tri-Stone Office App: User Manual

How to use the Tri-Stone office app, screen by screen. This manual grows as each screen is finished; sections marked *(coming soon)* are planned but not built yet.

# 1. Who can do what

Every account has one **Level** and (except Admins) one **Department**. The left sidebar only shows the pages your account is allowed to use.

| Level | What they can do |
|---|---|
| **Admin** | Everything, in every department. Creates accounts and sets passwords. Approves change orders. |
| **Supervisor** | Full access to their own department, plus approvals (material usage, cash advances, vouchers, receipts). Sees their department's numbers on the Dashboard. |
| **Employee** | Day-to-day work in their own department: adds and edits records, submits requests for approval. |

Departments: **HR & Payroll**, **Inventory**, **Subcontractors**, **Receipts**.

Site workers do not use the app. They only use the fingerprint scanner.

# 2. Installing the app *(coming soon)*

A normal installer (Setup.exe) will be provided at the end of the project. The first time the app opens it asks for the server address once.

# 3. Signing in and out

## First time on a computer: Database Settings

The first time the app opens on a computer (or if it can't find the database), the **Database Settings** window appears. Ask your Admin for the values.

1. **Server address:** `localhost` on the development PC; the office server address (e.g. `192.168.1.50`) in the office.
2. **Port:** leave `5432`.
3. **Database name:** `construction_db`.
4. **Database username / password:** given by your Admin.
5. Click **Test Connection**. When it says *Connected.*, click **Save**.

The settings are saved on that computer only. To change them later, click **Database settings** on the Sign In window.

## Signing in

1. Type your **username** (capital letters don't matter) and **password**.
2. Tick **Show password** if you want to check what you typed.
3. Click **Sign In** (or press Enter).

If it doesn't work, the message under the password box tells you why:

| Message | Meaning |
|---|---|
| Wrong username or password. *N* attempts left... | Try again carefully. After 5 wrong tries the account locks. |
| This account is locked... | Ask an Admin to unlock it. |
| This account is turned off... | Your account was disabled. Ask an Admin. |

## Forgot your password?

**Employees and Supervisors**

1. On the Sign In window, click **Forgot password?**
2. Type your username and click **Next**.
3. The message says your request was sent to the Admins. An Admin will give you a new password in person. You can't change it yourself.

**Admins**

1. Click **Forgot password?**, type your username, click **Next**.
2. Answer your **security question** (capital letters don't matter).
3. Type a new password twice (at least 8 characters, with a letter and a number) and click **Reset Password**.

If you never set a security question, ask another Admin to set a new password for you.

## For Admins: handling password requests

1. In the main window, the **Password requests (N)** button shows how many people are waiting.
2. Click it, select the person, and click **Set New Password**.
3. Click **Generate** (or type one), then **Copy**, and click **Save**.
4. Give the new password to the person in person. If their account was locked, it is unlocked automatically.

Use **Dismiss** if a request was sent by mistake. Every change is recorded in the log.

## Signing out

Click **Sign Out** at the bottom of the left sidebar, or close the window. Both are recorded.

## The main window

- **Left sidebar:** the pages you're allowed to use. You only see your own department's page (Admins see everything). The page that's open is shown in **bold**.
- **Top:** the page name, your name, and your department and level. Admins also see **Password requests (N)**.
- **Middle:** the page you chose.

## My Account

Click **My Account** in the sidebar to see your name, username, department and level.

**Admins only:**

- **Change password:** type your current password, then the new one twice (at least 8 characters, with a letter and a number), and click **Save Password**.
- **Security question:** pick a question (or type your own), type the answer, type your current password to confirm, and click **Save Security Question**. You'll need this answer if you ever use *Forgot password?*. Capital letters don't matter.

**Set your security question as soon as you get an Admin account.** Without it, only another Admin can reset your password.

Employees and Supervisors can't change their password here; ask an Admin.

## For Admins: User Accounts

Click **User Accounts** in the sidebar.

**Make a new account**

First add the person in **HR & Payroll > Employees**. Only office staff get logins, never site workers.

1. Click **Create Account**.
2. Pick the **Employee** from the list. Only office staff without a login are listed.
3. A **username** is suggested (e.g. Juan Dela Cruz -> jdelacruz). Change it if you like: letters, numbers, dots, dashes or underscores, no spaces.
4. Type a first password, or click **Generate**.
5. Choose the **Level**. The **Department** fills in by itself from the employee (Admins have none).
6. Click **Save**. Give the username and password to the person in person.

The login's name and department follow the employee record. If HR renames someone or moves them to another department, their login updates too.

If the department already has an active person at that level (the plan is 1 Employee and 1 Supervisor per department), the app asks before saving.

**Change an account:** click it in the list, change the username or level (or which employee it's for), and click **Save**. To change the password, use **Set New Password**.

**Other buttons** (click the account first):

| Button | Does |
|---|---|
| Set New Password | Gives the person a new password (also unlocks it) |
| Unlock | For accounts locked after 5 wrong passwords |
| Disable | Turns the account off, e.g. when someone leaves. Their records are kept. |
| Enable | Turns a disabled account back on |

You can't disable your own account, and the last active Admin can't be disabled or changed to another level.

## "Still there?"

If you don't touch the keyboard or mouse for **5 minutes**, a *"Still there?"* message appears with a 60-second countdown.

- Click **Stay Signed In** to keep working.
- Click **Sign Out** to sign out now.
- If nobody answers, the app signs out by itself, and the Sign In window says why.

## Rules

- **One sign in** for the whole app.
- **5 wrong passwords** lock the account. Only an Admin can unlock it.
- **Forgot your password?** Click *Forgot Password* and type your username. The Admins get a request and will give you a new password in person. (Admins use their security question instead.)
- **Only Admins change passwords.** Employees and Supervisors can't change their own.
- **Idle check:** after 5 minutes without using the app, a *"Still there?"* message appears. If nobody answers within 60 seconds, the app signs out by itself.

# 4. Dashboard *(coming soon)*

# 5. Projects

**Admins only.** The company's one project list. Other departments pick projects from it.

**Add a project**

1. Click **Add Project**.
2. Fill in the **name**, **site / location**, **client** (optional), **sector**, **contract price**, **start date**, and the **target end date** (tick the box to set one).
3. Choose the **status**: Pending, Ongoing, Done or Cancelled.
4. Tick **Show on website** if it should appear on the public website. You add photos and details later in Website Admin.
5. Click **Save**.

**Change a project:** click it in the list, change it, then click **Save**. When you set a project to **Done**, today's date is recorded as its completion date.

**Summary** (worked out by the app): days so far (or how long it took), revenue, expenses to date, and profit to date. Expenses fill in as receipts, materials and subcontractor payments are recorded.

| Button | Does |
|---|---|
| Export | Saves the list (as filtered) to an Excel file |
| Print Report | A one-page report for the selected project. To make a PDF, choose "Microsoft Print to PDF". |
| Delete | Only for projects with nothing recorded yet. Otherwise set them to **Cancelled**. |

Use the **Search** box (name, site or client) and the **Status** and **Sector** filters to find projects.

# 6. HR & Payroll

For Admins and the HR department. The page has 7 tabs: Employees, Attendance, Payroll, Payslip History, Cash Advances, Settings and Edit History. **Settings** and **Edit History** are for Admins and the HR Supervisor. *(So far only Employees is built.)*

## Employees

Everyone who works for the company: **office staff** and **site workers**.

**Add an employee**

1. Click **Add Employee**.
2. Fill in the **full name**, contact number, address and position.
3. Choose the **department**. Choose **Site** for site workers. The pay schedule follows automatically:
   - **Site:** paid every Saturday
   - **everyone else:** paid the 15th and 30th
4. Type the **daily rate** and the date it **applies from** (usually today).
5. Type their **Biometric ID**, the number the fingerprint scanner gives them.
6. **Government deductions** are off by default. Tick the box only for people who should have tax, SSS, PhilHealth and Pag-IBIG deducted. The ID numbers are optional.
7. Click **Save**.

**Give someone a raise**

1. Click the employee.
2. Type the new **daily rate**, and set **Rate applies from** to the day it starts. It can be a future date.
3. Click **Save**.

Past payslips keep the old rate. The **Daily rate history** table lists every rate.

**When someone leaves:** click **Mark Inactive**. Their records stay, and if they have a login, it's turned off. **Reactivate** brings them back. An Admin then has to turn their login on again in User Accounts.

Use **Search** (name or Biometric ID), the **Status** filter (Active by default) and the **Pay schedule** filter. **Export** saves the list to Excel.

# 7. Inventory & Vendors *(coming soon)*

# 8. Subcontractors *(coming soon)*

# 9. Receipts *(coming soon)*

# 10. Website Admin *(coming soon)*
