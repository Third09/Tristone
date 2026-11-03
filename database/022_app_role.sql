-- =====================================================================
-- Tri-Stone - the app's own limited database login (Phase 6 security, owner decision 2026-10-27)
-- The office PCs connect as tristone_app, never as postgres: it can read and change data,
-- but not create, change or drop tables (only setup-database, run as postgres, does that).
-- The role is made here WITHOUT a password and cannot sign in yet; on the office server,
-- server/install-server.sh gives it a password and turns sign-in on. On the home and work
-- PCs it stays unused (they keep using postgres on localhost).
-- Run once. Safe to re-run (also re-grants tables added by later scripts).
-- =====================================================================

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'tristone_app') THEN
        CREATE ROLE tristone_app NOLOGIN;
    END IF;
    EXECUTE format('GRANT CONNECT ON DATABASE %I TO tristone_app', current_database());
END $$;

GRANT USAGE ON SCHEMA public TO tristone_app;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO tristone_app;
GRANT USAGE, SELECT, UPDATE ON ALL SEQUENCES IN SCHEMA public TO tristone_app;

-- Tables and sequences that later scripts create (setup-database runs them as postgres) get the same rights.
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public
    GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO tristone_app;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public
    GRANT USAGE, SELECT, UPDATE ON SEQUENCES TO tristone_app;

-- The list of applied scripts is setup-database's business, not the app's.
REVOKE INSERT, UPDATE, DELETE ON schema_versions FROM tristone_app;
