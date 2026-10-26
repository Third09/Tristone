# Projects page details agreed while building (Oct 27, 2026). Expects outline_insert.ps1 loaded and the outline open.
Insert-After "Fields: Project Name" @(
  "Completion date is recorded automatically when the status is set to Done (cleared if reopened). Days shown: Day N while running, Finished in N days when done.")
