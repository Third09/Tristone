-- =====================================================================
-- Tri-Stone Office App - Part 2: HR Settings, cash advances, payroll and payslips (DFD D2.4, D2.5)
-- Work hours, overtime rate and government deduction rates are kept in app_settings (keys "hr.*").
-- Run by setup-database.ps1. Safe to re-run.
-- =====================================================================

-- Holidays: no one is marked Absent on them; paydays on them move to the day before.
-- Regular = paid even when not worked; Special = no work, no pay.
CREATE TABLE IF NOT EXISTS holidays (
    holiday_date    DATE          PRIMARY KEY,
    name            VARCHAR(100)  NOT NULL,
    kind            VARCHAR(10)   NOT NULL DEFAULT 'Regular' CHECK (kind IN ('Regular', 'Special'))
);

-- Cash advances (vale): HR Employee records, HR Supervisor approves; the same amount is taken every payday.
CREATE TABLE IF NOT EXISTS cash_advances (
    advance_id      SERIAL        PRIMARY KEY,
    employee_id     INT           NOT NULL REFERENCES employees (employee_id),
    given_on        DATE          NOT NULL,
    amount          NUMERIC(12,2) NOT NULL CHECK (amount > 0),
    per_payday      NUMERIC(12,2) NOT NULL CHECK (per_payday > 0),
    balance         NUMERIC(12,2) NOT NULL CHECK (balance >= 0),
    reason          VARCHAR(500),
    status          VARCHAR(10)   NOT NULL DEFAULT 'Waiting' CHECK (status IN ('Waiting', 'Active', 'Paid Off', 'Rejected')),
    created_by      INT           REFERENCES users (user_id),
    created_at      TIMESTAMP     NOT NULL DEFAULT NOW(),
    decided_by      INT           REFERENCES users (user_id),
    decided_at      TIMESTAMP
);

-- One payroll per pay group and period. Draft = can be computed again; Final = locked, payslips issued.
CREATE TABLE IF NOT EXISTS payroll_runs (
    run_id          SERIAL        PRIMARY KEY,
    pay_group       VARCHAR(10)   NOT NULL CHECK (pay_group IN ('Office', 'Site')),
    period_from     DATE          NOT NULL,
    period_to       DATE          NOT NULL,
    pay_date        DATE          NOT NULL,
    status          VARCHAR(10)   NOT NULL DEFAULT 'Draft' CHECK (status IN ('Draft', 'Final')),
    computed_by     INT           REFERENCES users (user_id),
    computed_at     TIMESTAMP     NOT NULL DEFAULT NOW(),
    finalized_by    INT           REFERENCES users (user_id),
    finalized_at    TIMESTAMP,
    UNIQUE (pay_group, period_from),
    CHECK (period_to >= period_from)
);

-- One payslip per employee per payroll.
CREATE TABLE IF NOT EXISTS payslips (
    payslip_id      SERIAL        PRIMARY KEY,
    run_id          INT           NOT NULL REFERENCES payroll_runs (run_id),
    employee_id     INT           NOT NULL REFERENCES employees (employee_id),
    daily_rate      NUMERIC(10,2) NOT NULL,                 -- the rate on the period's last day (shown on the slip)
    days_paid       NUMERIC(5,1)  NOT NULL DEFAULT 0,
    basic_pay       NUMERIC(12,2) NOT NULL DEFAULT 0,       -- daily rate x days paid (each day at its own rate)
    holiday_days    INT           NOT NULL DEFAULT 0,
    holiday_pay     NUMERIC(12,2) NOT NULL DEFAULT 0,       -- regular holidays not worked
    late_minutes    INT           NOT NULL DEFAULT 0,       -- late + early
    late_deduction  NUMERIC(12,2) NOT NULL DEFAULT 0,
    ot_minutes      INT           NOT NULL DEFAULT 0,       -- approved only
    ot_pay          NUMERIC(12,2) NOT NULL DEFAULT 0,
    gross_pay       NUMERIC(12,2) NOT NULL DEFAULT 0,
    sss             NUMERIC(12,2) NOT NULL DEFAULT 0,
    philhealth      NUMERIC(12,2) NOT NULL DEFAULT 0,
    pagibig         NUMERIC(12,2) NOT NULL DEFAULT 0,
    tax             NUMERIC(12,2) NOT NULL DEFAULT 0,
    cash_advance    NUMERIC(12,2) NOT NULL DEFAULT 0,
    net_pay         NUMERIC(12,2) NOT NULL DEFAULT 0 CHECK (net_pay >= 0),
    UNIQUE (run_id, employee_id)
);

-- What each payslip took from each cash advance.
CREATE TABLE IF NOT EXISTS cash_advance_deductions (
    deduction_id    SERIAL        PRIMARY KEY,
    advance_id      INT           NOT NULL REFERENCES cash_advances (advance_id),
    payslip_id      INT           NOT NULL REFERENCES payslips (payslip_id),
    amount          NUMERIC(12,2) NOT NULL CHECK (amount > 0),
    balance_after   NUMERIC(12,2) NOT NULL CHECK (balance_after >= 0)
);
