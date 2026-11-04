-- =====================================================================
-- Tri-Stone Demo: made-up sample data, loaded on top of all the numbered database scripts by build-demo-data.ps1.
-- Every person, company, email and number here is invented. Dates are relative to the day the Demo is built.
-- All logins use the public password Demo1234 (below: its bcrypt hash; demo only, never used on real data).
-- =====================================================================
BEGIN;

-- ---------- People ----------
INSERT INTO employees (employee_id, full_name, contact_number, address, position, department, pay_schedule, biometric_id, gov_deductions, status) VALUES
 (1,  'Maria Santos',     '09170000001', 'Quezon City',  'General Manager',        'Administration', 'Office', '101', TRUE,  'Active'),
 (2,  'Ramon Dizon',      '09170000002', 'Quezon City',  'Website Coordinator',    'Administration', 'Office', '102', FALSE, 'Active'),
 (3,  'Liza Reyes',       '09170000003', 'Caloocan',     'HR Supervisor',          'HR & Payroll',   'Office', '103', TRUE,  'Active'),
 (4,  'Paolo Cruz',       '09170000004', 'Caloocan',     'HR Staff',               'HR & Payroll',   'Office', '104', FALSE, 'Active'),
 (5,  'Grace Mendoza',    '09170000005', 'Valenzuela',   'Inventory Supervisor',   'Inventory',      'Office', '105', TRUE,  'Active'),
 (6,  'Mark Villanueva',  '09170000006', 'Valenzuela',   'Inventory Staff',        'Inventory',      'Office', '106', FALSE, 'Active'),
 (7,  'Ana Bautista',     '09170000007', 'Marikina',     'Subcontracts Supervisor','Subcontractors', 'Office', '107', TRUE,  'Active'),
 (8,  'Jose Ramos',       '09170000008', 'Marikina',     'Subcontracts Staff',     'Subcontractors', 'Office', '108', FALSE, 'Active'),
 (9,  'Carla Navarro',    '09170000009', 'Pasig',        'Receipts Supervisor',    'Receipts',       'Office', '109', TRUE,  'Active'),
 (10, 'Noel Garcia',      '09170000010', 'Pasig',        'Receipts Staff',         'Receipts',       'Office', '110', FALSE, 'Active'),
 (11, 'Ben Aquino',       '09170000011', 'Novaliches',   'Foreman',                'Site',           'Site',   '201', FALSE, 'Active'),
 (12, 'Rico Flores',      '09170000012', 'Novaliches',   'Mason',                  'Site',           'Site',   '202', FALSE, 'Active'),
 (13, 'Dan Castillo',     '09170000013', 'Fairview',     'Carpenter',              'Site',           'Site',   '203', FALSE, 'Active'),
 (14, 'Eddie Torres',     '09170000014', 'Fairview',     'Electrician',            'Site',           'Site',   '204', FALSE, 'Active'),
 (15, 'Nestor Lim',       '09170000015', 'Commonwealth', 'Laborer',                'Site',           'Site',   '205', FALSE, 'Active');

INSERT INTO employee_rates (employee_id, daily_rate, effective_from)
SELECT employee_id, CASE WHEN employee_id = 1 THEN 2200 WHEN employee_id IN (3,5,7,9) THEN 1400
                         WHEN employee_id = 11 THEN 1100 WHEN employee_id BETWEEN 12 AND 14 THEN 800
                         WHEN employee_id = 15 THEN 650 ELSE 900 END,
       CURRENT_DATE - 365
FROM employees;

INSERT INTO users (user_id, employee_id, username, password_hash, full_name, level, department, status)
SELECT v.id, v.emp, v.username, '$2a$11$jh4NM2zSLlIM3GqB1Jj/D.TWOZWPBWV7GOc0GxUyGQIHiONomp6Qi', e.full_name, v.level, v.dept, 'Active'
FROM (VALUES
 (1,  1,  'demo.admin',   'Admin',         NULL),
 (2,  2,  'demo.website', 'Website Admin', NULL),
 (3,  3,  'hr.sup',       'Supervisor',    'HR & Payroll'),
 (4,  4,  'hr.emp',       'Employee',      'HR & Payroll'),
 (5,  5,  'inv.sup',      'Supervisor',    'Inventory'),
 (6,  6,  'inv.emp',      'Employee',      'Inventory'),
 (7,  7,  'sub.sup',      'Supervisor',    'Subcontractors'),
 (8,  8,  'sub.emp',      'Employee',      'Subcontractors'),
 (9,  9,  'rec.sup',      'Supervisor',    'Receipts'),
 (10, 10, 'rec.emp',      'Employee',      'Receipts')) AS v(id, emp, username, level, dept)
