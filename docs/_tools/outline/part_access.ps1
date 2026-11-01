# Access levels (owner decision Nov 1, 2026). Expects outline_insert.ps1 loaded and the outline open.
Insert-After "Only Admin creates accounts and changes passwords" @(
  "Access levels (owner decision Nov 1, 2026): Employees do their own department's work (add, edit, submit for approval); Supervisors also approve, reject and delete in their own department; Admins can look at every page and department but change nothing there, only accounts (User Accounts, passwords, unlocking). The Receipts Supervisor manages the Projects list (every change is logged in the Receipts Edit History) and approves change orders. The Website Admin screens are unchanged: Admins can still edit there.")
Insert-After "Admin approves or rejects (it changes the project's price)" @(
  "Changed Nov 1, 2026: the Receipts Supervisor approves or rejects change orders, not an Admin.")
