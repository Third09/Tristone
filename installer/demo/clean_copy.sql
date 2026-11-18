-- =====================================================================
-- Tri-Stone copy of this PC's data (owner requests 2026-11-17 / 2026-11-18): run by build.ps1 -FromThisPc on a COPY
-- of this PC's database (zz_demo_build), never on the real one, for both the Test Setup and the Standalone Setup.
-- It removes the throwaway test entries (gibberish projects, materials, vendor, employee, inquiries, service,
-- quotations and test vendor prices), clears the sign-in / edit history and password requests, and takes the
-- owner's own government ID numbers out (the file is given to other people). Logins and passwords stay as they are.
-- =====================================================================
BEGIN;

-- ---------- 1. throwaway test entries ----------
CREATE TEMP TABLE junk_projects AS SELECT project_id FROM projects WHERE project_name IN ('12312312', 'tesing quote', 'tesigng 234');
CREATE TEMP TABLE junk_materials AS SELECT material_id FROM materials WHERE name IN ('weqyrhgqwe', 'testing mats', 'testing mats 2');

DELETE FROM quotations WHERE project_id IN (SELECT project_id FROM junk_projects)
                          OR client_name IN ('testing customer', 'tesigng2');          -- scope and cost lines go with them
DELETE FROM website_project_photos WHERE web_project_id IN (SELECT web_project_id FROM website_projects WHERE project_id IN (SELECT project_id FROM junk_projects));
DELETE FROM website_projects WHERE project_id IN (SELECT project_id FROM junk_projects);
DELETE FROM material_usages WHERE project_id IN (SELECT project_id FROM junk_projects) OR material_id IN (SELECT material_id FROM junk_materials);
DELETE FROM receipts WHERE project_id IN (SELECT project_id FROM junk_projects);
DELETE FROM projects WHERE project_id IN (SELECT project_id FROM junk_projects);

DELETE FROM purchases WHERE material_id IN (SELECT material_id FROM junk_materials) OR vendor_id IN (SELECT vendor_id FROM vendors WHERE name = '1223');
DELETE FROM quotes    WHERE material_id IN (SELECT material_id FROM junk_materials) OR vendor_id IN (SELECT vendor_id FROM vendors WHERE name = '1223');
DELETE FROM deliveries WHERE material_id IN (SELECT material_id FROM junk_materials) OR vendor_id IN (SELECT vendor_id FROM vendors WHERE name = '1223');
DELETE FROM vendor_materials;                                                            -- the test prices; real lists below
DELETE FROM materials WHERE material_id IN (SELECT material_id FROM junk_materials);
DELETE FROM vendor_documents WHERE vendor_id IN (SELECT vendor_id FROM vendors WHERE name = '1223');
DELETE FROM vendors WHERE name = '1223';

DELETE FROM payslips         WHERE employee_id IN (SELECT employee_id FROM employees WHERE full_name = 'testing emp');
DELETE FROM attendance_days  WHERE employee_id IN (SELECT employee_id FROM employees WHERE full_name = 'testing emp');
DELETE FROM attendance_scans WHERE employee_id IN (SELECT employee_id FROM employees WHERE full_name = 'testing emp');
DELETE FROM cash_advances    WHERE employee_id IN (SELECT employee_id FROM employees WHERE full_name = 'testing emp');
DELETE FROM employee_rates   WHERE employee_id IN (SELECT employee_id FROM employees WHERE full_name = 'testing emp');
DELETE FROM employees WHERE full_name = 'testing emp' AND employee_id NOT IN (SELECT employee_id FROM users WHERE employee_id IS NOT NULL);

DELETE FROM quotations WHERE inquiry_id IN (SELECT inquiry_id FROM inquiries WHERE full_name IN ('testing', 'terron mapogi'));
DELETE FROM inquiries WHERE full_name IN ('testing', 'terron mapogi');
DELETE FROM website_services WHERE service_name = 'testing service';

-- ---------- 2. history ----------
DELETE FROM audit_logs;
DELETE FROM login_logs;
DELETE FROM password_requests;
-- the owner's own employee record: no personal ID numbers in a file given to others
UPDATE employees SET sss_no = NULL, philhealth_no = NULL, pagibig_no = NULL, tin = NULL
 WHERE employee_id IN (SELECT employee_id FROM users WHERE username IN ('ITmorilla', 'yumi'));

COMMIT;
