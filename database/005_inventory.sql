-- =====================================================================
-- Tri-Stone Office App - Part 3: Inventory & Vendors (DFD D3.1-D3.5)
-- Plus app_settings (shared settings such as where scanned files are stored).
-- Run by setup-database.ps1. Safe to re-run.
-- =====================================================================

-- Settings every PC shares (one row per key). First one: the folder for uploaded scans and photos.
CREATE TABLE IF NOT EXISTS app_settings (
    key         VARCHAR(60)  PRIMARY KEY,
    value       TEXT         NOT NULL,
    updated_by  INT          REFERENCES users (user_id),
    updated_at  TIMESTAMP    NOT NULL DEFAULT NOW()
);
INSERT INTO app_settings (key, value) VALUES ('file_storage_path', 'C:\TriStoneFiles')
ON CONFLICT (key) DO NOTHING;

-- ---------------------------------------------------------------------
-- Materials (D3.1): stock and safety thresholds
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS materials (
    material_id     SERIAL        PRIMARY KEY,
    name            VARCHAR(120)  NOT NULL UNIQUE,
    category        VARCHAR(60)   NOT NULL,
    unit            VARCHAR(20)   NOT NULL,                     -- bag, piece, meter...
    unit_price      NUMERIC(12,2) NOT NULL DEFAULT 0 CHECK (unit_price >= 0),   -- latest price per unit
    stock_qty       NUMERIC(12,2) NOT NULL DEFAULT 0 CHECK (stock_qty >= 0),
    minimum_stock   NUMERIC(12,2) NOT NULL DEFAULT 0 CHECK (minimum_stock >= 0),
    created_by      INT           REFERENCES users (user_id),
    created_at      TIMESTAMP     NOT NULL DEFAULT NOW(),
    updated_at      TIMESTAMP     NOT NULL DEFAULT NOW()
);

-- Material used on a project: Employee records, Supervisor approves; then stock goes down and the cost
-- (quantity x price at approval) counts as that project's expense.
CREATE TABLE IF NOT EXISTS material_usages (
    usage_id        SERIAL        PRIMARY KEY,
    material_id     INT           NOT NULL REFERENCES materials (material_id),
    project_id      INT           NOT NULL REFERENCES projects (project_id),
    quantity        NUMERIC(12,2) NOT NULL CHECK (quantity > 0),
    used_on         DATE          NOT NULL,
    notes           VARCHAR(255),
    status          VARCHAR(10)   NOT NULL DEFAULT 'Waiting' CHECK (status IN ('Waiting', 'Approved', 'Rejected')),
    cost            NUMERIC(14,2),                              -- set on approval
    requested_by    INT           REFERENCES users (user_id),
    decided_by      INT           REFERENCES users (user_id),
    decided_at      TIMESTAMP,
    created_at      TIMESTAMP     NOT NULL DEFAULT NOW()
);
CREATE INDEX IF NOT EXISTS ix_material_usages_project ON material_usages (project_id) WHERE status = 'Approved';

