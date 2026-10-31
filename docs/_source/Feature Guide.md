# Tri-Stone: Feature Guide

A functional specification of every built feature: who may use it, what the inputs and validations are, the state changes it causes, the calculations it performs and what it writes to the database. Table and column names refer to the **Database Guide**; class and method names to the **Developer Guide**. The **User Manual** has the step-by-step instructions.

# 1. Cross-cutting rules

## 1.1 Access model

Each login (`users`) has a **Level** and, for Supervisor and Employee, one **Department** (HR & Payroll, Inventory, Subcontractors, Receipts). Admin and Website Admin have no department (database CHECK).

| Page | Admin | Supervisor | Employee | Website Admin |
|---|---|---|---|---|
| Dashboard | all departments or one | own department, forced | — | — |
| Projects, User Accounts | yes | — | — | — |
| HR & Payroll / Inventory / Subcontractors / Receipts | all four | own | own | — |
| HR > Settings, every Edit History tab | yes | own department | — | — |
| My Account | yes | yes | yes | — |
| Website Admin screens | yes (chosen at sign-in) | — | — | yes |

- The sidebar is built from `Access.CanSee`; services repeat the check (`Access.RequirePage`) before reading or writing, so hiding a button is never the only protection.
- **Approver** = Admin, or the Supervisor of that department (`Access.IsApprover`). Approve / Reject / Finalize buttons are hidden from Employees, who see a note instead; the service re-checks with `RequireApprover`. Exception: change orders are approved by Admins only.
- **Sign-in routing** (Tri-Stone): Website Admin → Website Admin app; Admin → choice of Office App / Website Admin / Sign Out; Supervisor / Employee → office app.

## 1.2 Approval states

| Workflow | States | Counts toward totals when |
|---|---|---|
| Material usage | Waiting → Approved / Rejected | Approved (stock drops, cost to project) |
| Quotes | Open → Selected / Not Selected | Selected creates a purchase |
| Subcontractor vouchers | Waiting → Approved / Rejected → Paid | Paid (the payout row is the expense) |
| Receipts | Waiting → Approved / Rejected | Approved |
| Change orders | Waiting → Approved / Rejected (Admin) | Approved (revenue, end date) |
| Overtime | None / Waiting → Approved / Rejected | Approved (paid by payroll) |
| Cash advances | Waiting → Active / Rejected; Active → Paid Off | Active (deducted by payroll) |
| Payroll runs | Draft → Final | Final (payslips issued, cash advances taken) |

Every decision re-reads the current state in the same save; deciding twice returns "This ... is already Approved/Rejected/...".

## 1.3 Audit trail

Every add, edit and delete inserts an `audit_logs` row in the same transaction: user, department (which Edit History tab shows it; NULL = Admin-only records), table, record id and display name, action, and JSON snapshots of the old and new values. Edit History flattens the snapshots into one row per changed field, filterable by date range, person and record name, exportable to Excel.

**Never logged:** passwords and their hashes, security answers, SSS / PhilHealth / Pag-IBIG numbers, TIN, bank account numbers. Snapshots store `*_on_file: true/false` instead.

## 1.4 List pages: add vs. edit

Applies to Projects, User Accounts, Employees, Materials, Vendors, Equipment, Subcontractors, Jobs, Change Orders, and in a simpler form to Attendance, Cash Advances, Payroll and Payslip History.

- **No automatic selection.** After every load, search or filter, and when a hidden tab is first shown, the grid has no selected row (`RecordForm.NoAutoSelect`). Only an explicit click selects a record.
- **Add** opens a pop-up containing only the form (`RecordForm.AddDialog`); saving there always inserts. The new record is then selected in the list.
- **The page form is edit-only** and disabled until a row is clicked (`RecordForm.SetEnabled`).
- **Save asks first:** a Yes/No message lists each changed field as `Label: old -> new` (private fields as "changed"). "No" saves nothing; no changes → "Nothing changed."
- After a save the list reloads and re-selects the saved record (`Responsive.SelectRow`).

## 1.5 Common behaviour

