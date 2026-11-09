-- =====================================================================
-- Tri-Stone - client quotations (owner request 2026-11-09)
-- A price offer to a client before a project starts: QT-0001, QT-0002...
-- The Receipts Employee prepares it (Draft), sends it for approval (Waiting), the Receipts Supervisor
-- approves or rejects it; an approved one is printed and sent; when the client accepts, it becomes a project.
-- Lines: item, quantity, unit, unit price. Totals: subtotal - discount, + 12% VAT when ticked.
-- Run once. Safe to re-run.
-- =====================================================================

CREATE TABLE IF NOT EXISTS quotations (
    quotation_id    SERIAL         PRIMARY KEY,
    quote_no        VARCHAR(12)    NOT NULL UNIQUE,
    client_name     VARCHAR(150)   NOT NULL,
    client_contact  VARCHAR(60),
    client_email    VARCHAR(150),
    project_title   VARCHAR(150)   NOT NULL,
    site_location   VARCHAR(255),
    sector          VARCHAR(30)    NOT NULL DEFAULT 'Residential'
                    CHECK (sector IN ('Residential', 'Commercial', 'Industrial', 'Repairs / Renovations')),
    quote_date      DATE           NOT NULL DEFAULT CURRENT_DATE,
    valid_until     DATE           NOT NULL,
    planned_start   DATE,
    discount        NUMERIC(14,2)  NOT NULL DEFAULT 0 CHECK (discount >= 0),
    vat_applies     BOOLEAN        NOT NULL DEFAULT FALSE,
    terms           VARCHAR(2000),
    notes           VARCHAR(1000),                       -- internal, never printed
    status          VARCHAR(10)    NOT NULL DEFAULT 'Draft'
                    CHECK (status IN ('Draft', 'Waiting', 'Approved', 'Rejected', 'Accepted', 'Declined')),
    inquiry_id      INT            REFERENCES inquiries (inquiry_id),
    project_id      INT            REFERENCES projects (project_id),   -- set when Accepted
    created_by      INT            REFERENCES users (user_id),
    created_at      TIMESTAMP      NOT NULL DEFAULT NOW(),
    updated_at      TIMESTAMP,
    decided_by      INT            REFERENCES users (user_id),
    decided_at      TIMESTAMP,
    CONSTRAINT chk_quotations_valid CHECK (valid_until >= quote_date),
    CONSTRAINT chk_quotations_project CHECK ((status = 'Accepted') = (project_id IS NOT NULL))
);

CREATE INDEX IF NOT EXISTS ix_quotations_status ON quotations (status);

CREATE TABLE IF NOT EXISTS quotation_items (
    item_id        SERIAL         PRIMARY KEY,
    quotation_id   INT            NOT NULL REFERENCES quotations (quotation_id) ON DELETE CASCADE,
    line_no        INT            NOT NULL,
    description    VARCHAR(300)   NOT NULL,
    quantity       NUMERIC(12,2)  NOT NULL CHECK (quantity > 0),
    unit           VARCHAR(20)    NOT NULL,
    unit_price     NUMERIC(14,2)  NOT NULL CHECK (unit_price >= 0),
    UNIQUE (quotation_id, line_no)
);

-- the app's limited login (script 022) gets the same rights on the new tables
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'tristone_app') THEN
        GRANT SELECT, INSERT, UPDATE, DELETE ON quotations, quotation_items TO tristone_app;
        GRANT USAGE, SELECT, UPDATE ON SEQUENCE quotations_quotation_id_seq, quotation_items_item_id_seq TO tristone_app;
    END IF;
END $$;
