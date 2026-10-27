# Testing Log

What was tested on which PC, and what still needs checking at home (the main PC, with the real data).

- **Work PC:** its own fresh database. Test data only; nothing here is real company data.
- **Home PC:** the main database. Anything marked "Check at home" gets ticked off there.

## Check at home

- [ ] **Row re-select after saving** (2026-10-27, `Responsive.SelectRow`). Passed on the work PC (see log); quick look at home: edit a project, click **Save**, the same project stays highlighted.
- [ ] **Materials: Add after viewing a material** (2026-10-27 fix). Click a material with category Cement, click **Add Material**, pick **Cement** again, fill the rest, **Save**: it saves (before the fix it said "Choose a category.").

## Log

| Date | PC | What | Result | Notes |
|---|---|---|---|---|
| 2026-10-27 | Work | `Responsive.SelectRow` helper on a test table (200 rows) | 5/5 passed | Selects the right row, scrolls to it, works with a hidden first column, selects nothing when no match. No database on this PC yet, so the real pages were not clicked through. |
| 2026-10-27 | Work | Whole solution builds after the change | Passed | 0 warnings, 0 errors |
| 2026-10-27 | Work | Work PC database set up: PostgreSQL 18.6, `setup-database.cmd` | Passed | Scripts 001-007 applied, 28 tables. Admin `ITmorilla` added (own test password, not the home one). |
| 2026-10-27 | Work | Row re-select on the real pages: Projects (add 3, edit 1), Materials (add 2, edit 1), Employees (add 2, edit 1) | 11/11 passed | Test program clicked **Save** on the real pages. Test records `ZZTEST...` deleted afterwards (database back to 0 records). |
| 2026-10-27 | Work | Bug found during that test: Materials form | Fixed | **Add Material** cleared only the text of Category/Unit, so the old choice stayed selected underneath; picking it again didn't register ("Choose a category."). Now clears the choice itself. Retest passed. |
