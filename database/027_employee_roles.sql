-- =====================================================================
-- Tri-Stone - employee roles instead of a typed position (owner request 2026-11-20)
-- employees.position now holds a role: Employee / Supervisor for office departments, Foreman / Laborer / Helper for
-- Site. The level of an Employee / Supervisor login follows it. This maps the positions typed so far:
--   office staff with an Employee / Supervisor login take their login's level;
--   other office staff: "...Supervisor", "...Manager", "...Head" -> Supervisor, everyone else -> Employee;
--   site workers: "...Foreman" -> Foreman, "...Helper" -> Helper, everyone else (Mason, Carpenter...) -> Laborer.
-- Run once. Safe to re-run (only positions that aren't a role yet are changed).
-- =====================================================================

UPDATE employees e
   SET position = u.level, updated_at = NOW()
  FROM users u
 WHERE u.employee_id = e.employee_id
   AND u.level IN ('Employee', 'Supervisor')
   AND e.department <> 'Site'
   AND e.position IS DISTINCT FROM u.level
   AND (e.position IS NULL OR e.position NOT IN ('Employee', 'Supervisor'));

UPDATE employees
   SET position = CASE WHEN position ~* '(supervisor|manager|head)' THEN 'Supervisor' ELSE 'Employee' END,
       updated_at = NOW()
 WHERE department <> 'Site'
   AND (position IS NULL OR position NOT IN ('Employee', 'Supervisor'));

UPDATE employees
   SET position = CASE WHEN position ~* 'foreman' THEN 'Foreman' WHEN position ~* 'helper' THEN 'Helper' ELSE 'Laborer' END,
       updated_at = NOW()
 WHERE department = 'Site'
   AND (position IS NULL OR position NOT IN ('Foreman', 'Laborer', 'Helper'));
