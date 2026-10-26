# User Accounts linked to employees (Oct 27, 2026). Expects outline_insert.ps1 loaded and the outline open.
Insert-After "Only Admin creates accounts and changes passwords" @(
  "Each login belongs to one Active office employee (picked from HR & Payroll > Employees; never site workers). One login per employee. The login's name and department come from the employee record.")
