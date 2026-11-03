-- =====================================================================
-- Tri-Stone Website Admin - where each inquiry came from (owner decision 2026-11-03)
-- The website's Contact form saves each message in a Google Sheet through its Google Apps Script;
-- the Contact Us page copies new rows into inquiries. source_ref = the script's id for that message,
-- so a message is never added twice (even when two PCs open Contact Us at the same time).
-- Run once. Safe to re-run.
-- =====================================================================

ALTER TABLE inquiries ADD COLUMN IF NOT EXISTS source_ref VARCHAR(64);

CREATE UNIQUE INDEX IF NOT EXISTS ux_inquiries_source_ref ON inquiries (source_ref) WHERE source_ref IS NOT NULL;
