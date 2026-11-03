-- =====================================================================
-- Tri-Stone Website Admin - icon per service card (owner request 2026-11-03)
-- Any service can be typed (script 018); each card now picks one of the website's 3 service icons.
-- The icon also decides the website's own picture used when the card has none:
--   ruler  = Residential Construction look (Residential.jpg)
--   helmet = Commercial Construction look (commercial.jpg)
--   tools  = Repairs and Home Extension look (repairs.jpg)
-- Run once. Safe to re-run.
-- =====================================================================

ALTER TABLE website_services ADD COLUMN IF NOT EXISTS icon VARCHAR(20);

UPDATE website_services
   SET icon = CASE service_name
                  WHEN 'Commercial Construction'    THEN 'helmet'
                  WHEN 'Repairs and Home Extension' THEN 'tools'
                  ELSE 'ruler'
              END
 WHERE icon IS NULL;

ALTER TABLE website_services ALTER COLUMN icon SET DEFAULT 'ruler';
ALTER TABLE website_services ALTER COLUMN icon SET NOT NULL;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'chk_website_services_icon') THEN
        ALTER TABLE website_services ADD CONSTRAINT chk_website_services_icon CHECK (icon IN ('ruler', 'helmet', 'tools'));
    END IF;
END $$;