JOIN employees e ON e.employee_id = v.emp;

-- ---------- Projects ----------
INSERT INTO projects (project_id, project_name, site_location, client_name, sector, contract_price, start_date, target_end_date, completed_date, status, show_on_website, created_by) VALUES
 (1, 'Dela Paz Family Home',      'Antipolo City',  'Mr. and Mrs. Dela Paz', 'Residential',           4850000,  CURRENT_DATE - 300, CURRENT_DATE - 60,  CURRENT_DATE - 70, 'Done',    TRUE,  9),
 (2, 'Sunrise Bakery Fit-out',    'Quezon City',    'Sunrise Bakery Inc.',   'Commercial',            1650000,  CURRENT_DATE - 210, CURRENT_DATE - 120, CURRENT_DATE - 125,'Done',    TRUE,  9),
 (3, 'Harbor Logistics Warehouse','Valenzuela City','Harbor Logistics Corp.','Industrial',            12400000, CURRENT_DATE - 150, CURRENT_DATE + 120, NULL,              'Ongoing', TRUE,  9),
 (4, 'Mabini Townhouses (4 units)','Marikina City', 'Mabini Realty',         'Residential',           9200000,  CURRENT_DATE - 90,  CURRENT_DATE + 200, NULL,              'Ongoing', FALSE, 9),
 (5, 'St. Jude Clinic Renovation','Caloocan City',  'St. Jude Clinic',       'Repairs / Renovations', 980000,   CURRENT_DATE - 25,  CURRENT_DATE + 45,  NULL,              'Ongoing', FALSE, 9),
 (6, 'Rosario Roof Repair',       'Pasig City',     'Mrs. Rosario',          'Repairs / Renovations', 320000,   CURRENT_DATE + 15,  CURRENT_DATE + 40,  NULL,              'Pending', FALSE, 9);

INSERT INTO change_orders (change_order_id, co_no, project_id, requested_on, description, cost_change, days_change, status, created_by, decided_by, decided_at) VALUES
 (1, 'CO-001', 1, CURRENT_DATE - 180, 'Added a covered carport',              185000, 10, 'Approved', 10, 9, NOW() - INTERVAL '178 days'),
 (2, 'CO-002', 3, CURRENT_DATE - 40,  'Thicker floor slab for forklift loads', 640000, 14, 'Approved', 10, 9, NOW() - INTERVAL '38 days'),
 (3, 'CO-003', 4, CURRENT_DATE - 3,   'Upgrade kitchen counters to granite',   96000,  0,  'Waiting',  10, NULL, NULL);

-- ---------- Inventory ----------
INSERT INTO vendors (vendor_id, name, contact_person, phone, email, address, vat_status, created_by) VALUES
 (1, 'Sample Hardware Depot',  'Tony Uy',     '02-8000-0001', 'sales@hardware.example.com',  'Quezon City',     'VAT',     6),
 (2, 'Demo Cement Trading',    'Mely Tan',    '02-8000-0002', 'orders@cement.example.com',   'Valenzuela City', 'VAT',     6),
 (3, 'Example Steel Supply',   'Rey Chua',    '02-8000-0003', 'info@steel.example.com',      'Caloocan City',   'Non-VAT', 6);

INSERT INTO materials (material_id, name, category, unit, unit_price, stock_qty, minimum_stock, created_by) VALUES
 (1, 'Portland Cement 40kg',   'Cement',     'bag',   245,  320, 100, 6),
 (2, 'Deformed Bar 12mm x 6m', 'Steel',      'piece', 235,  180, 200, 6),
 (3, 'Washed Sand',            'Aggregates', 'cu.m',  1350, 42,  15,  6),
 (4, 'Gravel 3/4',             'Aggregates', 'cu.m',  1450, 12,  15,  6),
 (5, 'CHB 4 inch',             'Masonry',    'piece', 16,   4200, 1000, 6),
 (6, 'Plywood 1/2 x 4x8',      'Lumber',     'sheet', 690,  65,  20,  6),
 (7, 'THHN Wire 3.5mm',        'Electrical', 'roll',  3150, 14,  5,   6),
 (8, 'Latex Paint White',      'Paint',      'gallon',780,  28,  10,  6);

