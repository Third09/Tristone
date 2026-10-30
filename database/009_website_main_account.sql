-- =====================================================================
-- Tri-Stone Website Admin app - main accounts
-- Only a MAIN account can manage the other Website Admin accounts
-- (owner decision 2026-10-30), like Admins in the office app.
-- Run once. Safe to re-run.
-- =====================================================================

ALTER TABLE website_accounts ADD COLUMN IF NOT EXISTS is_main BOOLEAN NOT NULL DEFAULT FALSE;

-- Accounts made before this script (by hand, Developer Guide 10.2): if none is main yet,
-- the oldest one becomes main, so somebody can always manage the accounts.
UPDATE website_accounts SET is_main = TRUE
WHERE account_id = (SELECT MIN(account_id) FROM website_accounts)
  AND NOT EXISTS (SELECT 1 FROM website_accounts WHERE is_main);
