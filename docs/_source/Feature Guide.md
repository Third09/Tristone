# Tri-Stone Office App: Feature Guide

This guide explains **how each feature works and why**: what happens behind each button, the rules the app follows, and what the messages mean. It is for the people who use the app every day.

The **User Manual** is the short, step-by-step version. Use this guide when you want to understand a feature, train someone new, or find out why the app said "no".

> Parts not built yet (Attendance, Payroll, Cash Advances, Website Admin) are listed at the end. This guide grows as they are finished.

# 1. The basics

## 1.1 Levels and departments

Every login has one **Level**. Supervisors and Employees also have one **Department**.

| Level | What it is for |
|---|---|
| **Admin** | Runs the whole system. Sees every page and every department. Creates logins, sets passwords, approves change orders. |
| **Supervisor** | Leads one department. Does everything an Employee does, plus **approves** the department's requests. Sees the Dashboard for their own department. |
| **Employee** | Does the day-to-day work in one department: adds records and sends requests for approval. |

Departments with their own page: **HR & Payroll**, **Inventory**, **Subcontractors**, **Receipts**.

The left sidebar only shows the pages your login may use, so different people see different sidebars. That's normal.

| Page | Admin | Supervisor | Employee |
|---|---|---|---|
| Dashboard | Yes (all or one department) | Yes, own department's numbers only | No |
| Projects, User Accounts, Website Admin | Yes | No | No |
| Own department's page | Yes, all four | Yes | Yes |
| My Account | Yes | Yes | Yes |

Even if someone found a way to open a page they shouldn't, the app checks again before saving anything, so the rule can't be bypassed.

## 1.2 Approvals: Waiting, Approved, Rejected

Many things need two people: the **Employee** records it, the **Supervisor** (or an Admin) approves it. Until it is approved, it **doesn't count**: stock doesn't move, and money isn't added to any total.

| Status | Meaning |
|---|---|
| **Waiting Approval** | Recorded, waiting for the Supervisor. Doesn't count yet. |
| **Approved** | Counts from now on (stock, expenses, revenue). Usually can't be edited any more. |
| **Rejected** | Doesn't count. Depending on the feature, it can be fixed and sent again, or a new one made. |

Employees see a short note instead of the Approve and Reject buttons. That's on purpose.

**Why two people?** It stops honest mistakes and makes every peso traceable to who recorded it and who approved it.

## 1.3 Edit History

Every add, edit and delete is recorded automatically: **who**, **when**, **what changed**, the **old value** and the **new value**. Each department has an **Edit History** tab (Supervisor and Admins only) showing its own changes. You can filter by date, person and record, and export to Excel.

Private details are **never** written in the history: passwords, security answers, government ID numbers, TIN and bank account numbers. For those, the history only says "on file: yes/no" or "(changed)".

## 1.4 Things that work the same everywhere

- **The main window always fills the screen** (above the taskbar) and has no title bar or minimize / maximize / close buttons, so it can't be resized, moved or closed by accident. Leave with **Sign Out**.
- **Search boxes** wait until you stop typing for a moment, then search. You don't need to press Enter.
- **Click a row** in a list to load it into the form next to it. After **Save**, the same row stays highlighted.
- **Add ...** buttons clear the form for a new record. **Cancel** throws away changes you haven't saved.
- **Export** saves the list, exactly as filtered, as an Excel file with real numbers and dates.
- **Print / Print Report / Print Voucher** opens a print preview. To make a PDF, choose the printer **Microsoft Print to PDF**.
- **Attaching files** (scans, photos, papers): PDF, pictures (JPG, PNG, BMP, TIF) or Word, up to **25 MB** each. The app keeps its own copy, so moving or deleting your original later doesn't matter.
- **Money** is shown in pesos, for example ₱1,250.00.
- **Delete** is only allowed while nothing else depends on the record. Once it has history, use the "keep history" option instead (Cancelled, Inactive, Retired, Disabled). That way old reports stay correct.

