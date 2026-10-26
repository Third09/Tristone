-- =====================================================================
-- Tri-Stone Office App - one login per employee
-- User Accounts now picks the employee a login belongs to; an employee can't have two logins.
-- Accounts made before employees existed (e.g. the first Admin) may stay unlinked (NULL).
-- Run by setup-database.ps1. Safe to re-run.
-- =====================================================================

CREATE UNIQUE INDEX IF NOT EXISTS ux_users_employee
    ON users (employee_id) WHERE employee_id IS NOT NULL;