-- ---------------------------------------------------------------------
-- Vendors + their papers
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS vendors (
    vendor_id       SERIAL        PRIMARY KEY,
    name            VARCHAR(150)  NOT NULL UNIQUE,
    contact_person  VARCHAR(100),
    phone           VARCHAR(30),
    email           VARCHAR(100),
    address         VARCHAR(255),
    bank_name       VARCHAR(100),
    account_name    VARCHAR(100),
    account_number  VARCHAR(40),
    vat_status      VARCHAR(10)   NOT NULL DEFAULT 'VAT' CHECK (vat_status IN ('VAT', 'Non-VAT')),
    created_by      INT           REFERENCES users (user_id),
    created_at      TIMESTAMP     NOT NULL DEFAULT NOW(),
    updated_at      TIMESTAMP     NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS vendor_documents (
    document_id     SERIAL        PRIMARY KEY,
    vendor_id       INT           NOT NULL REFERENCES vendors (vendor_id),
    doc_type        VARCHAR(40)   NOT NULL CHECK (doc_type IN ('Company Profile', 'Bank Details', 'Certificate of Registration', 'Other')),
    file_path       VARCHAR(400)  NOT NULL,                      -- relative to file_storage_path
    original_name   VARCHAR(255)  NOT NULL,
    uploaded_by     INT           REFERENCES users (user_id),
    uploaded_at     TIMESTAMP     NOT NULL DEFAULT NOW()
);

-- ---------------------------------------------------------------------
-- Price comparison (D3.5): quotes from several vendors; the Supervisor picks one = approved purchase
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS quotes (
    quote_id        SERIAL        PRIMARY KEY,
    material_id     INT           NOT NULL REFERENCES materials (material_id),
    vendor_id       INT           NOT NULL REFERENCES vendors (vendor_id),
    quoted_on       DATE          NOT NULL,
    product_name    VARCHAR(150)  NOT NULL,
    description     VARCHAR(255),
    category        VARCHAR(60),
    unit_price      NUMERIC(12,2) NOT NULL CHECK (unit_price > 0),
    quantity        NUMERIC(12,2) NOT NULL CHECK (quantity > 0),
    status          VARCHAR(12)   NOT NULL DEFAULT 'Open' CHECK (status IN ('Open', 'Selected', 'Not Selected')),
    created_by      INT           REFERENCES users (user_id),
    created_at      TIMESTAMP     NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS purchases (
    purchase_id     SERIAL        PRIMARY KEY,
    quote_id        INT           NOT NULL UNIQUE REFERENCES quotes (quote_id),
    material_id     INT           NOT NULL REFERENCES materials (material_id),
    vendor_id       INT           NOT NULL REFERENCES vendors (vendor_id),
    quantity        NUMERIC(12,2) NOT NULL CHECK (quantity > 0),
    unit_price      NUMERIC(12,2) NOT NULL CHECK (unit_price > 0),
    approved_by     INT           REFERENCES users (user_id),
    approved_at     TIMESTAMP     NOT NULL DEFAULT NOW()
);

-- Deliveries: no approval (the purchase was approved). Stock goes up. Every delivery has its receipt:
-- a scan, or "physical copy only" with the receipt number and where the paper is filed.
CREATE TABLE IF NOT EXISTS deliveries (
    delivery_id         SERIAL        PRIMARY KEY,
    purchase_id         INT           REFERENCES purchases (purchase_id),
    vendor_id           INT           NOT NULL REFERENCES vendors (vendor_id),
    material_id         INT           NOT NULL REFERENCES materials (material_id),
    quantity            NUMERIC(12,2) NOT NULL CHECK (quantity > 0),
    unit_price          NUMERIC(12,2) NOT NULL CHECK (unit_price >= 0),
    delivered_on        DATE          NOT NULL,
    receipt_no          VARCHAR(60)   NOT NULL,
    receipt_file_path   VARCHAR(400),                            -- relative to file_storage_path
    physical_only       BOOLEAN       NOT NULL DEFAULT FALSE,
    filed_location      VARCHAR(150),
    recorded_by         INT           REFERENCES users (user_id),
    created_at          TIMESTAMP     NOT NULL DEFAULT NOW(),
    CONSTRAINT chk_deliveries_receipt
        CHECK ((physical_only AND filed_location IS NOT NULL) OR (NOT physical_only AND receipt_file_path IS NOT NULL))
);

-- ---------------------------------------------------------------------
-- Equipment (D3.2) + maintenance and repair log (D3.4). Location is not tracked.
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS equipment (
    equipment_id        SERIAL        PRIMARY KEY,
    name                VARCHAR(120)  NOT NULL,
    type                VARCHAR(20)   NOT NULL CHECK (type IN ('Tool', 'Heavy Equipment', 'Vehicle')),
    ownership           VARCHAR(10)   NOT NULL CHECK (ownership IN ('Owned', 'Rented')),
    rental_company      VARCHAR(150),
    rental_rate         NUMERIC(12,2) CHECK (rental_rate >= 0),
    rental_rate_unit    VARCHAR(10)   CHECK (rental_rate_unit IN ('Day', 'Week', 'Month')),
    return_date         DATE,
    status              VARCHAR(15)   NOT NULL DEFAULT 'Available' CHECK (status IN ('Available', 'Under Repair', 'Retired')),
    next_maintenance    DATE,
    created_by          INT           REFERENCES users (user_id),
    created_at          TIMESTAMP     NOT NULL DEFAULT NOW(),
    updated_at          TIMESTAMP     NOT NULL DEFAULT NOW(),
    CONSTRAINT chk_equipment_rental
        CHECK (ownership = 'Owned' OR (rental_company IS NOT NULL AND rental_rate IS NOT NULL AND rental_rate_unit IS NOT NULL))
);

-- Repair and maintenance costs are company-wide overhead (not per project).
CREATE TABLE IF NOT EXISTS maintenance_logs (
    log_id          SERIAL        PRIMARY KEY,
    equipment_id    INT           NOT NULL REFERENCES equipment (equipment_id),
    done_on         DATE          NOT NULL,
    kind            VARCHAR(12)   NOT NULL CHECK (kind IN ('Preventive', 'Repair')),
    work_done       VARCHAR(500)  NOT NULL,
    cost            NUMERIC(12,2) NOT NULL DEFAULT 0 CHECK (cost >= 0),
    done_by         VARCHAR(150),
    recorded_by     INT           REFERENCES users (user_id),
    created_at      TIMESTAMP     NOT NULL DEFAULT NOW()
);