INSERT INTO deliveries (delivery_id, vendor_id, material_id, quantity, unit_price, delivered_on, receipt_no, physical_only, filed_location, recorded_by) VALUES
 (1, 2, 1, 400, 245, CURRENT_DATE - 40, 'DR-1001', TRUE, 'Cabinet A, Folder 1', 6),
 (2, 3, 2, 300, 235, CURRENT_DATE - 35, 'DR-1002', TRUE, 'Cabinet A, Folder 1', 6),
 (3, 1, 7, 15,  3150, CURRENT_DATE - 20, 'DR-1003', TRUE, 'Cabinet A, Folder 2', 6),
 (4, 1, 8, 30,  780,  CURRENT_DATE - 12, 'DR-1004', TRUE, 'Cabinet A, Folder 2', 6);

INSERT INTO material_usages (usage_id, material_id, project_id, quantity, used_on, notes, status, cost, requested_by, decided_by, decided_at) VALUES
 (1, 1, 3, 180, CURRENT_DATE - 30, 'Warehouse footings', 'Approved', 44100, 6, 5, NOW() - INTERVAL '29 days'),
 (2, 2, 3, 120, CURRENT_DATE - 28, 'Warehouse columns',  'Approved', 28200, 6, 5, NOW() - INTERVAL '27 days'),
 (3, 5, 4, 1800,CURRENT_DATE - 15, 'Townhouse walls',    'Approved', 28800, 6, 5, NOW() - INTERVAL '14 days'),
 (4, 8, 5, 6,   CURRENT_DATE - 2,  'Clinic waiting room','Waiting',  4680,  6, NULL, NULL);

INSERT INTO equipment (equipment_id, name, type, ownership, rental_company, rental_rate, rental_rate_unit, return_date, status, next_maintenance, created_by) VALUES
 (1, 'Concrete Mixer 1-bagger', 'Tool',            'Owned',  NULL, NULL, NULL, NULL, 'Available',    CURRENT_DATE + 20, 6),
 (2, 'Mini Dump Truck',         'Vehicle',         'Owned',  NULL, NULL, NULL, NULL, 'Under Repair', CURRENT_DATE - 5,  6),
 (3, 'Backhoe',                 'Heavy Equipment', 'Rented', 'Sample Equipment Rentals', 18000, 'Day', CURRENT_DATE + 10, 'Available', NULL, 6);

INSERT INTO maintenance_logs (equipment_id, done_on, kind, work_done, cost, done_by, recorded_by) VALUES
 (1, CURRENT_DATE - 70, 'Preventive', 'Changed oil, greased the drum', 850,  'Site team',     6),
 (2, CURRENT_DATE - 6,  'Repair',     'Replaced the clutch',           14500,'Demo Auto Shop',6);

-- ---------- Subcontractors ----------
INSERT INTO subcontractors (subcontractor_id, business_name, trade_name, primary_name, primary_role, primary_mobile, primary_email, office_address, pcab_license_no, pcab_classification, pcab_expiry, created_by) VALUES
 (1, 'Sample Electrical Works',  'SEW',  'Arnel Sy',    'Owner',           '09180000001', 'arnel@sew.example.com',   'Quezon City', 'PCAB-00001', 'Category C', CURRENT_DATE + 200, 8),
 (2, 'Demo Plumbing Services',   'DPS',  'Rowena Lim',  'Project Manager', '09180000002', 'rowena@dps.example.com',  'Caloocan',    'PCAB-00002', 'Category D', CURRENT_DATE + 25,  8),
 (3, 'Example Steel Fabricators','ESF',  'Gino Ong',    'Owner',           '09180000003', 'gino@esf.example.com',    'Valenzuela',  'PCAB-00003', 'Category B', CURRENT_DATE + 400, 8);

INSERT INTO subcontractor_trades (subcontractor_id, trade) VALUES
 (1, 'Electrical Rough-ins & Wiring'), (2, 'Plumbing & Sanitary'), (3, 'Structural Steel');

INSERT INTO sub_jobs (job_id, subcontractor_id, project_id, scope, contract_amount, start_date, end_date, retention_pct, created_by) VALUES
 (1, 3, 3, 'Warehouse roof trusses and purlins', 2400000, CURRENT_DATE - 100, CURRENT_DATE + 60, 10, 8),
 (2, 1, 1, 'Whole-house electrical',              420000, CURRENT_DATE - 240, CURRENT_DATE - 90, 10, 8),
 (3, 2, 4, 'Plumbing rough-ins, 4 units',         610000, CURRENT_DATE - 60,  CURRENT_DATE + 90, 10, 8);

