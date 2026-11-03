-- =====================================================================
-- Tri-Stone Website Admin - no category on service cards (owner decision 2026-11-03)
-- The category reused the office project sectors, which don't fit services such as Electrical Works or Landscaping,
-- and the website never showed it. Removed from Website Admin > Services.
-- Run once. Safe to re-run.
-- =====================================================================

ALTER TABLE website_services DROP COLUMN IF EXISTS sector;
