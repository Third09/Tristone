-- =====================================================================
-- Tri-Stone Office App - Part 4: Subcontractors (DFD D4.1-D4.3)
-- Directory, jobs with fixed payment stages + retention, progress, vouchers and payouts.
-- Run by setup-database.ps1. Safe to re-run.
-- =====================================================================

-- ---------------------------------------------------------------------
-- Directory (D4.1)
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS subcontractors (
    subcontractor_id     SERIAL        PRIMARY KEY,
    business_name        VARCHAR(150)  NOT NULL UNIQUE,
    trade_name           VARCHAR(150),                          -- DBA
    dti_sec_no           VARCHAR(60),
    tin                  VARCHAR(30),
    primary_name         VARCHAR(100),
    primary_role         VARCHAR(80),
    primary_phone        VARCHAR(30),
    primary_mobile       VARCHAR(30),
    primary_email        VARCHAR(100),
    secondary_name       VARCHAR(100),
    secondary_role       VARCHAR(80),
    secondary_phone      VARCHAR(30),
    secondary_mobile     VARCHAR(30),
    secondary_email      VARCHAR(100),
    office_address       VARCHAR(255),
    yard_address         VARCHAR(255),
    bank_name            VARCHAR(100),                          -- printed on billing vouchers
    account_name         VARCHAR(100),
    account_number       VARCHAR(40),
    pcab_license_no      VARCHAR(60),
    pcab_classification  VARCHAR(60),
    pcab_expiry          DATE,                                  -- reminder before it expires
    created_by           INT           REFERENCES users (user_id),
    created_at           TIMESTAMP     NOT NULL DEFAULT NOW(),
    updated_at           TIMESTAMP     NOT NULL DEFAULT NOW()
);

-- Trade categories (multi-select): one row per ticked trade.
CREATE TABLE IF NOT EXISTS subcontractor_trades (
    subcontractor_id     INT           NOT NULL REFERENCES subcontractors (subcontractor_id) ON DELETE CASCADE,
    trade                VARCHAR(60)   NOT NULL,
    PRIMARY KEY (subcontractor_id, trade)
);

-- ---------------------------------------------------------------------
-- Jobs (D4.2): subcontractor + project + scope + amount + dates; payment stages must total 100%
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS sub_jobs (
    job_id               SERIAL        PRIMARY KEY,
    subcontractor_id     INT           NOT NULL REFERENCES subcontractors (subcontractor_id),
    project_id           INT           NOT NULL REFERENCES projects (project_id),
    scope                VARCHAR(1000) NOT NULL,
    contract_amount      NUMERIC(14,2) NOT NULL CHECK (contract_amount > 0),
    start_date           DATE          NOT NULL,
    end_date             DATE          NOT NULL,
    retention_pct        NUMERIC(5,2)  NOT NULL DEFAULT 10 CHECK (retention_pct >= 0 AND retention_pct < 100),
    created_by           INT           REFERENCES users (user_id),
    created_at           TIMESTAMP     NOT NULL DEFAULT NOW(),
    updated_at           TIMESTAMP     NOT NULL DEFAULT NOW(),
    CHECK (end_date >= start_date)
);
CREATE INDEX IF NOT EXISTS ix_sub_jobs_project ON sub_jobs (project_id);

CREATE TABLE IF NOT EXISTS sub_job_stages (
    stage_id             SERIAL        PRIMARY KEY,
    job_id               INT           NOT NULL REFERENCES sub_jobs (job_id),
    stage_no             INT           NOT NULL,
    name                 VARCHAR(80)   NOT NULL,
    percent              NUMERIC(5,2)  NOT NULL CHECK (percent > 0 AND percent <= 100),
    reached_on           DATE,                                  -- set by "Mark Stage Reached"
    reached_by           INT           REFERENCES users (user_id),
    UNIQUE (job_id, stage_no)
);

