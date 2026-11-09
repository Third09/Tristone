-- =====================================================================
-- Tri-Stone - a price for each Scope of Works line (owner request 2026-11-09)
-- Optional; when typed it prints beside the work and is added to the Total Cost with the cost lines.
-- Run once. Safe to re-run.
-- =====================================================================

ALTER TABLE quotation_scope ADD COLUMN IF NOT EXISTS price NUMERIC(14,2);
ALTER TABLE quotation_scope DROP CONSTRAINT IF EXISTS chk_quotation_scope_price;
ALTER TABLE quotation_scope ADD CONSTRAINT chk_quotation_scope_price CHECK (price IS NULL OR price >= 0);
