-- =====================================================================
-- Tri-Stone Website Admin app - accounts and sign-in log
-- Database: construction_db (PostgreSQL 18)
-- The Website Admin app is a separate program with its OWN accounts,
-- separate from the office app's users (owner decision 2026-10-30).
-- Run once. Safe to re-run: every object uses IF NOT EXISTS.
-- =====================================================================

-- ---------------------------------------------------------------------
-- website_accounts: people who manage the public website
-- Same password and lock-out rules as the office app.
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS website_accounts (
    account_id           SERIAL       PRIMARY KEY,
    username             VARCHAR(50)  NOT NULL,
    password_hash        VARCHAR(255) NOT NULL,                 -- bcrypt, never plain text
    full_name            VARCHAR(100) NOT NULL,
    status               VARCHAR(20)  NOT NULL DEFAULT 'Active'
                         CHECK (status IN ('Active', 'Locked', 'Disabled')),
    failed_login_count   INT          NOT NULL DEFAULT 0,       -- locks at 5
    last_login_at        TIMESTAMP,
    created_at           TIMESTAMP    NOT NULL DEFAULT NOW(),
    updated_at           TIMESTAMP    NOT NULL DEFAULT NOW()
);

-- Usernames are unique ignoring capital letters ("Juan" and "juan" can't both exist).
CREATE UNIQUE INDEX IF NOT EXISTS ux_website_accounts_username ON website_accounts (LOWER(username));

-- ---------------------------------------------------------------------
-- website_login_logs: sign ins, sign outs, failed attempts, locks
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS website_login_logs (
    log_id          SERIAL       PRIMARY KEY,
    account_id      INT          REFERENCES website_accounts (account_id),   -- NULL when the username didn't exist
    username_tried  VARCHAR(50),
    action          VARCHAR(30)  NOT NULL
                    CHECK (action IN ('Login', 'Logout', 'Failed Login', 'Account Locked', 'Account Unlocked', 'Password Changed')),
    details         VARCHAR(255),
    computer_name   VARCHAR(100),
    logged_at       TIMESTAMP    NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS ix_website_login_logs_time ON website_login_logs (logged_at DESC);
