# Decisions made while building Parts 1, 4 and 5 (Oct 27, 2026). Expects outline_insert.ps1 loaded and the outline open.
Insert-After "Who sees it: Admin sees all departments" @(
  "How money is counted (completed-contract method): a project's revenue and all its expenses count in the period it is finished (Done). Company overhead (overhead receipts, equipment repairs, payroll) counts by its own date. A department view shows only that department's expenses.")
Insert-After "Payouts: record date, amount, reference no. and method." @(
  "Vouchers are numbered SV-0001, SV-0002... A payout is the approved voucher's exact amount. The retention gets its own voucher once progress is 100% and every stage is paid.")
Insert-After "Approval: the Receipts Employee uploads" @(
  "Delivery receipts from Inventory are shown in the vault straight from the deliveries (marked ""Stock purchase"", read only) and never count as project expenses.")
Insert-After "Admin approves or rejects (it changes the project's price)" @(
  "Change orders are numbered CO-001, CO-002... across all projects. The revised contract price can't go below zero.")