- **Search boxes** run 300 ms after typing stops; a slower earlier search never overwrites a newer result (`LatestOnly` tickets).
- **Export** writes the list as filtered to `.xlsx` with typed numbers and dates.
- **Printing** uses a print preview; PDFs are produced with the Windows printer "Microsoft Print to PDF" (reports and payslips can save straight to a PDF file).
- **Attachments:** PDF, JPG/JPEG, PNG, BMP, TIF/TIFF, DOC/DOCX, up to 25 MB. The file is copied into the storage folder (`<area>\yyyy\MM\<random>_<name>`), so later changes to the original don't matter. A stored file opens in its default Windows program.
- **Delete** is allowed only while no other record references the row; otherwise the message names the history-keeping alternative (Cancelled, Inactive, Retired, Disabled, Reject).
- **Main window:** borderless, fills the screen's working area, no minimize / maximize / close buttons; leaving is through Sign Out.

# 2. Sign-in and session

## 2.1 Sign In

| Condition | Result | Logged (`login_logs`) |
|---|---|---|
| Unknown username | "Wrong username or password." | Failed Login, "Unknown username", the typed name |
| Status Disabled | "This account is turned off. Please contact an Admin." | Failed Login, "Account disabled" |
| Status Locked | "This account is locked after too many wrong passwords..." (even with the right password) | Failed Login, "Account locked" |
| Wrong password | "Wrong username or password. n attempts left before the account locks." `failed_login_count + 1` | Failed Login, "Wrong password (n of 5)" |
| 5th wrong password | Status → Locked | Account Locked |
| Correct | Counter reset, `last_login_at` set, session started | Login (with PC name) |

Usernames are matched case-insensitively; passwords are case-sensitive (bcrypt).

## 2.2 Account status

| Status | Entered by | Left by |
|---|---|---|
| Active | creation, Unlock, Enable, Set New Password | — |
| Locked | 5 consecutive wrong passwords | Admin: Unlock or Set New Password (both reset the counter) |
| Disabled | Admin: Disable; HR: employee marked Inactive | Admin: Enable (Reactivate in HR does **not** re-enable) |

## 2.3 Passwords

- Rule (`AccountRules.CheckNewPassword`): at least 8 characters, at least one letter and one number; confirmation must match.
- **Generate:** 10 random characters including 2 digits, without look-alikes (0/O, 1/l/I).
- **Forgot Password, Employees and Supervisors:** creates one Waiting `password_requests` row (a second request while one is waiting is not duplicated); works while Locked. Admins handle it with Set New Password (hash replaced, account unlocked, request Done, "Password Changed" logged) or Dismiss.
- **Forgot Password, Admins:** security question; the answer is compared after trimming and lower-casing; after 10 wrong answers in total the route is blocked and another Admin must reset the password.
- **My Account (Admins only):** change own password and set the security question; both require the current password; the new password must differ from the current one.

## 2.4 Idle sign-out

After 5 minutes without keyboard or mouse input, "Still there?" appears with a 60-second countdown. Stay Signed In resets the timer; Sign Out or no answer signs out (logged as Logout / Idle Sign Out) and the Sign In window states the reason. Same behaviour in the Website Admin app.

# 3. User Accounts (Admin)

- **Source of identity:** a login is created from an Active office employee without a login (never Site). Name and department are copied from the employee and kept in sync when HR changes them. One login per employee (service check + partial unique index).
- **Level and department:** Admin / Website Admin → no department; Supervisor / Employee → the employee's department. Administration employees can only be Admin or Website Admin.
- **Username:** 3-50 characters, letters, digits, `.`, `-`, `_`; unique ignoring case. Suggested as first initial + surname, keeping surname particles (de, dela, de los, san, sta).
- **Same-role warning:** if the department already has an active login at that level, a confirmation is asked (not blocked).
- **Self-protection:** no disabling or demoting your own account; the last active Admin can't be disabled or demoted; a linked login can't be unlinked.
- Changes audited with department NULL (Admin-only history).

# 4. Dashboard

**Period:** This Month, This Quarter, This Year (each from its first day to today), All Time (from 2000-01-01), Custom.

**Completed-contract method:** a project contributes revenue and all its expenses in the period in which it was set to Done (`completed_date`); running projects contribute nothing to the cards.

| Card | Formula |
|---|---|
| Completed Projects | count of projects with status Done and `completed_date` in the period |
| Gross Revenue | Σ over those projects of (contract price + approved change orders) |
| Operational Expenses | Σ their expenses (approved material cost + approved project receipts + payouts) + approved overhead receipts dated in the period + equipment maintenance costs dated in the period |
| Net Revenue | Gross Revenue − Operational Expenses |

**Department view** (Supervisors always, forced in the service; Admins optional): Expenses = Inventory: materials + repairs; Subcontractors: payouts; Receipts: project + overhead receipts; HR & Payroll: 0 (payroll is not included in the Dashboard). Revenue and Net are hidden.

