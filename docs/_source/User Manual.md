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

## Screen size

The app always fills the whole screen (above the taskbar). It has no title bar or minimize / maximize / close buttons, so it can't be resized or closed by accident. To leave, click **Sign Out**. On big monitors, lists grow to show more rows; on small laptop screens, scroll down to see the rest of a page. Search boxes update the list when you stop typing.

## Adding and changing records (every list page)

- **Add / New / Create** buttons open a **separate window** with the form. Saving there always makes a **new** record, then the window closes and the new record is selected in the list.
- The form under the list is **only for changing** the record you clicked. Until you click one, it's empty and greyed out. The list never selects anything by itself.
- **Save** first shows a message listing every change, for example *Unit price: 250 -> 275*. Click **Yes** to save or **No** to go back. Private numbers (government IDs, TIN, bank account numbers) are shown only as "changed".
- **Cancel** puts back what's saved. In an Add window, **Cancel** closes it without saving.

## Signing out

Click **Sign Out** at the bottom of the left sidebar. It is recorded, and the Sign In window comes back. To close the app completely, close the Sign In window.

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

1. Click **Create Account**. The **Create Account** window opens.
2. Pick the **Employee** from the list. Only office staff without a login are listed.
3. A **username** is suggested (e.g. Juan Dela Cruz -> jdelacruz). Change it if you like: letters, numbers, dots, dashes or underscores, no spaces.
4. Type a first password, or click **Generate**.
5. Choose the **Level**. The **Department** fills in by itself from the employee (Admins have none).
6. Click **Save**. The window closes and the new account is selected. Give the username and password to the person in person.

The login's name and department follow the employee record. If HR renames someone or moves them to another department, their login updates too.

If the department already has an active person at that level (the plan is 1 Employee and 1 Supervisor per department), the app asks before saving.