# 2. Signing in and staying safe

## 2.1 Signing in

Type your **username** and **password**, then **Sign In**. Capital letters in the username don't matter (`JDelaCruz` = `jdelacruz`); in the password they **do**.

Every sign-in, sign-out and failed attempt is recorded with the computer's name (Dashboard > Log History).

## 2.2 Wrong passwords and locking

- Each wrong password shows how many tries are left.
- On the **5th** wrong password in a row, the account is **Locked**.
- A locked account can't sign in, **even with the right password**, until an Admin clicks **Unlock** (or sets a new password, which also unlocks it).
- A correct sign-in resets the counter to zero.

**Why:** it stops someone from guessing passwords over and over.

## 2.3 Account statuses

| Status | Meaning | How it gets there | How it goes back |
|---|---|---|---|
| **Active** | Can sign in | Normal | |
| **Locked** | Can't sign in | 5 wrong passwords | Admin: **Unlock** or **Set New Password** |
| **Disabled** | Turned off | Admin: **Disable**, or HR marks the employee **Inactive** | Admin: **Enable** |

## 2.4 Password rules

Every new password, whoever sets it, must be:

- at least **8 characters**,
- with at least **one letter** and **one number**.

Examples: `bodega2026` is OK. `password` is refused (no number). `12345678` is refused (no letter). `abc123` is refused (too short).

## 2.5 Forgot password

**Employees and Supervisors** can't reset their own password. **Forgot password?** sends a **request** to the Admins:

- It works even if your account is locked.
- Sending it twice doesn't create two requests.
- An Admin sets a new password and gives it to you **in person**. That's on purpose: a new password should never travel by chat or email.

**Admins** answer their own **security question** instead, then type a new password.

- Capital letters and spaces at the ends of the answer don't matter (`Manila ` = `manila`).
- After **10** wrong answers in total, the security question stops working for that Admin; another Admin must reset the password.
- An Admin who never set a security question must ask another Admin.

## 2.6 "Still there?" (idle sign-out)

After **5 minutes** without touching the keyboard or mouse, a *"Still there?"* message appears with a **60-second** countdown.

- **Stay Signed In:** keep working.
- **Sign Out:** sign out now.
- **No answer:** the app signs out by itself. The log says *Idle Sign Out*, and the Sign In window tells you why.

**Why:** a computer left signed in is an open door to payroll, bank details and approvals.

# 3. User Accounts (Admins)

## 3.1 How logins are tied to employees

A login always belongs to **one office employee** from **HR & Payroll > Employees**.

- The login **takes the employee's name and department**. You don't type them.
- If HR renames the employee or moves them to another department, the login updates by itself.
- **Site workers never get logins.** They only use the fingerprint scanner.
- Someone in the **Administration** department can only get an **Admin** login (Admins have no department).
- **One login per employee.** The app and the database both refuse a second one.
- Only **Active** employees who don't have a login yet appear in the list.

So the order is always: **HR adds the employee first, then the Admin creates the login.**

## 3.2 Creating a login

1. Click **Create Account** and pick the **employee**.
2. A **username** is suggested: first letter of the first name + surname. For example, Juan Dela Cruz becomes `jdelacruz`. Surname parts like *de, dela, de los, san, sta* are kept. You can change it.
3. Type a **first password** or click **Generate** (see 3.3).
4. Choose the **Level**. The **Department** fills in from the employee.
5. Click **Save**, then give the username and password to the person **in person**.

**Username rules:** 3 to 50 characters; only letters, numbers, dots, dashes and underscores; no spaces. It must be unique, ignoring capital letters, so `Juan` and `juan` can't both exist.

**"Already has someone" question:** the plan is 1 Employee and 1 Supervisor per department. If the department already has an active person at that level, the app **asks** before saving. It doesn't block you, because a temporary replacement is sometimes needed.

## 3.3 Typing a password vs. Generate

