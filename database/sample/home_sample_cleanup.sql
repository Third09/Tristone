-- =====================================================================
-- Removes the sample data of home_sample_data.sql (and anything later recorded on the sample project,
-- materials, vendors, subcontractors or site workers). Run before moving real data to the office server
-- if the sample should not go along:   psql -1 -v ON_ERROR_STOP=1 -f home_sample_cleanup.sql
-- =====================================================================

CREATE TEMP TABLE s_project AS SELECT project_id FROM projects WHERE project_name = 'Aster Residences' AND client_name = 'Mr. and Mrs. Villareal';
CREATE TEMP TABLE s_vendor AS SELECT vendor_id FROM vendors WHERE name IN
  ('Bagong Lipunan Hardware & Construction Supply', 'Metro North Cement Trading', 'Rizal Steel & Rebar Supply', 'San Juan Tile Center');
CREATE TEMP TABLE s_material AS SELECT material_id FROM materials WHERE name IN
  ('Portland Cement 40kg', 'Deformed Bar 10mm x 6m', 'Washed Sand', 'Gravel 3/4', 'CHB 4 inch', 'Marine Plywood 3/4 x 4x8',
   'Gypsum Board 1/2 x 4x8', 'Ceramic Floor Tile 60x60', 'Latex Paint White', 'THHN Wire 3.5mm');
CREATE TEMP TABLE s_sub AS SELECT subcontractor_id FROM subcontractors WHERE business_name IN ('JRC Electrical Contractor', 'AquaFlow Plumbing Services');
CREATE TEMP TABLE s_worker AS SELECT employee_id FROM employees WHERE biometric_id IN ('201', '202', '203', '204', '205');
CREATE TEMP TABLE s_job AS SELECT job_id FROM sub_jobs WHERE project_id IN (SELECT * FROM s_project) OR subcontractor_id IN (SELECT * FROM s_sub);

-- website
DELETE FROM website_project_photos WHERE web_project_id IN (SELECT web_project_id FROM website_projects WHERE project_id IN (SELECT * FROM s_project));
DELETE FROM website_projects WHERE project_id IN (SELECT * FROM s_project);
DELETE FROM inquiries WHERE email IN ('pedro.manalo@example.com', 'sofia@cafe-sofia.example.com', 'luis.fernandez@example.com');
-- subcontractors
DELETE FROM sub_payouts WHERE job_id IN (SELECT * FROM s_job);
DELETE FROM sub_vouchers WHERE job_id IN (SELECT * FROM s_job);
DELETE FROM sub_progress WHERE job_id IN (SELECT * FROM s_job);
DELETE FROM sub_job_documents WHERE job_id IN (SELECT * FROM s_job);
DELETE FROM sub_job_stages WHERE job_id IN (SELECT * FROM s_job);
DELETE FROM sub_jobs WHERE job_id IN (SELECT * FROM s_job);
DELETE FROM subcontractor_trades WHERE subcontractor_id IN (SELECT * FROM s_sub);
DELETE FROM subcontractors WHERE subcontractor_id IN (SELECT * FROM s_sub);
-- receipts and change orders
DELETE FROM receipts WHERE project_id IN (SELECT * FROM s_project)
   OR (expense_type = 'Overhead' AND receipt_no IN ('BILL-10-26', 'BILL-IN-77', 'AR-2026-11'));
DELETE FROM change_orders WHERE project_id IN (SELECT * FROM s_project);
-- inventory
DELETE FROM material_usages WHERE project_id IN (SELECT * FROM s_project) OR material_id IN (SELECT * FROM s_material);
DELETE FROM deliveries WHERE material_id IN (SELECT * FROM s_material) OR vendor_id IN (SELECT * FROM s_vendor);
DELETE FROM purchases WHERE material_id IN (SELECT * FROM s_material) OR vendor_id IN (SELECT * FROM s_vendor);
DELETE FROM quotes WHERE material_id IN (SELECT * FROM s_material) OR vendor_id IN (SELECT * FROM s_vendor);
DELETE FROM vendor_documents WHERE vendor_id IN (SELECT * FROM s_vendor);
DELETE FROM vendors WHERE vendor_id IN (SELECT * FROM s_vendor);
DELETE FROM materials WHERE material_id IN (SELECT * FROM s_material);
DELETE FROM maintenance_logs WHERE equipment_id IN (SELECT equipment_id FROM equipment WHERE name IN
  ('Concrete Mixer 1-bagger', 'Steel Scaffolding Set (10 frames)', 'Mini Dump Truck'));
DELETE FROM equipment WHERE name IN ('Concrete Mixer 1-bagger', 'Steel Scaffolding Set (10 frames)', 'Mini Dump Truck');
-- HR: the site workers and the sample attendance of the department staff
DELETE FROM cash_advance_deductions WHERE advance_id IN (SELECT advance_id FROM cash_advances WHERE employee_id IN (SELECT * FROM s_worker));
DELETE FROM cash_advances WHERE employee_id IN (SELECT * FROM s_worker);
DELETE FROM payslips WHERE employee_id IN (SELECT * FROM s_worker);
DELETE FROM attendance_scans WHERE employee_id IN (SELECT * FROM s_worker);
DELETE FROM attendance_days WHERE employee_id IN (SELECT * FROM s_worker)
   OR (employee_id IN (SELECT employee_id FROM employees WHERE biometric_id IN ('11','12','13','14','15','16','17','18'))
       AND fix_reason IS NULL AND work_date >= CURRENT_DATE - 60);
DELETE FROM employee_rates WHERE employee_id IN (SELECT * FROM s_worker);
DELETE FROM employees WHERE employee_id IN (SELECT * FROM s_worker);
-- the project itself
DELETE FROM projects WHERE project_id IN (SELECT * FROM s_project);