INSERT INTO sub_job_stages (stage_id, job_id, stage_no, name, percent, reached_on, reached_by) VALUES
 (1, 1, 1, 'Mobilization',     30, CURRENT_DATE - 90, 8),
 (2, 1, 2, 'Trusses installed',40, NULL, NULL),
 (3, 1, 3, 'Completion',       30, NULL, NULL),
 (4, 2, 1, 'Rough-in',         50, CURRENT_DATE - 180, 8),
 (5, 2, 2, 'Completion',       50, CURRENT_DATE - 95, 8),
 (6, 3, 1, 'Rough-in',         60, CURRENT_DATE - 5, 8),
 (7, 3, 2, 'Completion',       40, NULL, NULL);

INSERT INTO sub_progress (job_id, percent, progress_date, notes, created_by) VALUES
 (1, 45, CURRENT_DATE - 10, 'Half of the trusses are up', 8),
 (2, 100, CURRENT_DATE - 95, 'Finished and checked', 8),
 (3, 55, CURRENT_DATE - 5, 'Rough-ins done on units 1 to 3', 8);

INSERT INTO sub_vouchers (voucher_id, voucher_no, job_id, stage_id, kind, gross_amount, retention_amount, payable_amount, status, prepared_by, decided_by, decided_at) VALUES
 (1, 'SV-0001', 1, 1, 'Stage', 720000, 72000, 648000, 'Paid',     8, 7, NOW() - INTERVAL '88 days'),
 (2, 'SV-0002', 2, 4, 'Stage', 210000, 21000, 189000, 'Paid',     8, 7, NOW() - INTERVAL '178 days'),
 (3, 'SV-0003', 2, 5, 'Stage', 210000, 21000, 189000, 'Paid',     8, 7, NOW() - INTERVAL '93 days'),
 (4, 'SV-0004', 3, 6, 'Stage', 366000, 36600, 329400, 'Waiting',  8, NULL, NULL);

INSERT INTO sub_payouts (voucher_id, job_id, paid_on, amount, reference_no, method, created_by) VALUES
 (1, 1, CURRENT_DATE - 86,  648000, 'BT-20001', 'Bank Transfer', 8),
 (2, 2, CURRENT_DATE - 176, 189000, 'CHK-3001', 'Check',         8),
 (3, 2, CURRENT_DATE - 91,  189000, 'CHK-3002', 'Check',         8);

-- ---------- Receipts ----------
INSERT INTO receipts (receipt_no, purchase_date, merchant, amount, category, expense_type, project_id, physical_only, filed_location, no_receipt, no_receipt_reason, status, uploaded_by, decided_by, decided_at) VALUES
 ('OR-5001', CURRENT_DATE - 200, 'Sample Hardware Depot', 386000, 'Materials',        'Project',  1, TRUE,  'Cabinet B, 2026',  FALSE, NULL, 'Approved', 10, 9, NOW() - INTERVAL '199 days'),
 ('OR-5002', CURRENT_DATE - 160, 'Demo Tile Center',      512500, 'Materials',        'Project',  1, TRUE,  'Cabinet B, 2026',  FALSE, NULL, 'Approved', 10, 9, NOW() - INTERVAL '159 days'),
 ('OR-5003', CURRENT_DATE - 190, 'Example Glass House',   248000, 'Materials',        'Project',  2, TRUE,  'Cabinet B, 2026',  FALSE, NULL, 'Approved', 10, 9, NOW() - INTERVAL '189 days'),
 ('OR-5004', CURRENT_DATE - 60,  'Sample Hardware Depot', 174300, 'Tools',            'Project',  3, TRUE,  'Cabinet B, 2026',  FALSE, NULL, 'Approved', 10, 9, NOW() - INTERVAL '59 days'),
 ('OR-5005', CURRENT_DATE - 20,  'City Permits Office',   38500,  'Permits',          'Project',  4, TRUE,  'Cabinet B, 2026',  FALSE, NULL, 'Approved', 10, 9, NOW() - INTERVAL '19 days'),
 ('OR-5006', CURRENT_DATE - 2,   'Demo Lumber Yard',      22750,  'Materials',        'Project',  5, TRUE,  'Tray on the desk', FALSE, NULL, 'Waiting',  10, NULL, NULL),
 ('OR-5007', CURRENT_DATE - 15,  'Example Power Company', 18640,  'Utilities',        'Overhead', NULL, TRUE, 'Cabinet C, Bills', FALSE, NULL, 'Approved', 10, 9, NOW() - INTERVAL '14 days'),
 ('OR-5008', CURRENT_DATE - 9,   'Sample Office Rentals', 45000,  'Rent',             'Overhead', NULL, TRUE, 'Cabinet C, Bills', FALSE, NULL, 'Approved', 10, 9, NOW() - INTERVAL '8 days'),
 (NULL,      CURRENT_DATE - 1,   'Tricycle fare to site', 450,    'Transport',        'Project',  5, FALSE, NULL, TRUE, 'Driver gave no receipt', 'Waiting', 10, NULL, NULL);