**You can do either.** Both are checked against the same password rules (2.4).

- **Type one yourself:** fine when you want something easy to read out loud. It still needs 8+ characters with a letter and a number.
- **Generate:** the app makes a random 10-character password with 2 numbers. It leaves out look-alike characters (no `0`/`O`, no `1`/`l`/`I`), so it's easy to read out and type. It's harder to guess than one you'd make up.

The password box is only used **when creating** a login. For an existing login it's greyed out; use **Set New Password** instead (3.4).

**Tip:** Generate is the safer choice. Write the password on paper, hand it over, and have the person keep it private.

## 3.4 The buttons for an existing login

Click the login in the list first.

| Button | What it does | Notes |
|---|---|---|
| **Set New Password** | Gives the person a new password | Also unlocks a locked account, resets the wrong-password counter, and closes their waiting password requests |
| **Unlock** | Lets a locked account sign in again | Resets the wrong-password counter |
| **Disable** | Turns the account off | Records are kept. Use it when someone leaves. |
| **Enable** | Turns a disabled account back on | Resets the wrong-password counter |

You can also change the **username**, **level** or which employee it's for, then **Save**.

**Safety rules:**

- You can't disable your own account or remove your own Admin level, so you can't lock yourself out by accident.
- The **last active Admin** can't be disabled or demoted. There must always be someone who can manage accounts. Make another Admin first.
- An account linked to an employee can't be unlinked.
- If HR marks an employee **Inactive**, their login is turned **off**. If HR reactivates them, the login **stays off** until an Admin clicks **Enable**. That's on purpose: an Admin decides when access comes back.

## 3.5 Password requests

The **Password requests (N)** button at the top shows how many people are waiting. Open it, select the person, and click **Set New Password** (Generate, Copy, Save). **Dismiss** closes a request sent by mistake without changing anything.

# 4. My Account

Everyone can see their own name, username, department and level here.

**Admins only:**

- **Change password:** type the current password, then the new one twice. The new one must follow the password rules and be different from the current one.
- **Security question:** pick or type a question, type the answer, and confirm with your current password. The answer is stored scrambled (like a password); nobody can read it, not even other Admins.

Both ask for your **current password** first, so nobody can change them on a computer you left signed in.

**Employees and Supervisors** can't change their password here. Ask an Admin.

# 5. Dashboard

For Admins (all departments) and Supervisors (own department only).

## 5.1 How the money is counted

The Dashboard uses the **completed-contract method**, the usual method for construction companies. A project's revenue **and all its expenses** count in the period when the project is **finished** (set to Done), not while it's running.

**Example:** a project ran from March to June and was set to Done in June. On "This Month" in May it shows nothing. In June, all of its revenue and expenses appear at once.

Company **overhead** (overhead receipts, equipment repairs, and payroll once built) counts by its own date, because it isn't tied to a project.

| Card | How it's worked out |
|---|---|
| **Completed Projects** | Projects set to Done in the period |
| **Gross Revenue** | Their contract prices + approved change orders |
| **Operational Expenses** | Their expenses (approved material use + approved project receipts + subcontractor payouts) + overhead in the period |
| **Net Revenue** | Gross Revenue minus Operational Expenses |

The line under the cards shows where the expenses come from.

## 5.2 Department view

Supervisors always see their own department; Admins can choose one. A department view shows only that department's expenses:

| Department | Expenses shown |
|---|---|
| Inventory | Materials used + equipment repairs |
| Subcontractors | Payouts |
| Receipts | Project + overhead receipts |
| HR & Payroll | Payroll (once built) |

Revenue and Net Revenue are company-wide, so they're hidden in a department view.

## 5.3 The rest of the page

- **Project status chart:** how many projects are Pending, Ongoing, Done and Cancelled **right now** (not affected by the period).
- **Top performing projects:** projects active during the period, with revenue, expenses, profit and margin **so far**, highest profit first. Click a column title to sort.
- **Log history** (Admins only): sign-ins, sign-outs, failed attempts, locks and unlocks, password changes and requests.

