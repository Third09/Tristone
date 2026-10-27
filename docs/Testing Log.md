# Testing Log

What was tested on which PC, and what still needs checking at home (the main PC, with the real data).

- **Work PC:** its own fresh database. Test data only; nothing here is real company data.
- **Home PC:** the main database. Anything marked "Check at home" gets ticked off there.

## Check at home

- [ ] **Row re-select after saving** (2026-10-27, `Responsive.SelectRow`). Open Projects, edit a project, click **Save**: the same project stays highlighted. Repeat once on Employees and on Materials.

## Log

| Date | PC | What | Result | Notes |
|---|---|---|---|---|
| 2026-10-27 | Work | `Responsive.SelectRow` helper on a test table (200 rows) | 5/5 passed | Selects the right row, scrolls to it, works with a hidden first column, selects nothing when no match. No database on this PC yet, so the real pages were not clicked through. |
| 2026-10-27 | Work | Whole solution builds after the change | Passed | 0 warnings, 0 errors |
| 2026-10-27 | Work | Work PC database set up: PostgreSQL 18.6, `setup-database.cmd` | Passed | Scripts 001-007 applied, 28 tables. Admin `ITmorilla` added (own test password, not the home one). |