**Status chart:** current count per status (Done, Ongoing, Cancelled, Pending), independent of the period. **Top projects:** projects not Cancelled that were active during the period (started by its end, not finished before its start), with revenue, expenses, profit and margin to date; default order profit descending; click a header to sort. **Log History** (Admin): `login_logs` filtered by Login/Logout, Failed Login (incl. lock/unlock), Password Change, Password Request, and by name or username; newest 2,000.

# 5. Projects (Admin)

- **Validation:** name and site required; sector from the list; contract price ≥ 0; target end and completion date not before the start (also database CHECKs).
- **Automatic:** `completed_date` = today when status changes to Done, cleared when it changes away from Done.
- **Money (to date):** Revenue = contract price + approved change orders. Expenses = approved material usage cost + approved Project receipts + subcontractor payouts. Profit = Revenue − Expenses. Payroll and equipment repairs are never project expenses.
- **Days text:** "Day n" (running), "Starts in n days", "Finished in n days", "Cancelled".
- **Delete:** only when no material usage, sub job, receipt or change order refers to the project; otherwise set it to Cancelled.
- **Show on website:** marks the project for the Website Admin app.

# 6. HR & Payroll

## 6.1 Employees

- **Departments → pay schedule:** Site → Site (weekly, Saturday); Administration, HR & Payroll, Inventory, Subcontractors, Receipts → Office (15th and 30th). Enforced by a database CHECK.
- **Daily rate history:** each change with an "applies from" date adds an `employee_rates` row; the same date again corrects that row. Rate on a day = the latest row not after it. Limits: > ₱0 and ≤ ₱100,000.
- **Biometric ID:** optional, up to 20 characters, unique (the message names who has it).
- **Inactive:** the record stays; every linked login becomes Disabled. **Reactivate:** active again; logins stay Disabled until an Admin enables them. You can't mark yourself or the last active Admin's employee Inactive.
- **Government deductions switch:** off by default; turns on SSS, PhilHealth, Pag-IBIG and tax in payroll. ID numbers are optional and never logged.

## 6.2 Attendance: import

- **Input:** the scanner's "All Report" export (.xls; .xlsx and .csv in the same layout also accepted), sheet "Attendance Logs". A file without that sheet is refused with "This file has no "Attendance Logs" sheet...".
- **Parsing:** date range from the header (day/month/year; the first printed weekday resolves ambiguity); per person block: scanner ID, name, department, day-number row, weekday row, then every `hh:mm` under each day (several per cell allowed).
- **Matching:** scanner ID = `employees.biometric_id`, leading zeros ignored (`0102` = `102`). People with scans but no matching employee are skipped and listed under **View Rejected Rows** with the reason.
- **Idempotent:** a scan already stored for the same employee and minute is skipped; uploading the same or an overlapping file adds only new scans. The summary reads "Oct 1 - Oct 30, 2026: 312 scans imported, 5 already there, 1 row rejected".
- **Make Scanner Employee File:** writes the scanner's own Employee Form for every Active employee with a numeric Biometric ID (sorted by ID): ID, name, department number (Site 1, Administration 3, HR & Payroll 4, Inventory 5, Subcontractors 6, Receipts 7), authority 0; schedule columns empty. Employees without a usable ID are listed as not included.

## 6.3 Attendance: how a day is worked out

Uses HR Settings (defaults: 8:00-17:00, break 12:00-13:00, grace 15 min, work days Mon-Sat, overtime minimum 30 min).

1. Scans less than 3 minutes apart are merged (the first is kept).
2. Slots: 4 or more scans → first = in, last = out, break out = nearest to 12:00 among the middle scans, break in = nearest to 13:00 after it. Fewer than 4 → each scan goes to the slot whose usual time is closest, keeping their order. On a non-work day, 2-3 scans → first = in, last = out.
3. **Late** = (in − 8:00 if in > 8:15) + (break in − 13:00 if break in > 13:00). **Early** = 17:00 − out if out < 17:00. **Overtime** = out − 17:00 if ≥ 30 minutes, else 0.
4. **Status:** fewer than 4 times → Missing Scan; else Late if late > 0; else Present. Non-work day with scans → Rest Day Work (overtime = worked minutes: out − in − break).
5. A work day with no scans → Absent, but only before today, from the employee's first rate date, and not on a holiday.

