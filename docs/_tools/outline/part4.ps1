# Part 4: Subcontractors (agreed Oct 26, 2026). Expects outline_insert.ps1 loaded and the outline open.
Replace-Para "Contact details, specializations, and past project ratings" "Contact details and specializations. (Past project ratings: not for now.)"
Insert-After "Licenses & Accreditations" @(
  "Reminder when a PCAB license is about to expire.")
Insert-After "Record scope assigned" @(
  "Job assignment: subcontractor + project, scope of work, agreed contract amount, start and end dates, supporting documents (contract, scope papers).",
  "Progress: update the % completed with a date, notes and photos or documents.",
  "Payment by fixed stages set per job (e.g. Down payment 30%, Midway 40%, Completion 30%). The stages must add up to 100%.",
  "Retention: a % is held back from every payment (default 10%, set per job, can be 0). It is released right after the job is finished and checked.",
  "Billing: the Subcontractor Employee marks a stage as reached and prepares the billing voucher (PDF, using the subcontractor's bank details). The Subcontractor Supervisor approves it before payment.",
  "Payouts: record date, amount, reference no. and method. The app shows the remaining balance and the retention held. Payouts count as project expenses on the Dashboard.")
