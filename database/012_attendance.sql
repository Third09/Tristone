-- =====================================================================
-- Tri-Stone Office App - Part 2: Attendance (DFD D2.3)
-- Scans come from the fingerprint scanner's USB export ("All Report.xls", sheet "Attendance Logs").
-- Run by setup-database.ps1. Safe to re-run.
-- =====================================================================

-- One row per uploaded scanner file.
CREATE TABLE IF NOT EXISTS attendance_imports (
    import_id       SERIAL        PRIMARY KEY,
    file_name       VARCHAR(255)  NOT NULL,
    period_from     DATE          NOT NULL,                 -- the date range printed in the file
    period_to       DATE          NOT NULL,
    scans_added     INT           NOT NULL DEFAULT 0,
    scans_skipped   INT           NOT NULL DEFAULT 0,       -- already imported before (same person, same minute)
    rows_rejected   INT           NOT NULL DEFAULT 0,       -- e.g. a scanner ID no employee has
    problems        TEXT,                                   -- one line per rejected row
    imported_by     INT           REFERENCES users (user_id),
    imported_at     TIMESTAMP     NOT NULL DEFAULT NOW()
);

-- Every single fingerprint scan. Uploading the same file twice adds nothing (unique person + time).
CREATE TABLE IF NOT EXISTS attendance_scans (
    scan_id         SERIAL        PRIMARY KEY,
    employee_id     INT           NOT NULL REFERENCES employees (employee_id),
    scan_time       TIMESTAMP     NOT NULL,
    import_id       INT           REFERENCES attendance_imports (import_id),
    UNIQUE (employee_id, scan_time)
);

-- One row per employee per work day, worked out from the scans (or fixed by HR with a reason).
-- Status: Present, Late, Missing Scan, Absent, Rest Day Work. Only complete days (Present / Late, or fixed)
-- are paid by Payroll.
CREATE TABLE IF NOT EXISTS attendance_days (
    day_id          SERIAL        PRIMARY KEY,
    employee_id     INT           NOT NULL REFERENCES employees (employee_id),
    work_date       DATE          NOT NULL,
    time_in         TIME,
    break_out       TIME,
    break_in        TIME,
    time_out        TIME,
    late_minutes    INT           NOT NULL DEFAULT 0,
    early_minutes   INT           NOT NULL DEFAULT 0,       -- left before 5 PM (undertime)
    ot_minutes      INT           NOT NULL DEFAULT 0,       -- after 5 PM; paid only when approved
    ot_status       VARCHAR(10)   NOT NULL DEFAULT 'None' CHECK (ot_status IN ('None', 'Waiting', 'Approved', 'Rejected')),
    status          VARCHAR(15)   NOT NULL CHECK (status IN ('Present', 'Late', 'Missing Scan', 'Absent', 'Rest Day Work')),
    fix_reason      VARCHAR(500),                           -- set when HR fixed the times by hand
    fixed_by        INT           REFERENCES users (user_id),
    fixed_at        TIMESTAMP,
    ot_decided_by   INT           REFERENCES users (user_id),
    ot_decided_at   TIMESTAMP,
    updated_at      TIMESTAMP     NOT NULL DEFAULT NOW(),
    UNIQUE (employee_id, work_date)
);
CREATE INDEX IF NOT EXISTS ix_attendance_days_date ON attendance_days (work_date);
