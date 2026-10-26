-- =====================================================================
-- Tri-Stone Office App - Part 0: Login & Security
-- Database: construction_db (PostgreSQL 18)
-- Run once. Safe to re-run: every object uses IF NOT EXISTS.
-- =====================================================================

-- bcrypt password hashing (crypt / gen_salt) for seeding the first Admin.
-- The app itself hashes with BCrypt.Net, which reads the same $2a$ format.
CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- ---------------------------------------------------------------------
-- users: one login per office staff member (site workers never get one)
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS users (
    user_id              SERIAL       PRIMARY KEY,
    employee_id          INT,                                   -- link to employees (added in Part 2)
    username             VARCHAR(50)  NOT NULL UNIQUE,
    password_hash        VARCHAR(255) NOT NULL,                 -- bcrypt, never plain text
    full_name            VARCHAR(100) NOT NULL,
    level                VARCHAR(20)  NOT NULL
                         CHECK (level IN ('Admin', 'Supervisor', 'Employee')),
    department           VARCHAR(30)
                         CHECK (department IN ('HR & Payroll', 'Inventory', 'Subcontractors', 'Receipts')),
    status               VARCHAR(20)  NOT NULL DEFAULT 'Active'
                         CHECK (status IN ('Active', 'Locked', 'Disabled')),
    failed_login_count   INT          NOT NULL DEFAULT 0,       -- locks at 5
    security_question    VARCHAR(200),                          -- Admin only
    security_answer_hash VARCHAR(255),                          -- Admin only, bcrypt
    last_login_at        TIMESTAMP,
    created_at           TIMESTAMP    NOT NULL DEFAULT NOW(),
    updated_at           TIMESTAMP    NOT NULL DEFAULT NOW(),

    -- Supervisors and Employees belong to exactly one department; Admins to none.
    CONSTRAINT chk_users_department
        CHECK ((level = 'Admin' AND department IS NULL)
            OR (level <> 'Admin' AND department IS NOT NULL))
);

-- ---------------------------------------------------------------------
-- login_logs: sign ins, sign outs, failed attempts, locks, password events
-- (shown in Dashboard > Log History)
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS login_logs (
    log_id          SERIAL      PRIMARY KEY,
    user_id         INT         REFERENCES users (user_id),     -- NULL when the username didn't exist
    username_tried  VARCHAR(50),
    action          VARCHAR(30) NOT NULL
                    CHECK (action IN ('Login', 'Logout', 'Idle Sign Out', 'Failed Login',
                                      'Account Locked', 'Account Unlocked',
                                      'Password Changed', 'Password Request')),
    details         VARCHAR(255),                               -- e.g. "set by admin1"
    computer_name   VARCHAR(100),
    logged_at       TIMESTAMP   NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS ix_login_logs_logged_at ON login_logs (logged_at DESC);
CREATE INDEX IF NOT EXISTS ix_login_logs_user      ON login_logs (user_id);

-- ---------------------------------------------------------------------
-- password_requests: "Forgot Password" from Employees / Supervisors.
-- Waiting requests drive the Admin notification badge.
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS password_requests (
    request_id    SERIAL      PRIMARY KEY,
    user_id       INT         NOT NULL REFERENCES users (user_id),
    requested_at  TIMESTAMP   NOT NULL DEFAULT NOW(),
    status        VARCHAR(20) NOT NULL DEFAULT 'Waiting'
                  CHECK (status IN ('Waiting', 'Done', 'Dismissed')),
    handled_by    INT         REFERENCES users (user_id),       -- the Admin who handled it
    handled_at    TIMESTAMP
);

CREATE INDEX IF NOT EXISTS ix_password_requests_waiting
    ON password_requests (status) WHERE status = 'Waiting';

-- ---------------------------------------------------------------------
-- audit_logs: who changed what, and when (every department's Edit History tab)
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS audit_logs (
    audit_id    SERIAL      PRIMARY KEY,
    user_id     INT         REFERENCES users (user_id),
    department  VARCHAR(30),                                    -- which Edit History tab shows it
    table_name  VARCHAR(50) NOT NULL,
    record_id   INT,
    record_name VARCHAR(150),                                   -- readable name, e.g. "Juan Cruz"
    action      VARCHAR(10) NOT NULL CHECK (action IN ('Added', 'Edited', 'Deleted')),
    old_values  JSONB,
    new_values  JSONB,
    changed_at  TIMESTAMP   NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS ix_audit_logs_dept_time ON audit_logs (department, changed_at DESC);