| Example | Result |
|---|---|
| 7:58 / 12:01 / 12:59 / 17:02 | Present |
| 8:14 / 12:00 / 13:00 / 17:00 | Present (inside grace) |
| 8:20 / 12:00 / 13:10 / 18:45 | Late 30 min, overtime 105 min Waiting |
| 8:00 / 17:00 | Missing Scan |
| 8:00 / 12:00 / 13:00 / 16:30 | Present, early 30 min |
| Sunday 9:00 / 13:00 | Rest Day Work, 240 min Waiting |

**Recomputation** happens after every upload (file range) and after HR Settings or holidays change (last 120 days). Days HR fixed and days inside a finalized payroll period are never recomputed.

## 6.4 Attendance: fixing and overtime

- **Fix Missing Scan** (any HR user): known times ticked and set, reason required, times strictly in order (in < break out < break in < out); the day is worked out again from those times, marked Fixed (`fix_reason`, who, when), and audited with before/after times. Refused for days in a finalized payroll.
- **Overtime:** appears as Waiting; Approve / Reject by an approver; refused when not Waiting or when the day is in a finalized payroll. After a re-upload the decision stays unless the overtime minutes changed (then Waiting again).
- **Highlighting:** Absent / Missing Scan light red; overtime Waiting light yellow. Filters: All, Late (late or early minutes), Absent, Missing Scan, Overtime Waiting Approval, Fixed by HR.

## 6.5 Pay calendar

| Group | Payday | Period |
|---|---|---|
| Office | 15th | 26th of the previous month – 10th |
| Office | 30th; February: last day | 11th – 25th |
| Site | Saturday | previous Saturday – Friday |

A payday on a Sunday or holiday moves to the day before (repeatedly). Example: payday Sun 28 Feb 2027 → Sat 27 Feb.

## 6.6 Payroll computation

Per employee in the pay group (Active), per period:

| Component | Formula |
|---|---|
| Paid days | days with status Present or Late, or fixed by HR and not Absent / Rest Day Work |
| Basic pay | Σ daily rate on each paid day |
| Holiday pay | daily rate for each Regular holiday on a work day in the period, from the employee's first rate date, up to today, that the employee didn't work |
| Late deduction | Σ (late + early minutes) × daily rate of that day ÷ 480 |
| Overtime pay | approved overtime minutes ÷ 60 × overtime rate (₱100) |
| Gross pay | basic + holiday + overtime − late deduction (not below 0) |
| Government | only with the employee's switch on (6.7) |
| Cash advance | for each Active advance, oldest first: min(per payday, balance, pay left) |
| Net pay | gross − government − cash advance (≥ 0) |

**Worked example** (₱800/day, office, switch off): 11 paid days = ₱8,800; one Regular holiday = ₱800; 30 late minutes = −₱50; 2 h approved overtime = ₱200 → gross ₱9,750; cash advance ₱1,000 → net ₱8,750.

**Draft / Final:**
- Compute (any HR user) creates or rebuilds the Draft; can be repeated.
- Warnings: unfixed Missing Scan days and Waiting overtime **block** Finalize; "period isn't over yet" and work days with no attendance at all are warnings only.
- Finalize (approver, confirmation): recomputes, then in one transaction records the cash advance deductions, lowers balances (Paid Off at ₱0), and marks the run Final. A Final run can't be computed again, and its attendance days are locked.

## 6.7 Government deductions

