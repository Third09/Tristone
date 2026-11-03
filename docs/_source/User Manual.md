# Tri-Stone: User Manual

How to use Tri-Stone, screen by screen: the office app and the Website Admin screens. This manual grows as each screen is finished; sections marked *(coming soon)* are planned but not built yet.

# 1. Who can do what

Every account has one **Level** and (except Admins and Website Admins) one **Department**. The left sidebar only shows the pages your account is allowed to use.

| Level | What they can do |
|---|---|
| **Admin** | Can **look at** every page and department, but can't change department records (the Add, Save, Delete and Approve buttons are hidden). Handles accounts: creates them, sets passwords, unlocks and disables them. |
| **Supervisor** | Everything their Employee can do in their own department, plus **approving** (material usage, purchases, vouchers, receipts, cash advances, overtime, payroll) and **deleting**. Sees their department's numbers on the Dashboard. The **Receipts Supervisor** also manages Projects and approves change orders. |
| **Employee** | Day-to-day work in their own department: adds and edits records, submits requests for approval. Can't approve or delete. |
| **Website Admin** | Manages the public website in the Website Admin screens (Tri-Stone opens them at sign-in). No office pages. |

Departments: **HR & Payroll**, **Inventory**, **Subcontractors**, **Receipts**.

Site workers do not use the app. They only use the fingerprint scanner.

# 2. Installing the app *(coming soon)*

A normal installer (Setup.exe) will be provided at the end of the project. It installs **Tri-Stone**, the one program everyone opens (office staff and Website Admins alike), and puts a shortcut on the desktop. The first time it opens it asks for the server address once.

Until then, on the development PCs, the program is `Apps\Tri-Stone\TriStone.exe` inside the project folder.

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