**Change an account:** click it in the list, change the username or level (or which employee it's for), click **Save**, check the changes in the message and click **Yes**. To change the password, use **Set New Password**.

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

# 4. Dashboard

For Admins and Supervisors (Employees don't have it). It opens first after signing in.

- **Period:** This Month, This Quarter, This Year, All Time, or Custom (then pick From and To). Click **Refresh** after changing it.
- **Department** (Admins only): All, or one department. Supervisors always see their own department.

**The four cards**

- **Completed Projects:** projects set to Done in the period.
- **Gross Revenue:** contract price + approved change orders of those finished projects.
- **Operational Expenses:** those projects' expenses (materials used, project receipts, subcontractor payouts) plus company overhead in the period (overhead receipts, equipment repairs; payroll once it is built). The line under the cards shows the breakdown.
- **Net Revenue:** Gross Revenue minus Operational Expenses.

A project's money counts when it is finished, so a period with no finished projects shows no revenue. A department view shows only that department's expenses (Revenue and Net are hidden).

**Project status chart:** how many projects are Done, Ongoing, Cancelled and Pending right now.

**Top performing projects:** projects active in the period with revenue, expenses, profit and margin so far, highest profit first. Click a column title to sort; **Export** saves it to Excel.

**Log history** (Admins only): sign-ins, sign-outs, failed attempts, account locks, password changes and password requests in the period. Filter by activity or search a name or username; **Export** saves it to Excel.


# 5. Projects

**Admins only.** The company's one project list. Other departments pick projects from it.

**Add a project**

1. Click **Add Project**. The **Add Project** window opens.
2. Fill in the **name**, **site / location**, **client** (optional), **sector**, **contract price**, **start date**, and the **target end date** (tick the box to set one).
3. Choose the **status**: Pending, Ongoing, Done or Cancelled.
4. Tick **Show on website** if it should appear on the public website. You add photos and details later in the Website Admin app.
5. Click **Save**.

**Change a project:** click it in the list, change it, click **Save**, check the changes in the message and click **Yes**. When you set a project to **Done**, today's date is recorded as its completion date.

**Summary** (worked out by the app): days so far (or how long it took), revenue, expenses to date, and profit to date. Expenses fill in as receipts, materials and subcontractor payments are recorded.

| Button | Does |
|---|---|
| Export | Saves the list (as filtered) to an Excel file |
| Print Report | A one-page report for the selected project. To make a PDF, choose "Microsoft Print to PDF". |
| Delete | Only for projects with nothing recorded yet. Otherwise set them to **Cancelled**. |

Use the **Search** box (name, site or client) and the **Status** and **Sector** filters to find projects.

# 6. HR & Payroll

For Admins and the HR department. The page has 7 tabs: Employees, Attendance, Payroll, Payslip History, Cash Advances, Settings and Edit History. **Settings** and **Edit History** are for Admins and the HR Supervisor. *(So far Employees and Edit History are built.)*

## Employees

Everyone who works for the company: **office staff** and **site workers**.

**Add an employee**

1. Click **Add Employee**. A separate **Add Employee** window opens.
2. Fill in the **full name**, contact number, address and position.
3. Choose the **department**. Choose **Site** for site workers. The pay schedule follows automatically:
   - **Site:** paid every Saturday
   - **everyone else:** paid the 15th and 30th
4. Type the **daily rate** and the date it **applies from** (usually today).
5. Type their **Biometric ID**, the number the fingerprint scanner gives them.
6. **Government deductions** are off by default. Tick the box only for people who should have tax, SSS, PhilHealth and Pag-IBIG deducted. The ID numbers are optional.
7. Click **Save**. The window closes and the new employee is selected in the list.

**Change an employee's details**

The form under the list is only for **changing** someone. It stays empty and greyed out until you click an employee.

1. Click the employee in the list.
2. Change what you need.
3. Click **Save**. A message lists every change (for example *Position: Mason -> Foreman*). Click **Yes** to save, or **No** to go back.
4. **Cancel** puts back what's saved.

**Give someone a raise**

1. Click the employee.
2. Type the new **daily rate**, and set **Rate applies from** to the day it starts. It can be a future date.
3. Click **Save**, check the change in the message, and click **Yes**.

Past payslips keep the old rate. The **Daily rate history** table lists every rate.

**When someone leaves:** click **Mark Inactive**. Their records stay, and if they have a login, it's turned off. **Reactivate** brings them back. An Admin then has to turn their login on again in User Accounts.

Use **Search** (name or Biometric ID), the **Status** filter (Active by default) and the **Pay schedule** filter. **Export** saves the list to Excel.

# 7. Inventory & Vendors

For Admins and the Inventory department. Approve buttons are for the **Inventory Supervisor** and Admins. Tabs: Materials, Material Usage, Deliveries, Equipment, Vendors, Price Comparison, Edit History (Supervisor and Admins).

## Materials

The list of construction materials. Rows turn **light red** when stock is below the minimum and **red** when it runs out; the tab title shows how many are low, e.g. *Materials (2 low)*.

- **Add Material** opens a window: name, category, unit, price, starting stock, minimum stock. Click **Save**.
- After that, the stock changes only by itself: down when usage is approved, up when a delivery is recorded.

## Material Usage

1. Choose the **project** and the **material**, type the **quantity** and date, and click **Submit**. It waits for approval.
2. The Supervisor selects it in the list and clicks **Approve** (or **Reject**). Approving lowers the stock and adds the cost to the project's expenses.

At the bottom, pick a project to see all the material it has used and the total cost.

## Price Comparison

1. Choose the **material to buy**.
2. Add a **quote** from each vendor (product, price, quantity). The cheapest open quote is highlighted.
3. The Supervisor selects one and clicks **Approve as Purchase**. The other quotes for that material close.

## Deliveries

1. Pick the **approved purchase** (or "no approved purchase"); vendor, material and price fill in.
2. Type the quantity received and the **receipt number**.
3. **Attach Scan** of the receipt, or tick **Physical copy only** and type where the paper is filed.
4. Click **Save Delivery**. The stock goes up.

**Open Receipt Scan** shows a saved scan.

## Equipment

Tools, heavy equipment and vehicles, owned or rented (rented needs the company and rate). Set the **next maintenance** date; when it arrives the row turns **yellow** and the tab says *Equipment (N due)*. Use **Add Log Entry** after maintenance or a repair; it can set the next date. Retired equipment keeps its history.

## Vendors

Supplier details, bank details and VAT status. After saving a vendor, upload their papers (company profile, bank details, certificate of registration) and open them with **View**.


# 8. Subcontractors

For Admins and the Subcontractors department. Approve buttons are for the **Subcontractors Supervisor** and Admins. Tabs: Directory, Jobs & Progress, Billing & Payouts, Edit History (Supervisor and Admins).

## Directory

Your subcontractors: company details, two contact people, addresses, bank details (printed on vouchers), trade categories and PCAB license.

- **Add Subcontractor** opens a window: fill in the form, tick at least one trade, click **Save**.
- PCAB reminder: a row turns **yellow** when the license expires within 30 days and **light red** once it has expired. The tab title says *Directory (N PCAB expiring)*.
- A subcontractor with jobs can't be deleted.

## Jobs & Progress

1. Click **New Job** (a window opens). Choose the **subcontractor** and the **project**, type the **scope**, **contract amount** and dates.
2. Set the **retention %** (default 10, can be 0). It is held back from every payment.
3. Set the **payment stages** (default: Down payment 30%, Midway 40%, Completion 30%). Edit the names and %s in the table; **Add Stage** / **Remove Stage**. They must add up to **100%**.
4. Click **Save Job**. Then **Upload Document** for the contract or scope papers.

Once billing starts, only the scope and dates can change.

**Progress:** type the **% completed**, the date and notes, optionally **Attach Photo / File**, then **Add Update**. Double-click an update to open its file.

## Billing & Payouts

1. Choose the **job**. The top line shows the contract, paid so far, retention held and remaining balance.
2. Select a stage and click **Mark Stage Reached**, then **Prepare Voucher**.
3. The Supervisor selects it and clicks **Approve Voucher** (or **Reject Voucher**; the Employee can then prepare a new one).
4. **Print Voucher** shows it with the subcontractor's bank details (choose "Microsoft Print to PDF" for a PDF).
5. After paying, select the approved voucher, enter the date, method and reference number, and click **Record Payout**. The amount must match the voucher.

**Retention:** when the job is 100% done (progress update) and every stage is paid, click **Release Retention**. That voucher is approved and paid the same way.

Payouts count as the project's expenses on the Projects page and Dashboard.

# 9. Receipts & Change Orders

For Admins and the Receipts department. Approve buttons for receipts are for the **Receipts Supervisor** and Admins; change orders are approved by an **Admin** only. Tabs: Receipt Vault, Needs Review, Change Orders, Reports, Edit History (Supervisor and Admins).

## Receipt Vault

All receipts. Filter by project (or Company Overhead), dates, category, status, or search by receipt number / merchant. Material delivery receipts from Inventory also appear here marked **Stock purchase** (read only; they are counted when the material is used, not twice).

**Add a receipt:** click **Add Receipt**, then:

1. Receipt number, date, merchant, amount and category.
2. **Project expense** (choose the project) or **Company overhead** (rent, electricity, internet, office supplies...).
3. The copy: **Attach Scan** (image, PDF or Word), or tick **Physical copy only** and type where the paper is filed, or tick **No receipt at all** and type why.
4. Click **Save**. It waits for the Supervisor's approval.

**Approve:** the Supervisor selects one or more waiting receipts (Ctrl+click) and clicks **Approve** (or **Reject**). Only approved receipts count as expenses. A waiting or rejected receipt can be fixed with **Edit**; saving sends it back for approval.

**Open** (or double-click) shows the scan and details; **Zoom In / Out**, **Open Original** for PDF/Word, **Print** for a summary sheet. **Print Selected** prints a sheet for each selected receipt.

## Needs Review

Expenses with no receipt (the tab title shows how many are waiting, e.g. *Needs Review (2)*). The Supervisor approves or rejects them. If the receipt turns up later, select the expense and click **Attach Receipt**: type its number and attach the scan (or paper location). It moves to the vault and keeps its approval.

## Change Orders

1. Click **New Change Order** (a window opens). Choose the project, the date the client asked, describe the change, and type the **cost change** and/or **days change** (use minus for less).
2. The **budget impact** shows the contract price and end date now and after.
3. **Attach Signed Document** (the client's signed paper), then **Submit**. It waits for an Admin.
4. An Admin selects it and clicks **Approve** (or **Reject**). Once approved, the project's revenue and target end date change everywhere.

**Print Form** gives the change order form for signing (choose "Microsoft Print to PDF" for a PDF). Numbers are given automatically: CO-001, CO-002...

## Reports

Choose the report (**Expense & Receipt Report**, **Weekly** or **Monthly Expense Summary**), the project (or All / Company Overhead) and the dates. Then **Preview**, **Print**, **Save as PDF** or **Export Excel**. Reports include approved expenses only.


# 10. Website Admin (separate app)

The public website is managed in a **separate program**, Tri-Stone Website Admin, not in the office app. It has **its own accounts**: your office login doesn't work there.

**Signing in**

1. Open **Tri-Stone Website Admin**.
2. Type your Website Admin **username** and **password**, then click **Sign In**.
3. The rules are the same as the office app: capital letters in the username don't matter, and 5 wrong passwords lock the account.

Forgot your password? Ask another Website Admin to set a new one.

The main window fills the screen and shows who is signed in. Click **Sign Out** (top right) to leave. Its six tabs (Page Content, Announcements, Website Projects, Document Vault, Inquiries, Website Traffic) are being built one by one.
