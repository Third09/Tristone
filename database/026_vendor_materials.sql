-- =====================================================================
-- Tri-Stone - which materials each vendor supplies, at what price (owner request 2026-11-09)
-- Kept on Inventory > Vendors ("Materials supplied"). Price Comparison fills in a vendor's listed price,
-- and approving a purchase updates that vendor's price. Also from 2026-11-09: the Deliveries tab is gone;
-- an approved purchase adds the stock (old deliveries rows stay for the record and the Receipt Vault).
-- Run once. Safe to re-run.
-- =====================================================================

CREATE TABLE IF NOT EXISTS vendor_materials (
    vendor_id    INT            NOT NULL REFERENCES vendors (vendor_id) ON DELETE CASCADE,
    material_id  INT            NOT NULL REFERENCES materials (material_id) ON DELETE CASCADE,
    price        NUMERIC(12,2)  NOT NULL CHECK (price >= 0),
    updated_by   INT            REFERENCES users (user_id),
    updated_at   TIMESTAMP      NOT NULL DEFAULT NOW(),
    PRIMARY KEY (vendor_id, material_id)
);

CREATE INDEX IF NOT EXISTS ix_vendor_materials_material ON vendor_materials (material_id);

DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'tristone_app') THEN
        GRANT SELECT, INSERT, UPDATE, DELETE ON vendor_materials TO tristone_app;
    END IF;
END $$;