For employees with the switch on, from a monthly basis = daily rate (on the period's last day) × work days per month (26). Share per payday: office ½, site 12/52.

| Deduction | Default formula (2025 employee shares; editable in Settings) |
|---|---|
| SSS | 5% × salary credit, where credit = basis rounded to the nearest ₱500, kept within ₱5,000-₱35,000 |
| PhilHealth | 2.5% × basis kept within ₱10,000-₱100,000 |
| Pag-IBIG | 2% × min(basis, ₱10,000) |
| Withholding tax | TRAIN yearly table on (period gross − the three above) × paydays per year (24 office, 52 site), divided back per payday |

TRAIN yearly table (2023 onward): ≤ ₱250,000: 0; ≤ ₱400,000: 15% over ₱250,000; ≤ ₱800,000: ₱22,500 + 20% over ₱400,000; ≤ ₱2,000,000: ₱102,500 + 25%; ≤ ₱8,000,000: ₱402,500 + 30%; above: ₱2,202,500 + 35%.

**Example** (₱1,000/day, office, gross ₱13,000): basis ₱26,000 → SSS ₱650, PhilHealth ₱325, Pag-IBIG ₱100; taxable ₱11,925 × 24 = ₱286,200 → yearly tax ₱5,430 → ₱226.25 per payday; net ₱11,698.75. Total government deductions are capped at the gross pay.

## 6.8 Cash advances

- **Record** (any HR user): Active employee, amount > 0, per payday > 0 and ≤ amount, date, reason required; saved Waiting with balance = amount.
- **Approve / Reject:** approver, only while Waiting.
- **Deduction:** only at payroll Finalize, oldest advance first, never more than the pay left; the rest carries over. Example: ₱5,000 at ₱2,000 per payday, pay left ₱1,500 → ₱1,500 taken, balance ₱3,500. Each deduction is listed with pay date, period and balance after; the payslip shows the balance left.

## 6.9 Payslips and Payslip History

One page per employee: period, pay date, daily rate, basic pay with days, holiday pay, overtime, late/undertime deduction, gross, government deductions (if on), cash advance with the balance left, net pay, received-by line. Print Payslips prints the whole run; Save as PDF writes one file. Payslip History lists only finalized payslips, filterable by payroll and name.

## 6.10 HR Settings (approver)

- Work start / end, break start / end, grace minutes, work days, overtime rate and minimum, work days per month, and all government rates and limits (6.7), with tax on/off.
- Validation: end after start; break inside the work hours; grace 0-120; at least one work day; rates 0-50%; maxima not below minima.
- Saving asks Yes/No with the changes, writes the `hr.*` settings, audits before/after, and recomputes the last 120 days of attendance (fixed and finalized days excepted).
- **Holidays:** one per date; Regular (paid when not worked) or Special (no work, no pay). Adding or deleting recomputes attendance. Effects: no Absent on a holiday; payday moves back; Regular holiday pay in payroll. Working on a holiday is paid as a normal day.

# 7. Inventory & Vendors

## 7.1 Materials and stock

- Create: unique name, category, unit, unit price ≥ 0, starting stock ≥ 0, minimum ≥ 0. After creation `stock_qty` can't be edited.
- Stock changes only through approved usage (−) and deliveries (+), each in the same save as its record.
- Low stock: stock < minimum → light red; stock = 0 → red; tab title "Materials (n low)".
- Delete refused once usages, quotes or deliveries exist.

## 7.2 Material usage

- Record: project (not Cancelled), material, quantity > 0 and ≤ current stock, date, notes → Waiting.
- Approve (approver): re-checks stock, `stock_qty −= quantity`, `cost = quantity × unit_price` at that moment (frozen), Approved. Example: 20 bags at ₱250 → ₱5,000 to the project, stock 100 → 80.
- Reject: no stock or money effect. Project history lists the approved usage and its total cost.

## 7.3 Price comparison, purchases and deliveries

- Quotes per material from several vendors; open quotes listed first, cheapest first (highlighted).
- Approve as Purchase (approver): the quote becomes Selected and a purchase is created (one per quote); the material's other Open quotes become Not Selected. Only Open quotes can be deleted.
- Delivery (no approval): optional purchase (fills vendor, material, price), quantity, price, receipt number, and either a scan or "physical copy only" with the filed location (database CHECK). Effects: stock += quantity; material price = delivered price; the purchase shows delivered quantity. Delivery receipts appear in the Receipt Vault as Stock purchase.

## 7.4 Equipment

- Owned or Rented (Rented requires company, rate and unit: Day / Week / Month); status Available, Under Repair, Retired.
- Maintenance due when `next_maintenance` ≤ today (light yellow; tab title "Equipment (n due)").
- Log entries (Preventive / Repair, date, work, cost, done by) may set the next maintenance date. Costs are overhead (Dashboard by date).
- Delete refused once logs exist (set Retired).

## 7.5 Vendors

Name (unique), contacts, address, bank name / account name / number, VAT or Non-VAT. Papers: Company Profile, Bank Details, Certificate of Registration, Other. Account number logged only as on file. Delete refused once quotes or deliveries exist.

# 8. Subcontractors

## 8.1 Directory

Business name (unique), trade name, DTI/SEC, TIN, primary and secondary contacts, addresses, bank details, at least one trade, PCAB number / classification / expiry. PCAB expired → light red; expiring within 30 days → light yellow; tab title counts them. TIN and account number logged only as on file. Delete refused once jobs exist.

## 8.2 Jobs

- Subcontractor, project (not Cancelled), scope, contract amount > 0, start ≤ end, retention 0-99% (default 10), stages (default 30 / 40 / 30) totalling exactly 100%.
- **Stage amount** = contract × % ÷ 100 rounded to centavos; the last stage takes the remainder so Σ stages = contract.
- **Lock:** once any stage is reached or a voucher exists (not Rejected), subcontractor, project, amount, retention and stages can't change; scope and dates still can.
- Progress updates: % 0-100, date not in the future, notes, optional file; the latest is the job's progress. Job status: Ongoing, Finished (100%), Paid in Full (payouts = contract).
- Delete only without progress, papers or billing.

## 8.3 Billing

| Step | Who | Rule | Effect |
|---|---|---|---|
| Mark Stage Reached | any | once | `reached_on` |
| Prepare Voucher | any | stage reached, no live voucher | SV-nnnn, gross = stage amount, retention = gross × %, payable = gross − retention, Waiting |
| Approve / Reject | approver | Waiting | Approved / Rejected (Rejected frees the stage for a new voucher) |
| Record Payout | any | Approved; amount = payable; reference required for Bank Transfer and Check | `sub_payouts` row, voucher Paid; project expense |
| Release Retention | any | latest progress 100% and every stage Paid; retention held > 0 | Retention voucher for the held amount, same approval and payout |

Example (₱100,000, 10%): stages pay ₱27,000 + ₱36,000 + ₱27,000, then the retention voucher pays ₱10,000; total ₱100,000. The voucher prints with the subcontractor's bank details.

# 9. Receipts & change orders

## 9.1 Receipts

- Fields: receipt number (unless no receipt), purchase date (not in the future), merchant, amount > 0, category, Project (with project) or Company overhead.
- Copy, exactly one: scan; physical copy only + filed location; or no receipt + reason (database CHECK).
- Waiting until an approver decides (one or several at once; all selected must be Waiting). Only Approved count: Project → that project's expenses; Overhead → Dashboard by date.
- Edit only while Waiting or Rejected; saving returns it to Waiting. Approved receipts are locked.

## 9.2 Needs Review

No-receipt expenses, Waiting first; the tab title shows the waiting count. **Attach Receipt** adds the receipt number and copy later, clears the no-receipt flag and keeps the existing decision.

## 9.3 Stock purchases

Deliveries are listed in the vault as read-only "Stock purchase" rows (Approved, category Materials). They are never project expenses (the material's cost reaches a project when its usage is approved) and are not stored in `receipts`.

## 9.4 Change orders

- Fields: project (not Cancelled), date requested, description, cost change (±), days change (±), optional signed document. At least one change must be non-zero; the revised price can't go below ₱0. Numbered CO-001, CO-002... across all projects.
- Budget preview: current price (contract + approved change orders) → revised; current target end → revised.
- Approve / Reject: Admin only, while Waiting; editing only while Waiting. On approval: revenue += cost change; `target_end_date` += days change (if set), audited on the project too.

## 9.5 Reports

Expense & Receipt Report (every approved receipt in the range, by project / overhead / all), Weekly and Monthly Expense Summary (project vs. overhead totals per week starting Monday, or per month). Approved receipts only, stock purchases excluded. Preview, print, PDF, Excel.

# 10. Website Admin

- Opened from Tri-Stone by Website Admin logins (and by Admins who choose it); same idle sign-out; sidebar: Page Content, Announcements, Website Projects, Document Vault, Inquiries, Website Traffic (only Inquiries is built).
- **Inquiries:** website Contact form messages (an email or a phone number is required by the database). Status New → Contacted → Closed and internal notes, saved with who and when after a Yes/No list of changes; never deleted; the sidebar shows "Inquiries (n new)"; Copy Email copies the address for replying from Gmail.

# 11. Glossary

| Term | Meaning in this system |
|---|---|
| Approver | Admin, or the Supervisor of the department concerned |
| Completed-contract method | A project's revenue and expenses are recognised in the period it is set to Done |
| Overhead | Costs not tied to a project: overhead receipts, equipment repairs, payroll |
| Retention | Percentage withheld from each subcontractor stage payment, released after completion |
| Paid day | An attendance day that payroll pays: Present, Late, or fixed by HR |
| Locked period | A pay period whose payroll is Final; its attendance can't change |
| Stock purchase | A delivery's receipt shown in the Receipt Vault; never a project expense |
| Edit History | The per-department view of `audit_logs` |
