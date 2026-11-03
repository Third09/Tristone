-- =====================================================================
-- Tri-Stone Website Admin - more service icons (owner request 2026-11-03)
-- Website Admin now offers 29 common construction services, each with its own icon (the website's 3 + 26 Tabler
-- Icons in Updated/images/service-icons). The list lives in the app (WebsiteServiceIcons), so the icon CHECK from
-- script 019 becomes "not blank", and the column gets room for longer icon names.
-- Run once. Safe to re-run.
-- =====================================================================

ALTER TABLE website_services DROP CONSTRAINT IF EXISTS chk_website_services_icon;
ALTER TABLE website_services ALTER COLUMN icon TYPE VARCHAR(40);

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'chk_website_services_icon_not_blank') THEN
        ALTER TABLE website_services ADD CONSTRAINT chk_website_services_icon_not_blank CHECK (btrim(icon) <> '');
    END IF;
END $$;
