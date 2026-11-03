-- =====================================================================
-- Tri-Stone Website Admin - About Us sections as a list (owner request 2026-11-03)
-- About Us used to have 3 fixed sections in website_content (about.1.* / about.2.* / about.3.*).
-- Now it has any number of sections: add, move up/down, delete (at least one stays; the app checks).
-- Each section is one row of the website's About Us: a picture, a heading and its text, and optionally a
-- second heading and text in the same row (like OUR VISION / OUR MISSION).
-- The 3 current sections are copied over, then their old website_content rows are removed.
-- Run once. Safe to re-run.
-- =====================================================================

CREATE TABLE IF NOT EXISTS website_about_sections (
    about_section_id  SERIAL         PRIMARY KEY,
    sort_order        INT            NOT NULL DEFAULT 0,
    heading           VARCHAR(100)   NOT NULL,
    body              VARCHAR(2000),
    heading2          VARCHAR(100),                     -- second heading and text: both empty = one part only
    body2             VARCHAR(2000),
    image_path        VARCHAR(500),                     -- relative path in the files folder; empty = website keeps its picture
    created_by        INT            REFERENCES users (user_id),
    created_at        TIMESTAMP      NOT NULL DEFAULT NOW(),
    updated_by        INT            REFERENCES users (user_id),
    updated_at        TIMESTAMP
);

-- Copy the 3 fixed sections (with any edits already saved). Section 3's two parts are the website's
-- OUR VISION and OUR MISSION labels; its old overall heading was never shown on the website.
INSERT INTO website_about_sections (sort_order, heading, body, heading2, body2, image_path, updated_by, updated_at)
SELECT s.ord, s.heading, s.body, s.heading2, s.body2,
       (SELECT value FROM website_content WHERE content_key = 'about.' || s.ord || '.image'),
       (SELECT updated_by FROM website_content WHERE content_key LIKE 'about.' || s.ord || '.%' AND updated_at IS NOT NULL ORDER BY updated_at DESC LIMIT 1),
       (SELECT MAX(updated_at) FROM website_content WHERE content_key LIKE 'about.' || s.ord || '.%')
FROM (
    SELECT 1 AS ord, COALESCE((SELECT value FROM website_content WHERE content_key = 'about.1.heading'), 'ABOUT TRI-STONE') AS heading,
           (SELECT value FROM website_content WHERE content_key = 'about.1.text') AS body, NULL AS heading2, NULL AS body2
    UNION ALL
    SELECT 2, COALESCE((SELECT value FROM website_content WHERE content_key = 'about.2.heading'), 'OUR PRINCIPLES'),
           (SELECT value FROM website_content WHERE content_key = 'about.2.text'), NULL, NULL
    UNION ALL
    SELECT 3, 'OUR VISION', (SELECT value FROM website_content WHERE content_key = 'about.3.vision'),
           'OUR MISSION', (SELECT value FROM website_content WHERE content_key = 'about.3.mission')
) s
WHERE NOT EXISTS (SELECT 1 FROM website_about_sections);

DELETE FROM website_content WHERE content_key LIKE 'about.%';