# 6. Projects (Admins)

The company's **one** project list. Inventory, Subcontractors and Receipts choose projects from it in dropdowns; they can't add their own.

## 6.1 Fields and automatic values

- **Required:** name, site/location, sector. The price can't be negative, and the target end date can't be before the start date.
- **Completion date** is set automatically to **today** when the status becomes **Done**, and cleared if the project is reopened.
- **Days** in the summary: "Day 45" while running, "Starts in 10 days" if it hasn't started, "Finished in 120 days" when done, or "Cancelled".

## 6.2 The project's money

- **Revenue** = contract price + **approved** change orders.
- **Expenses to date** = **approved** material use + **approved** project receipts + subcontractor **payouts**.
- **Profit to date** = Revenue - Expenses.

Payroll and equipment repairs are **not** project expenses; they're company overhead. Stock purchases count when the material is **used** on a project, not when it's bought, so nothing is counted twice.

The Projects page, the project report and the Dashboard all use the same calculation, so they always agree.

## 6.3 Delete vs. Cancelled

A project can only be deleted while **nothing** is recorded against it (no material use, receipts, subcontractor jobs or change orders). After that, set it to **Cancelled**: it stays in history and reports, but drops out of active work.

**Show on website** sends the project to Website Admin, where its photos and website details will be added *(coming soon)*.

# 7. HR & Payroll: Employees

Everyone who works for the company: **office staff** and **site workers**.

## 7.0 Adding vs. changing an employee

Adding and changing are kept **completely separate**, so one can never be mistaken for the other:

- **Add Employee** opens its own **Add Employee** window. Saving there always creates a **new** person.
- The form under the list is **only for changing** the employee you clicked. Until you click someone, it's empty and greyed out.
- When you open the tab, search or filter, the list **never selects anyone by itself**.
- **Save** first shows a message listing every change, for example *Position: Mason -> Foreman* or *Daily rate: ₱600.00 -> ₱700.00 from 2 Nov 2026*. Nothing is saved until you click **Yes**. Government ID numbers are listed only as "changed", never shown.
- If nothing was changed, **Save** just says *Nothing changed.*

**Why:** before this, the form was quietly in "editing" mode for the first employee in the list. Typing a new person into it and clicking Save **overwrote** that employee instead of adding a new one.

## 7.1 Departments and pay schedule

The department decides the pay schedule automatically:

| Department | Pay schedule |
|---|---|
| **Site** (site workers) | Weekly, every Saturday |
| Administration, HR & Payroll, Inventory, Subcontractors, Receipts | Twice a month, the 15th and 30th |

## 7.2 Daily rate history

The daily rate is kept as a **history with start dates**, not overwritten. That way past payslips always keep the rate they were paid with.

**Example:** Juan earns ₱600. HR gives him ₱650 starting next Monday:

- Type **650** and set **Rate applies from** to next Monday, then **Save**.
- Until Sunday he's still at ₱600; from Monday, ₱650. Both rows are in the **Daily rate history**.
- If HR types ₱700 for the **same** start date later, that row is **corrected** instead of adding another.
- Editing other details without touching the rate adds no new row.

Rate limits: more than ₱0, at most ₱100,000 a day.

## 7.3 Biometric ID

The number the fingerprint scanner gives the person. Optional; up to 20 letters, numbers or dashes. **No two people can share one.** If you try, the message says who already has it. Attendance will match scans to employees by this number.

## 7.4 Leaving and coming back

- **Mark Inactive:** the person and all their records stay, and their login (if any) is turned **off**.
- **Reactivate:** the employee is active again, but the login **stays off** until an Admin clicks **Enable** in User Accounts.
- You can't mark yourself Inactive, or the last active Admin's employee record.

## 7.5 Government deductions

