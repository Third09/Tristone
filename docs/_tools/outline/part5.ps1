# Part 5: Receipts (agreed Oct 26, 2026). Expects outline_insert.ps1 loaded and the outline open.
Insert-After "Uploaded By" @(
  "Type: Project (choose which project) or Company overhead (rent, electricity, internet, office supplies, equipment repairs, payroll). Project expenses count in that project's expenses and profit; overhead counts only in the company totals.",
  "Receipt copy: upload a scan or photo (image, PDF or Word), or tick ""Physical copy only"" and enter the receipt number and where it is filed. Material delivery receipts arrive here from Inventory marked ""Stock purchase"".")
Insert-After "Batch Print / Export" @(
  "Approval: the Receipts Employee uploads; the Receipts Supervisor approves before it counts as an expense.",
  "Expenses with no receipt: flagged ""No receipt - needs review"" and highlighted. A reason is required (e.g. tricycle fare, driver gives no receipt). They go to a ""Needs review"" list with a notification badge and do not count as an expense until the Receipts Supervisor approves. Attaching a receipt later clears the flag.",
  "Change Orders (client-requested changes)",
  "The Receipts Employee logs it: project, what the client wants changed, cost change (+/-), days change (+/-), and the signed client document (scan).",
  "Budget impact preview before approval: original vs. revised contract price and end date.",
  "Admin approves or rejects (it changes the project's price). Once approved, the project's revenue and end date update everywhere (Projects page, Dashboard).",
  "Each change order is numbered (CO-001, CO-002...) and kept in the history. A change order form (PDF) can be printed for the client to sign.") @(1, 1, 1, 2, 2, 2, 2)
Insert-After "Fields: Project Name" @(
  "Revenue = Contract Price + approved change orders (Module 5). Approved change orders also move the target end date.")
