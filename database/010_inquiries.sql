-- =====================================================================
-- Tri-Stone Website Admin app - Inquiries (website Contact form messages)
-- Each message from the website Contact form lands here (through the API, later).
-- Website Admin accounts follow it up: New -> Contacted -> Closed, with notes.
-- Run once. Safe to re-run.
-- =====================================================================

CREATE TABLE IF NOT EXISTS inquiries (
    inquiry_id        SERIAL        PRIMARY KEY,
    received_at       TIMESTAMP     NOT NULL DEFAULT NOW(),
    full_name         VARCHAR(100)  NOT NULL,
    email             VARCHAR(150),
    phone             VARCHAR(40),
    project_location  VARCHAR(255),
    service_required  VARCHAR(100),                           -- optional: type of construction needed
    message           VARCHAR(4000) NOT NULL,
    status            VARCHAR(12)   NOT NULL DEFAULT 'New'
                      CHECK (status IN ('New', 'Contacted', 'Closed')),
    notes             VARCHAR(2000),                          -- internal follow-up notes, never shown on the website
    updated_by        INT           REFERENCES website_accounts (account_id),
    updated_at        TIMESTAMP,

    -- the client must leave at least one way to reach them
    CONSTRAINT chk_inquiries_contact CHECK (email IS NOT NULL OR phone IS NOT NULL)
);

CREATE INDEX IF NOT EXISTS ix_inquiries_received ON inquiries (received_at DESC);
CREATE INDEX IF NOT EXISTS ix_inquiries_status ON inquiries (status);