Off by default. Tick it only for people who should have tax, SSS, PhilHealth and Pag-IBIG deducted. The ID numbers are optional and are **never** shown in Edit History; only whether each is on file.

# 8. Inventory & Vendors

Employees record; the Inventory **Supervisor** (or an Admin) approves usage and purchases.

## 8.1 Stock is never typed in

After a material is added with its **starting stock**, the stock changes **only** by itself:

- **down** when material usage is **approved**,
- **up** when a **delivery** is recorded.

**Why:** if anyone could type the stock, nobody could trust it. This way every unit can be traced to a delivery or a project.

**Low stock:** a row turns **light red** below its minimum and **red** at zero. The tab title shows the count, for example *Materials (2 low)*.

## 8.2 Material usage

1. The Employee records what a project used. It waits.
2. The Supervisor approves or rejects it.
3. **On approval:** the stock drops, and the cost becomes that project's expense.

The cost is **quantity × the material's price at the moment of approval**. Example: 20 bags approved at ₱250 = ₱5,000 added to the project. Using more than what's in stock is refused.

## 8.3 Price comparison and purchases

1. The Employee enters **quotes** from several vendors for the same material.
2. The **cheapest open quote** is highlighted.
3. The Supervisor picks one with **Approve as Purchase**. The **other open quotes** for that material close automatically.

## 8.4 Deliveries

- No approval needed, because the purchase was already approved.
- Saving a delivery **adds the stock** and sets the material's price to the **delivered price**.
- Every delivery needs its receipt: either a **scan**, or **Physical copy only** plus where the paper is filed (for example "Folder B, Oct 2026"). The app refuses a delivery with neither.
- Delivery receipts automatically appear in **Receipts** as **Stock purchase**, read-only, so they're never counted twice.

## 8.5 Equipment

- Owned or rented. Rented needs the rental company and the rate.
- When the **next maintenance** date arrives, the row turns **yellow** and the tab says *Equipment (N due)*.
- **Add Log Entry** after maintenance or a repair; it can move the next maintenance date.
- Repair costs are **company overhead**, not project expenses.
- Equipment with history can't be deleted; set it to **Retired**.

## 8.6 Vendors

Supplier details, bank details, VAT status and papers (company profile, bank details, certificate of registration). Bank account numbers are kept out of Edit History. A vendor with history can't be deleted.

# 9. Subcontractors

Employees prepare; the Subcontractors **Supervisor** (or an Admin) approves vouchers.

## 9.1 Jobs, stages and retention

A **job** = subcontractor + project + scope + contract amount + dates + **retention %**.

- **Payment stages** split the contract (default: Down payment 30%, Midway 40%, Completion 30%). They must add up to exactly **100%**. The last stage absorbs any rounding centavos, so the stages always add up to the contract exactly.
- **Retention** (default 10%, can be 0) is held back from **every** payment until the job is finished and checked.

**Example:** a ₱100,000 job, 10% retention:

| Stage | Amount | Retention held | Paid now |
|---|---|---|---|
| Down payment 30% | ₱30,000 | ₱3,000 | ₱27,000 |
| Midway 40% | ₱40,000 | ₱4,000 | ₱36,000 |
| Completion 30% | ₱30,000 | ₱3,000 | ₱27,000 |
| **Retention release** | | | **₱10,000** |

Total paid: ₱100,000.

## 9.2 The billing steps

1. The Employee marks a stage **Reached**.
2. The Employee clicks **Prepare Voucher** (numbered SV-0001, SV-0002...). It waits.
3. The Supervisor **approves** or **rejects** it. A rejected voucher sends the stage back to "Reached", so a new one can be prepared.
4. After paying, the **payout** is recorded for exactly the voucher's amount. Bank transfers and checks need a reference number.

A voucher can't be prepared before its stage is reached, and a payout can't be recorded before approval or twice.

## 9.3 Locking