CREATE TABLE IF NOT EXISTS sub_job_documents (
    document_id          SERIAL        PRIMARY KEY,
    job_id               INT           NOT NULL REFERENCES sub_jobs (job_id),
    file_path            VARCHAR(400)  NOT NULL,                -- relative to file_storage_path
    original_name        VARCHAR(255)  NOT NULL,
    uploaded_by          INT           REFERENCES users (user_id),
    uploaded_at          TIMESTAMP     NOT NULL DEFAULT NOW()
);

-- Progress updates: % completed on a date, with notes and an optional photo or document.
CREATE TABLE IF NOT EXISTS sub_progress (
    progress_id          SERIAL        PRIMARY KEY,
    job_id               INT           NOT NULL REFERENCES sub_jobs (job_id),
    percent              NUMERIC(5,2)  NOT NULL CHECK (percent >= 0 AND percent <= 100),
    progress_date        DATE          NOT NULL,
    notes                VARCHAR(500),
    file_path            VARCHAR(400),
    original_name        VARCHAR(255),
    created_by           INT           REFERENCES users (user_id),
    created_at           TIMESTAMP     NOT NULL DEFAULT NOW()
);

-- ---------------------------------------------------------------------
-- Billing (D4.3): the Employee prepares a voucher for a reached stage (or for the held retention once the
-- job is finished); the Supervisor approves; then the payout is recorded.
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS sub_vouchers (
    voucher_id           SERIAL        PRIMARY KEY,
    voucher_no           VARCHAR(20)   NOT NULL UNIQUE,         -- SV-0001
    job_id               INT           NOT NULL REFERENCES sub_jobs (job_id),
    stage_id             INT           REFERENCES sub_job_stages (stage_id),   -- NULL = retention release
    kind                 VARCHAR(10)   NOT NULL CHECK (kind IN ('Stage', 'Retention')),
    gross_amount         NUMERIC(14,2) NOT NULL CHECK (gross_amount > 0),
    retention_amount     NUMERIC(14,2) NOT NULL DEFAULT 0 CHECK (retention_amount >= 0),
    payable_amount       NUMERIC(14,2) NOT NULL CHECK (payable_amount > 0),
    status               VARCHAR(10)   NOT NULL DEFAULT 'Waiting' CHECK (status IN ('Waiting', 'Approved', 'Rejected', 'Paid')),
    prepared_by          INT           REFERENCES users (user_id),
    prepared_at          TIMESTAMP     NOT NULL DEFAULT NOW(),
    decided_by           INT           REFERENCES users (user_id),
    decided_at           TIMESTAMP,
    CHECK ((kind = 'Stage') = (stage_id IS NOT NULL))
);
-- One live voucher per stage, and one live retention release per job (a rejected one can be redone).
CREATE UNIQUE INDEX IF NOT EXISTS ux_sub_vouchers_stage ON sub_vouchers (stage_id) WHERE status <> 'Rejected';
CREATE UNIQUE INDEX IF NOT EXISTS ux_sub_vouchers_retention ON sub_vouchers (job_id) WHERE kind = 'Retention' AND status <> 'Rejected';

-- Payouts count as project expenses (Dashboard, Projects page).
CREATE TABLE IF NOT EXISTS sub_payouts (
    payout_id            SERIAL        PRIMARY KEY,
    voucher_id           INT           NOT NULL UNIQUE REFERENCES sub_vouchers (voucher_id),
    job_id               INT           NOT NULL REFERENCES sub_jobs (job_id),
    paid_on              DATE          NOT NULL,
    amount               NUMERIC(14,2) NOT NULL CHECK (amount > 0),
    reference_no         VARCHAR(60),
    method               VARCHAR(20)   NOT NULL CHECK (method IN ('Bank Transfer', 'Check', 'Cash')),
    created_by           INT           REFERENCES users (user_id),
    created_at           TIMESTAMP     NOT NULL DEFAULT NOW()
);
CREATE INDEX IF NOT EXISTS ix_sub_payouts_job ON sub_payouts (job_id);
