# Attendance, built 2026-10-30 from the scanner's sample export. Expects outline_insert.ps1 loaded and the outline open.
Insert-After "One shared fingerprint scanner." @(
  "The file to upload is the scanner's ""All Report"" (.xls), sheet ""Attendance Logs"". The scanner's ID for each person must be set as their Biometric ID; scans of unknown IDs are skipped and listed. Uploading the same file again adds nothing.")
Insert-After "Overtime: after 5:00 PM only" @(
  "Defaults chosen while building (can be changed): coming back from lunch after 1:00 PM also counts as late minutes; leaving before 5:00 PM counts as early minutes; overtime counts from 30 minutes after 5:00 PM; two scans within 3 minutes count once; Sunday scans are ""Rest Day Work"" waiting for overtime approval.",
  "A day with fewer than 4 scans is ""Missing Scan""; a work day with no scan is ""Absent"" (up to yesterday, from the employee's first rate date). HR fixes a day by hand with a required reason; overtime is approved by the HR Supervisor or an Admin. Holidays come with the Settings tab.")
