-- =====================================================================
-- One account list for both apps (owner decision 2026-10-30)
-- Website Admin becomes a Level in the office app's users (User Accounts),
-- next to Admin / Supervisor / Employee. One Sign In window (the Tri-Stone
-- launcher) opens the office app or the Website Admin app depending on it.
-- Existing website accounts (script 008) move into users with their
-- passwords, sign-in history and inquiry follow-ups; then the separate
-- website account tables are removed.
-- Run once, all or nothing.
-- =====================================================================

-- 1. The new level. Like Admins, Website Admins have no department.
ALTER TABLE users DROP CONSTRAINT IF EXISTS users_level_check;
ALTER TABLE users ADD CONSTRAINT users_level_check
    CHECK (level IN ('Admin', 'Supervisor', 'Employee', 'Website Admin'));

ALTER TABLE users DROP CONSTRAINT IF EXISTS chk_users_department;
ALTER TABLE users ADD CONSTRAINT chk_users_department
    CHECK ((level IN ('Admin', 'Website Admin') AND department IS NULL)
        OR (level IN ('Supervisor', 'Employee') AND department IS NOT NULL));

-- 2. Move the website accounts (only if script 008's table is still there).
DO $$
BEGIN
    IF to_regclass('website_accounts') IS NULL THEN RETURN; END IF;

    -- Same username as an office login (ignoring capitals): keep the office one, the website one gets "-web".
    CREATE TEMP TABLE website_move ON COMMIT DROP AS
    SELECT w.*,
           CASE WHEN EXISTS (SELECT 1 FROM users u WHERE LOWER(u.username) = LOWER(w.username))
                THEN LEFT(w.username, 46) || '-web' ELSE w.username END AS new_username,
           NULL::INT AS new_user_id
    FROM website_accounts w;

    INSERT INTO users (username, password_hash, full_name, level, department, status, failed_login_count,
                       last_login_at, created_at, updated_at)
    SELECT new_username, password_hash, full_name, 'Website Admin', NULL, status, failed_login_count,
           last_login_at, created_at, updated_at
    FROM website_move;

    UPDATE website_move m SET new_user_id = u.user_id FROM users u WHERE u.username = m.new_username;

    -- Inquiry follow-ups now point at the user.
    ALTER TABLE inquiries DROP CONSTRAINT IF EXISTS inquiries_updated_by_fkey;
    UPDATE inquiries i SET updated_by = m.new_user_id FROM website_move m WHERE i.updated_by = m.account_id;
    UPDATE inquiries SET updated_by = NULL WHERE updated_by IS NOT NULL AND updated_by NOT IN (SELECT user_id FROM users);

    -- Sign-in history joins the office log (Dashboard > Log History).
    INSERT INTO login_logs (user_id, username_tried, action, details, computer_name, logged_at)
    SELECT m.new_user_id, l.username_tried, l.action,
           LEFT(TRIM('Website Admin app. ' || COALESCE(l.details, '')), 255), l.computer_name, l.logged_at
    FROM website_login_logs l LEFT JOIN website_move m ON m.account_id = l.account_id;

    DROP TABLE website_login_logs;
    DROP TABLE website_accounts;
END $$;

-- 3. Inquiry follow-ups are made by office users now.
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'inquiries_updated_by_users_fkey') THEN
        ALTER TABLE inquiries DROP CONSTRAINT IF EXISTS inquiries_updated_by_fkey;
        ALTER TABLE inquiries ADD CONSTRAINT inquiries_updated_by_users_fkey
            FOREIGN KEY (updated_by) REFERENCES users (user_id);
    END IF;
END $$;
