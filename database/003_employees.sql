-- =====================================================================
-- Tri-Stone Office App - Part 2a: Employees (DFD D2.1 Employee Records & Rates)
-- Office staff (who may have logins) and site workers (no login, attendance + payroll only).
-- Run by setup-database.ps1. Safe to re-run.
-- =====================================================================

CREATE TABLE IF NOT EXISTS employees (
    employee_id     SERIAL       PRIMARY KEY,
    full_name       VARCHAR(100) NOT NULL,
    contact_number  VARCHAR(30),
    address         VARCHAR(255),
    position        VARCHAR(80),                       -- e.g. "Mason", "HR Staff"
    department      VARCHAR(30)  NOT NULL
                    CHECK (department IN ('Administration', 'HR & Payroll', 'Inventory', 'Subcontractors', 'Receipts', 'Site')),
    -- Site workers are paid weekly (Saturday); everyone else twice a month (15th and 30th).
    pay_schedule    VARCHAR(10)  NOT NULL CHECK (pay_schedule IN ('Office', 'Site')),
    pay_grade       VARCHAR(30),
    biometric_id    VARCHAR(20)  UNIQUE,               -- the number the fingerprint scanner gives this person
    gov_deductions  BOOLEAN      NOT NULL DEFAULT FALSE,   -- tax / SSS / PhilHealth / Pag-IBIG on or off
    sss_no          VARCHAR(20),
    philhealth_no   VARCHAR(20),
    pagibig_no      VARCHAR(20),
    tin             VARCHAR(20),
    status          VARCHAR(10)  NOT NULL DEFAULT 'Active' CHECK (status IN ('Active', 'Inactive')),
    created_by      INT          REFERENCES users (user_id),
    created_at      TIMESTAMP    NOT NULL DEFAULT NOW(),
    updated_at      TIMESTAMP    NOT NULL DEFAULT NOW(),

    CONSTRAINT chk_employees_schedule_matches_department
        CHECK ((department = 'Site' AND pay_schedule = 'Site') OR (department <> 'Site' AND pay_schedule = 'Office'))
);

CREATE INDEX IF NOT EXISTS ix_employees_status ON employees (status);

-- Daily rate history: a new rate applies from a date HR chooses; past payslips keep the old rate.
CREATE TABLE IF NOT EXISTS employee_rates (
    rate_id         SERIAL        PRIMARY KEY,
    employee_id     INT           NOT NULL REFERENCES employees (employee_id),
    daily_rate      NUMERIC(10,2) NOT NULL CHECK (daily_rate > 0),
    effective_from  DATE          NOT NULL,
    created_by      INT           REFERENCES users (user_id),
    created_at      TIMESTAMP     NOT NULL DEFAULT NOW(),
    UNIQUE (employee_id, effective_from)
);

-- Logins can now point to the employee they belong to (users.employee_id was added in 001).
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_users_employee') THEN
        ALTER TABLE users ADD CONSTRAINT fk_users_employee FOREIGN KEY (employee_id) REFERENCES employees (employee_id);
    END IF;
END $$;