1. Open **Tri-Stone** (`Apps\Tri-Stone\TriStone.exe`, or its desktop shortcut). It's the one program for everyone. The Sign In window shows the logo and a white box with the fields.
2. Type your **username** (capital letters don't matter) and **password**.
3. Tick **Show password** if you want to check what you typed. **Forgot Password?** is on the same line, at the right.
4. Click **Sign In** (or press Enter). The office app opens. (Website Admin logins open the Website Admin screens; Admins are asked which one, see chapter 10.)

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

- **Left sidebar:** the logo, then the pages you're allowed to use, each with a small icon. You only see your own department's page (Admins see everything). The page that's open is **orange**. **Sign Out** (red) is at the bottom.
- **Top:** the page name in big letters, your name, and your department and level. Admins also see **Password requests (N)**.
- **Middle:** the page you chose. Pages with several parts (HR & Payroll, Inventory, Subcontractors, Receipts) list their parts **in the sidebar, under the page's button**, when the page is open; the open part is light orange. (They used to be tabs across the top.)

**How to read the screens**

- **Orange buttons** do the main thing (Add, Save, Submit, Approve...). **Red text** buttons remove or refuse something (Delete, Reject). White buttons do everything else (Export, Print, Cancel...). A pale button can't be used right now (for example, nothing is selected yet).
- **Tables** have a border around them and lines between every cell; the gray header row names the columns, every other row is lightly shaded, and the gray area under the last row means the list ends there. An empty table says **No records to show**.
- The **Website Admin** screens use white cards on a light gray background, with colored message boxes: orange = unsaved changes, green = saved, red = something went wrong.

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
5. Choose the **Level** (Admin, Supervisor, Employee or Website Admin). The **Department** fills in by itself from the employee (Admins and Website Admins have none).
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

For the HR department (Admins can only look). The page has 7 tabs: Employees, Attendance, Payroll, Payslip History, Cash Advances, Settings and Edit History. **Settings** and **Edit History** are for the HR Supervisor (Admins can only look). 

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

## Attendance

The days of everyone with a **Biometric ID**, worked out from the fingerprint scanner.

**Upload the scanner file**

1. On the scanner, export the report to the USB stick and plug it into the PC.
2. Click **Upload Scanner File** and choose the **All Report** file from the USB stick.
3. The line next to the button says how many scans came in. Uploading the same file twice is safe: nothing is added twice.
4. If someone's scanner ID isn't set as a Biometric ID in **Employees**, their scans are skipped and **View Rejected Rows** says who. Set the ID in Employees, then upload the file again.

**Send the employee list to the scanner**

Instead of typing names on the scanner:

1. Make sure each person has a **Biometric ID** (a number) in Employees.
2. Click **Make Scanner Employee File** and save it to the USB stick (it's called *Employee Form.xls*).
3. On the scanner, open the USB menu and upload the user / employee information from the stick.
4. Register each person's **fingerprint** on the scanner itself (the file can't carry fingerprints).

The message after saving lists anyone left out (no Biometric ID yet, or an ID that isn't a number). Departments go to the scanner as numbers: Site = 1, Administration = 3, HR & Payroll = 4, Inventory = 5, Subcontractors = 6, Receipts = 7.

**Check the days**

- Pick the dates, and **Show** (All, Late, Absent, Missing Scan, Overtime Waiting Approval, Fixed by HR). **Search** finds a name.
- Rows in **light red** are Absent or Missing Scan. Rows in **light yellow** have overtime waiting.

**Fix a missing scan**

1. Select the day (or double-click it) and click **Fix Missing Scan**.
2. Tick each time that is known and set it, type the **reason**, and click **Save**. Check the list of changes, then click **Yes**.

**Overtime** (HR Supervisor and Admins): select a day with overtime waiting and click **Approve Overtime** or **Reject Overtime**. Only approved overtime is paid.

## Payroll

1. Choose the **pay group** (Office: 15th and 30th, or Site: weekly) and the **pay period**.
2. Read the line under it. If it lists **missing scans** or **overtime waiting**, fix them in **Attendance** first.
3. Click **Compute Payroll**. You can compute again as often as you like until it's finalized.
4. The HR Supervisor (or an Admin) checks the table and clicks **Finalize Payroll**. This takes the cash advances and locks the payroll; those attendance days can't change any more.
5. **Print Payslips** (one page each), **Save Payslips as PDF** (one file), or **Export** the table. Double-click a name to see that payslip.

## Payslip History

Every payslip of a finalized payroll. Search a name or pick a payroll, select a payslip, then **View**, **Print** or **Save as PDF**.

## Cash Advances

1. Click **New Cash Advance**: the employee, the amount, how much to deduct each payday, the date and the reason. **Save**.
2. The HR Supervisor selects it and clicks **Approve** (or **Reject**).
3. From then on, every payday takes that amount until it's paid off. If a payslip is too small, the rest waits for the next payday. Select an advance to see the deductions taken so far.

## Settings (HR Supervisor; Admins can only look)

- **Work schedule:** start and end, break, grace minutes (15 = on time until 8:15), work days, overtime per hour and from how many minutes.
- **Government deductions:** the employee-share rates and limits for SSS, PhilHealth and Pag-IBIG, and income tax on or off. They apply only to employees whose **Government deductions** box is ticked in Employees. The defaults are the 2025 rates: check them with your accountant.
- Click **Save Settings**, check the list of changes, click **Yes**. Attendance is worked out again (days already paid don't change).
- **Holidays:** pick the date, type the name, choose **Regular** (paid even when not worked) or **Special** (no work, no pay), click **Add Holiday**. Nobody is marked absent on a holiday, and a payday on a holiday moves to the day before.

**Export** saves the list to Excel.

# 7. Inventory & Vendors

For the Inventory department (Admins can only look). Approve and Delete buttons are for the **Inventory Supervisor**. Tabs: Materials, Material Usage, Deliveries, Equipment, Vendors, Price Comparison, Edit History (Supervisor and Admins).

## Materials

The list of construction materials. Rows turn **light red** when stock is below the minimum and **red** when it runs out; the sidebar item shows how many are low, e.g. *Materials (2 low)*.

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

Tools, heavy equipment and vehicles, owned or rented (rented needs the company and rate). Set the **next maintenance** date; when it arrives the row turns **yellow** and the sidebar item says *Equipment (N due)*. Use **Add Log Entry** after maintenance or a repair; it can set the next date. Retired equipment keeps its history.

## Vendors

Supplier details, bank details and VAT status. After saving a vendor, upload their papers (company profile, bank details, certificate of registration) and open them with **View**.


# 8. Subcontractors

For the Subcontractors department (Admins can only look). Approve and Delete buttons are for the **Subcontractors Supervisor**. Tabs: Directory, Jobs & Progress, Billing & Payouts, Edit History (Supervisor and Admins).

## Directory

Your subcontractors: company details, two contact people, addresses, bank details (printed on vouchers), trade categories and PCAB license.

- **Add Subcontractor** opens a window: fill in the form, tick at least one trade, click **Save**.
- PCAB reminder: a row turns **yellow** when the license expires within 30 days and **light red** once it has expired. The sidebar item says *Directory (N PCAB expiring)*.
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

For the Receipts department (Admins can only look). Approve and Delete buttons, including change order approval, are for the **Receipts Supervisor**. Tabs: Receipt Vault, Needs Review, Change Orders, Reports, Edit History (Supervisor and Admins).

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

Expenses with no receipt (its sidebar item shows how many are waiting, e.g. *Needs Review (2)*). The Supervisor approves or rejects them. If the receipt turns up later, select the expense and click **Attach Receipt**: type its number and attach the scan (or paper location). It moves to the vault and keeps its approval.

## Change Orders

1. Click **New Change Order** (a window opens). Choose the project, the date the client asked, describe the change, and type the **cost change** and/or **days change** (use minus for less).
2. The **budget impact** shows the contract price and end date now and after.
3. **Attach Signed Document** (the client's signed paper), then **Submit**. It waits for an Admin.
4. An Admin selects it and clicks **Approve** (or **Reject**). Once approved, the project's revenue and target end date change everywhere.

**Print Form** gives the change order form for signing (choose "Microsoft Print to PDF" for a PDF). Numbers are given automatically: CO-001, CO-002...

## Reports

Choose the report (**Expense & Receipt Report**, **Weekly** or **Monthly Expense Summary**), the project (or All / Company Overhead) and the dates. Then **Preview**, **Print**, **Save as PDF** or **Export Excel**. Reports include approved expenses only.


# 10. Website Admin

The public website is managed in the **Website Admin** screens, separate from the office pages. Everyone signs in the same way, in **Tri-Stone**:

1. Open **Tri-Stone** (`Apps\Tri-Stone\TriStone.exe`, or its desktop shortcut).
2. Type your username and password and click **Sign In**.
3. What opens depends on your login:
   - **Website Admin** login: the Website Admin screens.
   - **Admin**: a small window asks **Office App** or **Website Admin** (or **Sign Out**).
   - Everyone else: the office app.

**Website Admin logins** are made by an Admin in **User Accounts**, like any other login: pick the employee and choose the Level **Website Admin** (no department). Same rules as all logins: 5 wrong passwords lock it, and Forgot Password goes to the Admins.

The Website Admin window looks like the office app: click a button in the **left sidebar** to open a page (the open one is in **bold**): **Home**, **Projects**, **Services**, **About Us**, **Contact Us**. **Log out** is at the bottom. After 5 minutes without activity, the same "Still there?" message appears.

**Save Changes and Publish Website**

- **Save Changes** (on each page) keeps your edits. The live website does **not** change yet.
- **Publish Website** (top right) puts **everything saved** on the live website. The website shows it in **about a minute**. Under it you see when it was last published and by whom.
- The first time on a PC, Publish opens **Website Settings**: paste the GitHub token, click **Test**, then **Save**. (See "Making the GitHub token" below.)
- Leaving a page with unsaved edits asks first. **Discard Changes** brings back the saved version.
- Pictures must be **JPEG or PNG**, **8 MB** at most.

**Home**

1. Click the dashed box (or drag a picture onto it) for the big top picture. **Remove Picture** clears it. The **Live Preview** on the right shows how the top of the website will look.
2. Type the **Main headline** (up to 150 characters) and the **Subheadline** (up to 300). The counter shows how many you used.
3. Click **Save Changes**, check the list of changes and click **Yes**. Then **Publish Website** when ready.

**Projects**

The projects come from the office **Projects** page: an Admin or the Receipts Supervisor ticks **Show on website** there first.

1. **Add New Project**: pick the project, type a short description (up to 100 characters), click **Add**.
2. Click a project in the list to edit it. Change the description and click **Save Changes**.
3. Click the dashed **Project Images** box (or drop pictures on it) to add up to 10 photos; you can pick several at once. A bar shows the upload; any picture that couldn't be uploaded is listed with the reason.
4. Click a photo to see it. **Up** / **Down** set the order; **Add to Showcase** / **Remove from Showcase** choose which photos the website carousel shows; **Remove** deletes the photo.
5. **Move Up** / **Move Down** set the order of the projects on the website. **Unpublish** hides a project (Publish shows it again). **Delete** removes it from the website only.

**Services**

1. **Add New Service**: click the picture box (or drop a picture on it), choose the service (Residential Construction, Commercial Construction or Repairs and Home Extension) and the category, type the location and a short description, click **Add Service**. The **Live Preview** shows the card as the website will.
2. Click a service to edit it, then **Save Changes**. **Move Up** / **Move Down**, **Unpublish** and **Delete** work like on Projects.

**About Us**

1. Click a section card at the top (for example **ABOUT TRI-STONE · Section 1**).
2. Change the picture, the heading and the text. Tick **Two parts** to give the section a second heading and text (like **OUR VISION** and **OUR MISSION**).
3. **Save Changes**.
4. **Add Section** opens a small window: add a picture, a heading and the text, then click **Add Section**. New sections go last.
5. **Move Up** / **Move Down** change the order on the website. **Delete Section** removes the open one (the last section can't be deleted).

**Contact Us**

The top shows **Total inquiries**, **Pending (New)** and **Website visits this month**.

- **Inquiries are forwarded to**: the email that receives the website's inquiries. Change it and click **Save Email**.
- **Company details**: the address, phone, email and footer line shown on the website. Click **Save Details**, then **Publish Website**.
- New inquiries from the website are added when you open Contact Us or click **Refresh** (the line under the list says how many). This needs the read key in **Website Settings** once per PC (see "Connecting the inquiries" below).
- The list shows the inquiries from the website's Contact form (new ones in **bold**). Search, pick a **Status** or tick **From** / **To** dates; **Reset Filter** clears them; **Export to Excel** saves the list.
- **Double-click** an inquiry (or click it and **Open Inquiry**) to read it. **Copy Email**, then paste it in Gmail to reply. Set the **Status** (New, Contacted, Closed), write **Notes** (only for you and the other Website Admins), click **Save**, check the changes and click **Yes**.

Inquiries are never deleted; close them instead.

**Making the GitHub token (once per PC that publishes)**

1. Sign in at **github.com** with the account that owns the website (Third09).
2. Click your picture (top right) > **Settings** > **Developer settings** (bottom left) > **Personal access tokens** > **Fine-grained tokens** > **Generate new token**.
3. Name it "Tri-Stone Publish". Pick an **Expiration** (when it ends, make a new one).
4. **Repository access**: **Only select repositories** > **tristone_website**.
5. **Permissions** > **Repository permissions** > **Contents**: **Read and write**.
6. Click **Generate token** and copy it (GitHub shows it only once).
7. In Website Admin, click **Website Settings**, paste it in **GitHub token**, click **Test**, then **Save**.

Never send the token in chat or email. If it leaks, delete it on GitHub and make a new one.

**Connecting the inquiries (once)**

1. Open **script.google.com** and click the website's script (**Untitled project**).
2. Select all the code and replace it with the code in `docs\_tools\google-script\Code.gs`. Click **Save** (disk icon).
3. At the top, pick **setup** in the function list and click **Run**. Allow the permissions it asks for (your own Google account).
4. The **Execution log** shows the **Read key**. Copy it.
5. Click **Deploy** > **Manage deployments** > the pencil (**Edit**) > **Version: New version** > **Deploy**. The web address stays the same.
6. In Website Admin, click **Website Settings**, paste the key in **Read key**, click **Test Inquiries**, then **Save**.

Each PC that uses Website Admin needs step 6 once. Keep the read key private.
