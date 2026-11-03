-- =====================================================================
-- Tri-Stone Website Admin - any service name (owner request 2026-11-03)
-- Website Admin > Services used to offer only 3 fixed services (script 014). Now any service can be typed;
-- the 3 original ones are still offered in the list. A typed new service must have its own picture (checked by
-- the app, WebServiceService).
-- Run once. Safe to re-run.
-- =====================================================================

ALTER TABLE website_services DROP CONSTRAINT IF EXISTS website_services_service_name_check;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'chk_website_services_name_not_blank') THEN
        ALTER TABLE website_services ADD CONSTRAINT chk_website_services_name_not_blank CHECK (btrim(service_name) <> '');
    END IF;
END $$;
