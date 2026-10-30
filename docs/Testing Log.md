# Testing Log

What was tested on which PC, and what still needs checking at home (the main PC, with the real data).

- **Work PC:** its own fresh database. Test data only; nothing here is real company data.
- **Home PC:** the main database. Anything marked "Check at home" gets ticked off there.

## Check at home

- [ ] **Row re-select after saving** (2026-10-27, `Responsive.SelectRow`). Passed on the work PC (see log); quick look at home: edit a project, click **Save**, the same project stays highlighted.
- [ ] **Materials: Add after viewing a material** (2026-10-27 fix). Click a material with category Cement, click **Add Material**, pick **Cement** again, fill the rest, **Save**: it saves (before the fix it said "Choose a category.").

- [ ] **Employees: Add / Edit** (2026-10-30). Open HR & Payroll: nobody is selected and the form is greyed out. **Add Employee** opens its own window. Click someone, change something, **Save**: a Yes/No message lists the change.
- [ ] **Main window** (2026-10-30). Fills the screen above the taskbar, no title bar or corner buttons; Sign Out still works.
- [ ] **All list pages: Add / Edit** (2026-10-30). On Projects, User Accounts, Materials, Vendors, Equipment, Subcontractors, Jobs and Change Orders: nothing is selected on opening; the Add/New/Create button opens a window; editing asks Yes/No with the list of changes.
- [ ] **Website Admin app** (2026-10-30). Run `databasesetup-database.cmd` (adds script 008), add your first Website Admin account in pgAdmin (Developer Guide 10.2), set Company.WebsiteAdmin as startup project, F5: sign in, check the 6 tabs, Sign Out.

## Log

| Date | PC | What | Result | Notes |
|---|---|---|---|---|
| 2026-10-27 | Work | `Responsive.SelectRow` helper on a test table (200 rows) | 5/5 passed | Selects the right row, scrolls to it, works with a hidden first column, selects nothing when no match. No database on this PC yet, so the real pages were not clicked through. |
| 2026-10-27 | Work | Whole solution builds after the change | Passed | 0 warnings, 0 errors |
| 2026-10-27 | Work | Work PC database set up: PostgreSQL 18.6, `setup-database.cmd` | Passed | Scripts 001-007 applied, 28 tables. Admin `ITmorilla` added (own test password, not the home one). |
| 2026-10-27 | Work | Row re-select on the real pages: Projects (add 3, edit 1), Materials (add 2, edit 1), Employees (add 2, edit 1) | 11/11 passed | Test program clicked **Save** on the real pages. Test records `ZZTEST...` deleted afterwards (database back to 0 records). |
| 2026-10-27 | Work | Bug found during that test: Materials form | Fixed | **Add Material** cleared only the text of Category/Unit, so the old choice stayed selected underneath; picking it again didn't register ("Choose a category."). Now clears the choice itself. Retest passed. |
| 2026-10-30 | Work | Owner report: HR "Save" removes an employee | Bug confirmed | The list selected the first employee by itself, so the form was silently editing them; typing a new person and clicking Save **overwrote** that employee. The edit history shows it happened to the work-PC test employee "Third" (became "john"). |
| 2026-10-30 | Work | Fix: no auto-select, Add Employee pop-up, Yes/No confirmation listing changes, edit-only form; main window full screen without corner buttons | 20/20 passed | Opening/searching selects nobody; form locked until a click; pop-up adds a new person (count 3 -> 4, nobody overwritten); Save lists changes (SSS shown only as "changed"); No saves nothing, Yes saves; main window = screen working area, no title bar. Test records `ZZTEST...` deleted; john/shem untouched. |
| 2026-10-30 | Work | Same fix on the other 8 list pages (shared `RecordForm`): Add pop-up = same page in "add only" mode, no auto-select, locked form, Yes/No list of changes | 66/66 passed | Per page: Add opens the pop-up without the list, saves a new record that gets selected; reopening selects nobody and locks the form; clicking loads it; Save lists the change (TIN / bank account shown as "changed"); No saves nothing; Yes saves. First run found that the pop-ups' hidden filter dropdowns reloaded the (hidden) list and locked the form; fixed so the add-only copy never loads the list. Test records deleted; the owner's own project, material and accounts untouched. |
| 2026-10-30 | Work | Owner decision: Website Admin becomes a separate desktop app with its own accounts; removed from the office app | Passed | Admin sidebar now: Dashboard, Projects, HR & Payroll, Inventory, Subcontractors, Receipts, User Accounts, My Account, Sign Out. Build 0 warnings. |
| 2026-10-30 | Work | Website Admin app, step 1: script 008 (own accounts + sign-in log), new project Company.WebsiteAdmin, Sign In window, main window with Sign Out and 6 empty tabs | 16/16 passed | Unknown user, office login (ITmorilla) and disabled account refused; wrong passwords count down and lock at 5; locked refused with the right password; capital letters ignored; counter resets; main window full screen with the signed-in name and 6 tabs; Sign Out logged; PC name logged; office login_logs untouched. Test accounts zz_web / zz_off and their log rows deleted. |
