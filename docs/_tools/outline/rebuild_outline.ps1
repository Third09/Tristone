# Re-applies every agreed outline change (Oct 26, 2026) on top of the tracked-changes backup.
param([string]$Out = "$env:TEMP\claude_outline\rebuilt.docx")
Copy-Item "$PSScriptRoot\..\..\_archive\Project Outline - with tracked changes.docx" $Out -Force
. "$PSScriptRoot\outline_insert.ps1"
Open-Outline $Out
try {
  $script:doc.Revisions.AcceptAll()
  Replace-Para "Absence & Tardy Tracking:" "Absence & Tardy Tracking: Flags absences and late entries against the work schedule."
  Replace-Para "Automated gross pay calculation" "Automated gross pay calculation based on validated biometric logs (Daily Rate x days present, minus late minutes, plus approved overtime)."
  Replace-Para "One shared project list" "One shared project list used by Receipts, Inventory, Subcontractors and the Website Admin."
  Replace-Para "Manage user status" "Manage user status (Active, Locked, Disabled). Locked = 5 wrong passwords, only Admin unlocks. Disabled = turned off by Admin or employee marked Inactive."
  Replace-Para "Forgot Password does not use an email code" "Forgot Password does not use an email code (OTP) or security question for Employees/Supervisors. It only sends a request to the Admins. Admin sets a new password and gives it to the person directly (no temporary password, no forced change)."
  Replace-Para "Only Admin creates accounts" "Only Admin creates accounts and changes passwords. There is no self sign-up, and Employees/Supervisors cannot change their own password (they ask Admin)."
  Insert-After "Website Traffic Analytics" @(
    "Admin only. Everything on the website is editable from here: all text and images on every page (Home, Projects, Services, About Us, Contact).",
    "Website projects come from the Projects page (""Show on website""). Admin adds the website-only details here: description, scope of work, project size and photos.",
    "Per-project choice of which details show on the website (client name, location, size, timeline, cost, scope of work).",
    "Client Inquiries: every Contact form message is saved in the database and a copy is emailed to the company Gmail. Each inquiry has a status (New, Contacted, Closed) and a notes box, managed in the app.")
  Insert-After "Low-Stock Alert System" @(
    "Each material: name, category, unit (bag, piece, meter...), unit price, stock quantity and minimum stock.",
    "Using materials: the Inventory Employee records what was used and on which project (e.g. 20 bags of cement to Santos Residence). The Inventory Supervisor approves it; then the stock goes down and the cost is added to that project.",
    "Receiving deliveries: the Employee records what arrived from a vendor and the stock goes up. No approval needed (the purchase was already approved).",
    "Every delivery has its receipt attached: upload a scan or photo (image or PDF), or tick ""Physical copy only"" and enter the receipt number and where it is filed. The receipt is saved in the Receipts vault (Module 5) with category Materials and marked ""Stock purchase"", so its cost is not counted twice (material cost goes to a project when the material is used).",
    "Material history per project: everything a project has used and its total cost.")
  Insert-After "Equipment Master List" @(
    "Fields: name, type (tool, heavy equipment, vehicle), owned or rented (if rented: rental company, rate, return date), status (Available, Under Repair, Retired). Current location is not tracked.",
    "Maintenance & repair log: date, preventive or repair, what was done, cost, done by. Next maintenance date with a reminder when it is due.",
    "Repair and maintenance costs count as company-wide overhead (not per project).")
  Insert-After "Paper needed:" @(
    "Price comparison: the Employee enters quotes from several vendors for the same item; the Inventory Supervisor picks one, which becomes the approved purchase.")
  . "$PSScriptRoot\part4.ps1"
  . "$PSScriptRoot\part5.ps1"
  . "$PSScriptRoot\part1b.ps1"
  . "$PSScriptRoot\part2a.ps1"
  . "$PSScriptRoot\part0b.ps1"
  . "$PSScriptRoot\part_built.ps1"
  . "$PSScriptRoot\part_oct30.ps1"
  . "$PSScriptRoot\part_attendance.ps1"
  . "$PSScriptRoot\part_payroll.ps1"
  $i = 0
  foreach ($p in $script:doc.Paragraphs) { $i++; $t = $p.Range.Text.Trim()
    if ($t) { "{0,3} {1,-10} L{2} {3}" -f $i, $p.Style.NameLocal, $(if ($p.Range.ListFormat.ListType) { $p.Range.ListFormat.ListLevelNumber } else { "-" }), $t.Substring(0, [Math]::Min(60, $t.Length)) } }
} finally { Close-Outline }
