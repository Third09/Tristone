-- =====================================================================
-- Tri-Stone - quotations follow the company's own quotation paper (owner request 2026-11-09, "new quote.docx")
--   Customer / Address / Attention / Date, Scope of Works (A, B, C... with sizes), cost lines (item + amount),
--   Total Cost, printed Notes, Work Duration, Terms of Payment, Cash Payable to (name + number),
--   Conforme by / Very truly yours (signatory name + title).
-- No discount, VAT, quantity, unit or unit price on that paper: those columns go.
-- Run once. Safe to re-run.
-- =====================================================================

ALTER TABLE quotations ADD COLUMN IF NOT EXISTS client_address  VARCHAR(255);
ALTER TABLE quotations ADD COLUMN IF NOT EXISTS attention       VARCHAR(150);
ALTER TABLE quotations ADD COLUMN IF NOT EXISTS work_duration   VARCHAR(100);
ALTER TABLE quotations ADD COLUMN IF NOT EXISTS payment_terms   VARCHAR(150);
ALTER TABLE quotations ADD COLUMN IF NOT EXISTS payable_to      VARCHAR(150);
ALTER TABLE quotations ADD COLUMN IF NOT EXISTS payable_number  VARCHAR(60);
ALTER TABLE quotations ADD COLUMN IF NOT EXISTS signatory_name  VARCHAR(150);
ALTER TABLE quotations ADD COLUMN IF NOT EXISTS signatory_title VARCHAR(100);
ALTER TABLE quotations DROP COLUMN IF EXISTS discount;
ALTER TABLE quotations DROP COLUMN IF EXISTS vat_applies;

-- terms -> printed_notes (the bullet list under "Notes:"), notes -> internal_notes (never printed)
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'quotations' AND column_name = 'terms') THEN
        ALTER TABLE quotations RENAME COLUMN terms TO printed_notes;
    END IF;
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'quotations' AND column_name = 'notes') THEN
        ALTER TABLE quotations RENAME COLUMN notes TO internal_notes;
    END IF;
END $$;

-- cost lines: item + amount (e.g. "Tools and Equipment" 5,000; "Labor Cost" 25,000)
ALTER TABLE quotation_items ADD COLUMN IF NOT EXISTS amount NUMERIC(14,2);
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'quotation_items' AND column_name = 'unit_price') THEN
        UPDATE quotation_items SET amount = ROUND(quantity * unit_price, 2) WHERE amount IS NULL;
    END IF;
END $$;
ALTER TABLE quotation_items ALTER COLUMN amount SET NOT NULL;
ALTER TABLE quotation_items DROP CONSTRAINT IF EXISTS chk_quotation_items_amount;
ALTER TABLE quotation_items ADD CONSTRAINT chk_quotation_items_amount CHECK (amount >= 0);
ALTER TABLE quotation_items DROP COLUMN IF EXISTS quantity;
ALTER TABLE quotation_items DROP COLUMN IF EXISTS unit;
ALTER TABLE quotation_items DROP COLUMN IF EXISTS unit_price;

-- Scope of Works: lettered A, B, C... in line order; details = size or measurement (e.g. 2.5 X 1.5 X 1.5 METERS)
CREATE TABLE IF NOT EXISTS quotation_scope (
    scope_id      SERIAL         PRIMARY KEY,
    quotation_id  INT            NOT NULL REFERENCES quotations (quotation_id) ON DELETE CASCADE,
    line_no       INT            NOT NULL,
    description   VARCHAR(300)   NOT NULL,
    details       VARCHAR(150),
    UNIQUE (quotation_id, line_no)
);

DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'tristone_app') THEN
        GRANT SELECT, INSERT, UPDATE, DELETE ON quotation_scope TO tristone_app;
        GRANT USAGE, SELECT, UPDATE ON SEQUENCE quotation_scope_scope_id_seq TO tristone_app;
    END IF;
END $$;
