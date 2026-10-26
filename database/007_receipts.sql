-- =====================================================================
-- Tri-Stone Office App - Part 5: Receipts & change orders (DFD D5.1-D5.2)
-- Run by setup-database.ps1. Safe to re-run.
-- =====================================================================

-- ---------------------------------------------------------------------
-- Receipts (D5.1). Each one is a Project expense or Company overhead. The Receipts Employee uploads, the
-- Receipts Supervisor (or an Admin) approves; only approved ones count as expenses.
-- A receipt copy is a scan, or "physical copy only" with where the paper is filed. With no receipt at all
-- (no_receipt) a reason is required and it shows under Needs Review until approved; attaching a receipt
-- later clears the flag. Material delivery receipts are NOT stored here: the vault shows them straight from
-- Inventory's deliveries as "Stock purchase" (counted when the material is used, not twice).
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS receipts (
    receipt_id          SERIAL        PRIMARY KEY,
    receipt_no          VARCHAR(60),                            -- empty only when no_receipt
    purchase_date       DATE          NOT NULL,
    merchant            VARCHAR(150)  NOT NULL,
    amount              NUMERIC(14,2) NOT NULL CHECK (amount > 0),
    category            VARCHAR(40)   NOT NULL,
    expense_type        VARCHAR(10)   NOT NULL CHECK (expense_type IN ('Project', 'Overhead')),
    project_id          INT           REFERENCES projects (project_id),
    file_path           VARCHAR(400),                           -- relative to file_storage_path
    original_name       VARCHAR(255),
    physical_only       BOOLEAN       NOT NULL DEFAULT FALSE,
    filed_location      VARCHAR(150),
    no_receipt          BOOLEAN       NOT NULL DEFAULT FALSE,
    no_receipt_reason   VARCHAR(500),
    status              VARCHAR(10)   NOT NULL DEFAULT 'Waiting' CHECK (status IN ('Waiting', 'Approved', 'Rejected')),
    uploaded_by         INT           REFERENCES users (user_id),
    uploaded_at         TIMESTAMP     NOT NULL DEFAULT NOW(),
    decided_by          INT           REFERENCES users (user_id),
    decided_at          TIMESTAMP,
    updated_at          TIMESTAMP     NOT NULL DEFAULT NOW(),
    CONSTRAINT chk_receipts_project CHECK ((expense_type = 'Project') = (project_id IS NOT NULL)),
    CONSTRAINT chk_receipts_copy CHECK (
        (no_receipt AND no_receipt_reason IS NOT NULL AND file_path IS NULL AND NOT physical_only)
        OR (NOT no_receipt AND receipt_no IS NOT NULL
            AND ((physical_only AND filed_location IS NOT NULL) OR (NOT physical_only AND file_path IS NOT NULL))))
);
CREATE INDEX IF NOT EXISTS ix_receipts_project ON receipts (project_id) WHERE status = 'Approved';
CREATE INDEX IF NOT EXISTS ix_receipts_date ON receipts (purchase_date);

-- ---------------------------------------------------------------------
-- Change orders (D5.2): logged by the Receipts Employee, approved by an Admin. Once approved the cost
-- change adds to the project's revenue and the days change moves its target end date.
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS change_orders (
    change_order_id     SERIAL        PRIMARY KEY,
    co_no               VARCHAR(20)   NOT NULL UNIQUE,          -- CO-001
    project_id          INT           NOT NULL REFERENCES projects (project_id),
    requested_on        DATE          NOT NULL,
    description         VARCHAR(1000) NOT NULL,
    cost_change         NUMERIC(14,2) NOT NULL DEFAULT 0,       -- + or -
    days_change         INT           NOT NULL DEFAULT 0,       -- + or -
    file_path           VARCHAR(400),                           -- the client's signed document
    original_name       VARCHAR(255),
    status              VARCHAR(10)   NOT NULL DEFAULT 'Waiting' CHECK (status IN ('Waiting', 'Approved', 'Rejected')),
    created_by          INT           REFERENCES users (user_id),
    created_at          TIMESTAMP     NOT NULL DEFAULT NOW(),
    decided_by          INT           REFERENCES users (user_id),
    decided_at          TIMESTAMP,
    CHECK (cost_change <> 0 OR days_change <> 0)
);
CREATE INDEX IF NOT EXISTS ix_change_orders_project ON change_orders (project_id) WHERE status = 'Approved';
