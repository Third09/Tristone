-- =====================================================================
-- Tri-Stone Website Admin - correct company email (owner, 2026-11-03)
-- The website's Google Script already emails tristoneconstructionservices@gmail.com, which the owner confirmed
-- is the right address. The inquiry recipient and the email shown on the website were seeded (script 014) with
-- the shortened "tristoneconstructionserv@gmail.com"; fix them only if they still hold that value.
-- Run once. Safe to re-run.
-- =====================================================================

UPDATE website_content
   SET value = 'tristoneconstructionservices@gmail.com', updated_at = NOW()
 WHERE content_key IN ('inquiry.recipient_email', 'contact.email')
   AND value = 'tristoneconstructionserv@gmail.com';
