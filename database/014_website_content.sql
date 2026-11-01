-- =====================================================================
-- Tri-Stone Website Admin - website content (owner decisions 2026-11-01, Figma "CCMS SUPERSYSTEM")
-- What the Website Admin editors change: Home hero, featured Projects (+ photos), Services, About Us,
-- Contact Us details. Save Changes writes these tables; Publish Website takes a snapshot of all of them
-- (website_publishes) for the live site. website_visits is the free visit counter (filled by the API later).
-- Images live in the shared files folder (FileStore); only their relative paths are stored here.
-- Run once. Safe to re-run.
-- =====================================================================

-- Single texts and images, one row per item (e.g. 'home.headline'). Seeded from the current website text.
CREATE TABLE IF NOT EXISTS website_content (
    content_key  VARCHAR(60)    PRIMARY KEY,
    value        VARCHAR(4000),                     -- text, or the image's relative path in the files folder
    updated_by   INT            REFERENCES users (user_id),
    updated_at   TIMESTAMP
);

INSERT INTO website_content (content_key, value) VALUES
    ('home.hero_image',       NULL),
    ('home.headline',         'QUALITY CONSTRUCTION BUILT AROUND YOUR NEEDS'),
    ('home.subheadline',      'Professional construction, renovation, and improvement services in the Philippines, delivering reliable workmanship and practical solutions for every project.'),
    ('about.1.image',         NULL),
    ('about.1.heading',       'ABOUT TRI-STONE'),
    ('about.1.text',          'Tri-stone construction services takes on the role of main constructor for small to medium size project. Providing a quality construction services includes a new construction, renovation, remodels, repairs, exterior / interior fit-outs, and residential houses.' || E'\n\n' || 'In particular we enjoy working with our clients and other members of project teams, to provide individual and innovative cost solution to each project brief.'),
    ('about.2.image',         NULL),
    ('about.2.heading',       'Our Principles'),
    ('about.2.text',          'In the pursuit of this goals, we place God above all. And we shall be guided by principles of teamwork, Integrity, discipline, excellence and safety.'),
    ('about.3.image',         NULL),
    ('about.3.heading',       'OUR VISION & MISSION'),
    ('about.3.vision',        'To be a responsible building constructor, delivering growth to our community beyond expectation.'),
    ('about.3.mission',       'We will strive to implement a long-term relationship with our client, based on quality, timely service, safety and anticipation of their needs. Dedication to providing quality construction services.'),
    ('contact.address',       '96, Maligaya St. Ibayo II Bagbag Novaliches Quezon City, Philippines'),
    ('contact.phone',         '0969-180-6929'),
    ('contact.email',         'tristoneconstructionserv@gmail.com'),
    ('contact.copyright',     'Copyright 2026 Tristone Construction - All Rights Reserved.'),
    ('inquiry.recipient_email', 'tristoneconstructionserv@gmail.com')
ON CONFLICT (content_key) DO NOTHING;

-- Featured projects: picked from the office Projects list (show_on_website). Name, site and sector come from there.
CREATE TABLE IF NOT EXISTS website_projects (
    web_project_id     SERIAL        PRIMARY KEY,
    project_id         INT           NOT NULL UNIQUE REFERENCES projects (project_id),
    sort_order         INT           NOT NULL DEFAULT 0,
    is_visible         BOOLEAN       NOT NULL DEFAULT TRUE,     -- Unpublish = hidden, kept here
    short_description  VARCHAR(100),
    created_by         INT           REFERENCES users (user_id),
    created_at         TIMESTAMP     NOT NULL DEFAULT NOW(),
    updated_at         TIMESTAMP
);

-- Up to 10 photos per featured project (checked by the app). Showcase photos are the ones in the website's carousel.
CREATE TABLE IF NOT EXISTS website_project_photos (
    photo_id        SERIAL        PRIMARY KEY,
    web_project_id  INT           NOT NULL REFERENCES website_projects (web_project_id) ON DELETE CASCADE,
    file_path       VARCHAR(500)  NOT NULL,
    file_name       VARCHAR(255)  NOT NULL,
    size_bytes      BIGINT        NOT NULL,
    sort_order      INT           NOT NULL DEFAULT 0,
    is_showcase     BOOLEAN       NOT NULL DEFAULT TRUE,
    uploaded_by     INT           REFERENCES users (user_id),
    uploaded_at     TIMESTAMP     NOT NULL DEFAULT NOW()
);
CREATE INDEX IF NOT EXISTS ix_website_project_photos_project ON website_project_photos (web_project_id, sort_order);

-- Service cards: one of the 3 fixed services (owner decision), with a sector, location, description and image.
CREATE TABLE IF NOT EXISTS website_services (
    web_service_id     SERIAL        PRIMARY KEY,
    service_name       VARCHAR(60)   NOT NULL
                       CHECK (service_name IN ('Residential Construction', 'Commercial Construction', 'Repairs and Home Extension')),
    sector             VARCHAR(30)   NOT NULL
                       CHECK (sector IN ('Residential', 'Commercial', 'Industrial', 'Repairs / Renovations')),  -- = ProjectSectors
    location           VARCHAR(100),
    short_description  VARCHAR(100),
    image_path         VARCHAR(500),
    sort_order         INT           NOT NULL DEFAULT 0,
    is_visible         BOOLEAN       NOT NULL DEFAULT TRUE,
    created_by         INT           REFERENCES users (user_id),
    created_at         TIMESTAMP     NOT NULL DEFAULT NOW(),
    updated_at         TIMESTAMP
);

INSERT INTO website_services (service_name, sector, short_description, sort_order)
SELECT v.name, v.sector, v.descr, v.ord
FROM (VALUES
    ('Residential Construction',   'Residential', 'Quality residential construction from planning to completion.', 1),
    ('Commercial Construction',    'Commercial',  'Quality commercial construction from planning to completion.', 2),
    ('Repairs and Home Extension', 'Repairs / Renovations', 'Repairs and home extensions done right.', 3)) AS v(name, sector, descr, ord)
WHERE NOT EXISTS (SELECT 1 FROM website_services);

-- Publish Website: a snapshot of all saved content at that moment (what the live site shows).
CREATE TABLE IF NOT EXISTS website_publishes (
    publish_id    SERIAL     PRIMARY KEY,
    published_at  TIMESTAMP  NOT NULL DEFAULT NOW(),
    published_by  INT        REFERENCES users (user_id),
    content       JSONB      NOT NULL
);

-- Website Traffic: one row per page visit, written by the API when the website reports a visit.
CREATE TABLE IF NOT EXISTS website_visits (
    visit_id    BIGSERIAL     PRIMARY KEY,
    visited_at  TIMESTAMP     NOT NULL DEFAULT NOW(),
    page        VARCHAR(100)
);
CREATE INDEX IF NOT EXISTS ix_website_visits_time ON website_visits (visited_at);
