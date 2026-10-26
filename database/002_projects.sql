-- =====================================================================
-- Tri-Stone Office App - Part 1b: Projects
-- The one shared list of company projects (DFD D1.1 / D5.3).
-- Receipts, Inventory, Subcontractors and Website Admin pick from it.
-- Run by setup-database.ps1. Safe to re-run: every object uses IF NOT EXISTS.
-- =====================================================================

CREATE TABLE IF NOT EXISTS projects (
    project_id       SERIAL        PRIMARY KEY,
    project_name     VARCHAR(150)  NOT NULL,
    site_location    VARCHAR(255)  NOT NULL,
    client_name      VARCHAR(150),
    sector           VARCHAR(40)   NOT NULL
                     CHECK (sector IN ('Residential', 'Commercial', 'Industrial', 'Repairs / Renovations')),
    contract_price   NUMERIC(14,2) NOT NULL DEFAULT 0 CHECK (contract_price >= 0),   -- pesos
    start_date       DATE          NOT NULL,
    target_end_date  DATE,
    completed_date   DATE,                                     -- set automatically when status becomes Done
    status           VARCHAR(20)   NOT NULL DEFAULT 'Pending'
                     CHECK (status IN ('Pending', 'Ongoing', 'Done', 'Cancelled')),
    show_on_website  BOOLEAN       NOT NULL DEFAULT FALSE,     -- sends it to Website Admin > Website Projects
    created_by       INT           REFERENCES users (user_id),
    created_at       TIMESTAMP     NOT NULL DEFAULT NOW(),
    updated_at       TIMESTAMP     NOT NULL DEFAULT NOW(),

    CONSTRAINT chk_projects_target_after_start
        CHECK (target_end_date IS NULL OR target_end_date >= start_date),
    CONSTRAINT chk_projects_completed_after_start
        CHECK (completed_date IS NULL OR completed_date >= start_date)
);

CREATE INDEX IF NOT EXISTS ix_projects_status ON projects (status);
CREATE INDEX IF NOT EXISTS ix_projects_website ON projects (show_on_website) WHERE show_on_website;