-- ---------- HR: the last 3 weeks of attendance (Sundays off; a few late, one missing scan, one overtime waiting) ----------
INSERT INTO attendance_days (employee_id, work_date, time_in, break_out, break_in, time_out, late_minutes, early_minutes, ot_minutes, ot_status, status)
SELECT e.employee_id, d::date,
       CASE WHEN (e.employee_id + EXTRACT(DOY FROM d)::int) % 11 = 0 THEN TIME '08:32' ELSE TIME '07:55' END,
       TIME '12:01', TIME '12:58',
       CASE WHEN e.employee_id = 13 AND d::date = CURRENT_DATE - 2 THEN NULL
            WHEN e.employee_id = 11 AND d::date = CURRENT_DATE - 3 THEN TIME '19:00'
            ELSE TIME '17:03' END,
       CASE WHEN (e.employee_id + EXTRACT(DOY FROM d)::int) % 11 = 0 THEN 17 ELSE 0 END,
       0,
       CASE WHEN e.employee_id = 11 AND d::date = CURRENT_DATE - 3 THEN 120 ELSE 0 END,
       CASE WHEN e.employee_id = 11 AND d::date = CURRENT_DATE - 3 THEN 'Waiting' ELSE 'None' END,
       CASE WHEN e.employee_id = 13 AND d::date = CURRENT_DATE - 2 THEN 'Missing Scan'
            WHEN (e.employee_id + EXTRACT(DOY FROM d)::int) % 11 = 0 THEN 'Late'
            ELSE 'Present' END
FROM employees e
CROSS JOIN generate_series(CURRENT_DATE - 21, CURRENT_DATE - 1, INTERVAL '1 day') AS d
WHERE EXTRACT(DOW FROM d) <> 0;

INSERT INTO cash_advances (employee_id, given_on, amount, per_payday, balance, reason, status, created_by, decided_by, decided_at) VALUES
 (12, CURRENT_DATE - 20, 3000, 500, 3000, 'Family emergency', 'Active',  4, 3, NOW() - INTERVAL '19 days'),
 (14, CURRENT_DATE - 1,  2000, 500, 2000, 'School fees',      'Waiting', 4, NULL, NULL);

-- ---------- Website ----------
INSERT INTO website_projects (project_id, sort_order, is_visible, short_description, created_by) VALUES
 (1, 1, TRUE, 'A two-storey family home, finished on schedule.', 2),
 (2, 2, TRUE, 'A full bakery fit-out, from counters to kitchen.', 2),
 (3, 3, TRUE, 'A warehouse built for heavy forklift loads.', 2);

INSERT INTO inquiries (received_at, full_name, email, phone, project_location, service_required, message, status, notes) VALUES
 (NOW() - INTERVAL '2 hours', 'Pedro Example',  'pedro@example.com', '09190000001', 'Bulacan',     'Residential Construction',   'We plan to build a bungalow next year. How much per square meter?', 'New', NULL),
 (NOW() - INTERVAL '1 day',   'Sofia Sample',   'sofia@example.com', '09190000002', 'Makati City', 'Commercial Construction',    'Do you do office fit-outs? About 200 square meters.', 'New', NULL),
 (NOW() - INTERVAL '5 days',  'Luis Demo',      'luis@example.com',  '09190000003', 'Taguig City', 'Repairs and Home Extension', 'Our roof leaks when it rains. Can someone check it?', 'Contacted', 'Site visit set for next Monday.'),
 (NOW() - INTERVAL '20 days', 'Tess Testcase',  'tess@example.com',  '09190000004', NULL,          'Other',                      'Thank you for the quote. We will contact you again next year.', 'Closed', 'Client postponed.');

INSERT INTO website_visits (visited_at, page)
SELECT NOW() - (n || ' hours')::interval, '/' FROM generate_series(1, 86) AS n
WHERE NOW() - (n || ' hours')::interval >= date_trunc('month', NOW());

-- ---------- Sequences continue after the rows above ----------
DO $$
DECLARE r record;
BEGIN
    FOR r IN SELECT c.table_name, c.column_name, pg_get_serial_sequence(quote_ident(c.table_name), c.column_name) AS seq
             FROM information_schema.columns c
             WHERE c.table_schema = 'public' AND c.column_default LIKE 'nextval%'
    LOOP
        EXECUTE format('SELECT setval(%L, GREATEST((SELECT COALESCE(MAX(%I), 0) FROM %I), 1))', r.seq, r.column_name, r.table_name);
    END LOOP;
END $$;

COMMIT;