Once billing starts (a stage reached or a voucher made), the subcontractor, project, amount, retention and stages are **locked**. Only the scope and dates can still change. **Why:** money already paid must keep matching the job it was paid for.

## 9.4 Retention release

Only possible when the latest progress update says **100%** and **every stage is paid**. It gets its own voucher and approval, like a stage.

## 9.5 Other rules

- Payouts (including released retention) are the project's subcontractor expenses.
- **PCAB license:** the row turns **yellow** within 30 days of expiry and **light red** once expired.
- A subcontractor with jobs can't be deleted. A job can only be deleted while it has no progress, papers or billing.
- TIN and bank account numbers are kept out of Edit History.

# 10. Receipts & Change Orders

Employees add receipts; the Receipts **Supervisor** (or an Admin) approves them. Change orders are approved by an **Admin only**.

## 10.1 Project or overhead

Every receipt is one of two kinds:

- **Project expense:** belongs to one project and counts in that project's expenses.
- **Company overhead:** rent, electricity, internet, office supplies and so on. Counts as overhead on the Dashboard.

Only **approved** receipts count. Waiting or rejected ones can be fixed with **Edit**, which sends them back to Waiting. Approved ones are locked.

## 10.2 The copy of the receipt

Each receipt has exactly **one** kind of copy:

| Copy | When to use | Required |
|---|---|---|
| **Scan** | Normal case | The attached file |
| **Physical copy only** | Paper only, no scan | Where the paper is filed |
| **No receipt at all** | Tricycle fare, small cash payments | A reason. Goes to **Needs Review** |

## 10.3 Needs Review

Expenses with no receipt wait here until the Supervisor decides. The tab title shows how many, for example *Needs Review (2)*. If the receipt turns up later, **Attach Receipt** moves it to the vault and **keeps** the decision already made.

## 10.4 Stock purchases

Delivery receipts from Inventory appear in the vault marked **Stock purchase**. They're read-only and are **not** project expenses; the material's cost reaches the project when it's **used** (8.2). That's how double counting is avoided.

## 10.5 Change orders

When a client asks for a change that costs more or less, or takes more or fewer days:

1. The Receipts department logs it: project, date, description, **cost change** (+ or -) and **days change** (+ or -). It's numbered automatically (CO-001, CO-002...).
2. The **budget impact** preview shows the contract price and end date now and after.
3. Attach the client's **signed** document and **Submit**.
4. An **Admin** approves or rejects it.

**On approval:** the cost change is added to the project's **revenue**, and the days move its **target end date**, everywhere (Projects, report, Dashboard).

**Rules:** a change order needs a cost change or a days change (or both), and the revised contract price can't go below ₱0.

## 10.6 Reports

Expense & Receipt Report (per project), Weekly and Monthly Expense Summary. Reports include **approved** expenses only. Print, save as PDF, or export to Excel.

# 11. Coming soon

- **Attendance:** fingerprint scans uploaded from the USB scanner (4 scans a day: in, break out, break in, out).
- **Payroll:** pay worked out from validated attendance, late minutes, approved overtime and deductions; payslips printed or saved as PDF.
- **Cash Advances:** recorded by HR, approved by the HR Supervisor, deducted a fixed amount per payday.
- **Website Admin:** edit the public website, choose which projects show, and read Contact form messages.

# 12. Words used in this guide

| Word | Meaning |
|---|---|
| **Approve / Reject** | The Supervisor's (or Admin's) decision on a request. Only approved items count. |
| **Overhead** | Company costs not tied to one project (rent, electricity, repairs, payroll). |
| **Retention** | Part of each subcontractor payment held back until the job is finished and checked. |
| **Change order (CO)** | A client-approved change to a project's price and/or end date. |
| **Voucher (SV)** | The paper that authorizes a subcontractor payment. |
| **Completed-contract method** | Counting a project's money when it's finished, not while it runs. |
| **Edit History** | The automatic record of every change: who, when, old and new values. |
