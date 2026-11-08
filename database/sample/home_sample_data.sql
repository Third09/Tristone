-- =====================================================================
-- Tri-Stone: sample company data for the HOME PC (owner request 2026-11-08).
-- One complete project, "Aster Residences", worked through every department, on top of the
-- existing test logins (hr_emp/hr_sup, inv_emp/inv_sup, sub_emp/sub_sup, rec_emp/rec_sup).
-- Every name, company, email and number is made up. Dates are relative to the day it is loaded.
-- Run once:  psql -1 -v ON_ERROR_STOP=1 -f home_sample_data.sql      Remove: home_sample_cleanup.sql
-- Not a numbered script: setup-database never runs it.
-- =====================================================================

-- ---------- Who records what (the existing test logins) ----------
SELECT user_id AS rec_sup FROM users WHERE username = 'rec_sup' \gset
SELECT user_id AS rec_emp FROM users WHERE username = 'rec_emp' \gset
SELECT user_id AS inv_sup FROM users WHERE username = 'inv_sup' \gset
SELECT user_id AS inv_emp FROM users WHERE username = 'inv_emp' \gset
SELECT user_id AS sub_sup FROM users WHERE username = 'sub_sup' \gset
SELECT user_id AS sub_emp FROM users WHERE username = 'sub_emp' \gset
SELECT user_id AS hr_sup  FROM users WHERE username = 'hr_sup'  \gset
SELECT user_id AS hr_emp  FROM users WHERE username = 'hr_emp'  \gset
SELECT user_id AS web     FROM users WHERE username = 'Webadmin1' \gset

-- ---------- The project ----------
INSERT INTO projects (project_name, site_location, client_name, sector, contract_price, start_date, target_end_date, status, show_on_website, created_by)
VALUES ('Aster Residences', 'San Juan City', 'Mr. and Mrs. Villareal', 'Repairs / Renovations', 2750000,
        CURRENT_DATE - 70, CURRENT_DATE + 80, 'Ongoing', TRUE, :rec_sup)
RETURNING project_id AS aster \gset

INSERT INTO change_orders (co_no, project_id, requested_on, description, cost_change, days_change, status, created_by, decided_by, decided_at) VALUES
 ('CO-001', :aster, CURRENT_DATE - 30, 'Add built-in cabinets in the master bedroom', 145000, 7, 'Approved', :rec_emp, :rec_sup, NOW() - INTERVAL '28 days'),
 ('CO-002', :aster, CURRENT_DATE - 2,  'Upgrade bathroom tiles to 60x60 porcelain',   38500,  0, 'Waiting',  :rec_emp, NULL, NULL);
-- the approved change order moved the target end date by its 7 days
UPDATE projects SET target_end_date = target_end_date + 7 WHERE project_id = :aster;

-- ---------- Inventory: vendors, materials, a quote comparison, deliveries, use on the project ----------
INSERT INTO vendors (name, contact_person, phone, email, address, vat_status, created_by) VALUES
 ('Bagong Lipunan Hardware & Construction Supply', 'Tony Uy',     '02-8361-2201', 'sales@blhardware.example.com', 'Quezon City',     'VAT',     :inv_emp),
 ('Metro North Cement Trading',                    'Mely Tan',    '02-8292-4410', 'orders@mncement.example.com',  'Valenzuela City', 'VAT',     :inv_emp),
 ('Rizal Steel & Rebar Supply',                    'Rey Chua',    '02-8645-0932', 'info@rizalsteel.example.com',  'Pasig City',      'Non-VAT', :inv_emp),
 ('San Juan Tile Center',                          'Liza Ong',    '02-8723-5518', 'hello@sjtile.example.com',     'San Juan City',   'VAT',     :inv_emp);
SELECT vendor_id AS v_hw    FROM vendors WHERE name = 'Bagong Lipunan Hardware & Construction Supply' \gset
SELECT vendor_id AS v_cem   FROM vendors WHERE name = 'Metro North Cement Trading' \gset
SELECT vendor_id AS v_steel FROM vendors WHERE name = 'Rizal Steel & Rebar Supply' \gset
SELECT vendor_id AS v_tile  FROM vendors WHERE name = 'San Juan Tile Center' \gset

-- stock = what was delivered (plus a little left from before) minus what was used on Aster (approved)
INSERT INTO materials (name, category, unit, unit_price, stock_qty, minimum_stock, created_by) VALUES
 ('Portland Cement 40kg',        'Cement',     'bag',    255,  70,  50, :inv_emp),
 ('Deformed Bar 10mm x 6m',      'Steel',      'piece',  185,  40,  60, :inv_emp),
 ('Washed Sand',                 'Aggregates', 'cu.m',   1350, 4,   3,  :inv_emp),
 ('Gravel 3/4',                  'Aggregates', 'cu.m',   1450, 5,   3,  :inv_emp),
 ('CHB 4 inch',                  'Masonry',    'piece',  17,   600, 300, :inv_emp),
 ('Marine Plywood 3/4 x 4x8',    'Lumber',     'sheet',  1450, 10,  8,  :inv_emp),
 ('Gypsum Board 1/2 x 4x8',      'Ceiling',    'sheet',  420,  25,  10, :inv_emp),
 ('Ceramic Floor Tile 60x60',    'Tiles',      'box',    780,  15,  10, :inv_emp),
 ('Latex Paint White',           'Paint',      'gallon', 820,  30,  10, :inv_emp),
 ('THHN Wire 3.5mm',             'Electrical', 'roll',   3250, 4,   3,  :inv_emp);
SELECT material_id AS m_cement FROM materials WHERE name = 'Portland Cement 40kg' \gset
SELECT material_id AS m_rebar  FROM materials WHERE name = 'Deformed Bar 10mm x 6m' \gset
SELECT material_id AS m_sand   FROM materials WHERE name = 'Washed Sand' \gset
SELECT material_id AS m_gravel FROM materials WHERE name = 'Gravel 3/4' \gset
SELECT material_id AS m_chb    FROM materials WHERE name = 'CHB 4 inch' \gset
SELECT material_id AS m_ply    FROM materials WHERE name = 'Marine Plywood 3/4 x 4x8' \gset
SELECT material_id AS m_gyp    FROM materials WHERE name = 'Gypsum Board 1/2 x 4x8' \gset
SELECT material_id AS m_tile   FROM materials WHERE name = 'Ceramic Floor Tile 60x60' \gset
SELECT material_id AS m_paint  FROM materials WHERE name = 'Latex Paint White' \gset
SELECT material_id AS m_wire   FROM materials WHERE name = 'THHN Wire 3.5mm' \gset

-- Price comparison: two cement quotes, the cheaper one chosen by the Inventory Supervisor
INSERT INTO quotes (material_id, vendor_id, quoted_on, product_name, description, category, unit_price, quantity, status, created_by) VALUES
 (:m_cement, :v_cem, CURRENT_DATE - 66, 'Portland Cement 40kg', 'Delivered to site, 3 days', 'Cement', 255, 150, 'Selected',     :inv_emp),
 (:m_cement, :v_hw,  CURRENT_DATE - 66, 'Portland Cement 40kg', 'Pick-up only',               'Cement', 268, 150, 'Not Selected', :inv_emp),
 (:m_tile,   :v_tile,CURRENT_DATE - 3,  'Porcelain Tile 60x60', 'For CO-002, glossy finish',   'Tiles',  1150, 40, 'Open',         :inv_emp);
INSERT INTO purchases (quote_id, material_id, vendor_id, quantity, unit_price, approved_by, approved_at)
SELECT quote_id, material_id, vendor_id, quantity, unit_price, :inv_sup, NOW() - INTERVAL '65 days'
FROM quotes WHERE material_id = :m_cement AND status = 'Selected'
RETURNING purchase_id AS p_cement \gset

INSERT INTO deliveries (purchase_id, vendor_id, material_id, quantity, unit_price, delivered_on, receipt_no, physical_only, filed_location, recorded_by) VALUES
 (:p_cement, :v_cem,   :m_cement, 150,  255,  CURRENT_DATE - 63, 'DR-24051', TRUE, 'Cabinet A, Folder 1', :inv_emp),
 (NULL,      :v_steel, :m_rebar,  140,  185,  CURRENT_DATE - 62, 'SI-10877', TRUE, 'Cabinet A, Folder 1', :inv_emp),
 (NULL,      :v_hw,    :m_sand,   10,   1350, CURRENT_DATE - 61, 'DR-55120', TRUE, 'Cabinet A, Folder 1', :inv_emp),
 (NULL,      :v_hw,    :m_gravel, 8,    1450, CURRENT_DATE - 61, 'DR-55121', TRUE, 'Cabinet A, Folder 1', :inv_emp),
 (NULL,      :v_hw,    :m_chb,    1500, 17,   CURRENT_DATE - 58, 'DR-55164', TRUE, 'Cabinet A, Folder 1', :inv_emp),
 (NULL,      :v_hw,    :m_ply,    30,   1450, CURRENT_DATE - 40, 'DR-55301', TRUE, 'Cabinet A, Folder 2', :inv_emp),
 (NULL,      :v_hw,    :m_gyp,    60,   420,  CURRENT_DATE - 33, 'DR-55388', TRUE, 'Cabinet A, Folder 2', :inv_emp),
 (NULL,      :v_tile,  :m_tile,   60,   780,  CURRENT_DATE - 20, 'SI-30210', TRUE, 'Cabinet A, Folder 2', :inv_emp),
 (NULL,      :v_hw,    :m_paint,  30,   820,  CURRENT_DATE - 8,  'DR-55470', TRUE, 'Cabinet A, Folder 2', :inv_emp),
 (NULL,      :v_hw,    :m_wire,   10,   3250, CURRENT_DATE - 45, 'DR-55250', TRUE, 'Cabinet A, Folder 2', :inv_emp);

INSERT INTO material_usages (material_id, project_id, quantity, used_on, notes, status, cost, requested_by, decided_by, decided_at) VALUES
 (:m_cement, :aster, 80,  CURRENT_DATE - 60, 'New kitchen slab and wall plastering',  'Approved', 80*255,  :inv_emp, :inv_sup, NOW() - INTERVAL '59 days'),
 (:m_rebar,  :aster, 100, CURRENT_DATE - 59, 'Kitchen extension columns and beams',   'Approved', 100*185, :inv_emp, :inv_sup, NOW() - INTERVAL '58 days'),
 (:m_sand,   :aster, 6,   CURRENT_DATE - 59, 'Concrete and plaster mix',              'Approved', 6*1350,  :inv_emp, :inv_sup, NOW() - INTERVAL '58 days'),
 (:m_gravel, :aster, 3,   CURRENT_DATE - 59, 'Kitchen slab',                          'Approved', 3*1450,  :inv_emp, :inv_sup, NOW() - INTERVAL '58 days'),
 (:m_chb,    :aster, 900, CURRENT_DATE - 55, 'Kitchen extension walls',               'Approved', 900*17,  :inv_emp, :inv_sup, NOW() - INTERVAL '54 days'),
 (:m_ply,    :aster, 20,  CURRENT_DATE - 38, 'Formworks and cabinet backing',         'Approved', 20*1450, :inv_emp, :inv_sup, NOW() - INTERVAL '37 days'),
 (:m_gyp,    :aster, 35,  CURRENT_DATE - 30, 'Living room and bedroom ceilings',      'Approved', 35*420,  :inv_emp, :inv_sup, NOW() - INTERVAL '29 days'),
 (:m_tile,   :aster, 45,  CURRENT_DATE - 18, 'Kitchen and dining floor',              'Approved', 45*780,  :inv_emp, :inv_sup, NOW() - INTERVAL '17 days'),
 (:m_wire,   :aster, 6,   CURRENT_DATE - 42, 'Rewiring (by the electrical subcontractor)', 'Approved', 6*3250, :inv_emp, :inv_sup, NOW() - INTERVAL '41 days'),
 (:m_paint,  :aster, 12,  CURRENT_DATE - 1,  'First coat, living and dining',         'Waiting',  12*820,  :inv_emp, NULL, NULL);

INSERT INTO equipment (name, type, ownership, rental_company, rental_rate, rental_rate_unit, return_date, status, next_maintenance, created_by) VALUES
 ('Concrete Mixer 1-bagger',        'Tool',    'Owned',  NULL, NULL, NULL, NULL, 'Available', CURRENT_DATE + 14, :inv_emp),
 ('Steel Scaffolding Set (10 frames)', 'Tool', 'Rented', 'Northline Equipment Rentals', 4500, 'Week', CURRENT_DATE + 21, 'Available', NULL, :inv_emp),
 ('Mini Dump Truck',                'Vehicle', 'Owned',  NULL, NULL, NULL, NULL, 'Under Repair', CURRENT_DATE - 3, :inv_emp);
INSERT INTO maintenance_logs (equipment_id, done_on, kind, work_done, cost, done_by, recorded_by)
SELECT equipment_id, CURRENT_DATE - 50, 'Preventive', 'Changed oil, greased the drum, checked the belt', 950, 'Site team', :inv_emp
FROM equipment WHERE name = 'Concrete Mixer 1-bagger';
INSERT INTO maintenance_logs (equipment_id, done_on, kind, work_done, cost, done_by, recorded_by)
SELECT equipment_id, CURRENT_DATE - 4, 'Repair', 'Replaced the clutch plate', 14500, 'Bonifacio Auto Shop', :inv_emp
FROM equipment WHERE name = 'Mini Dump Truck';

-- ---------- Subcontractors: electrical (stage 1 paid) and plumbing (stage 1 waiting for approval) ----------
INSERT INTO subcontractors (business_name, trade_name, primary_name, primary_role, primary_mobile, primary_email, office_address, pcab_license_no, pcab_classification, pcab_expiry, created_by) VALUES
 ('JRC Electrical Contractor',    'JRC Electrical', 'Jun Ramirez Cruz', 'Owner',           '09178300112', 'jun@jrcelectrical.example.com', 'Marikina City', 'PCAB-44812', 'Category C', CURRENT_DATE + 210, :sub_emp),
 ('AquaFlow Plumbing Services',   'AquaFlow',       'Rowena Dimaculangan', 'Project Manager', '09189022451', 'rowena@aquaflow.example.com', 'Caloocan City', 'PCAB-39005', 'Category D', CURRENT_DATE + 25, :sub_emp);
SELECT subcontractor_id AS s_elec FROM subcontractors WHERE business_name = 'JRC Electrical Contractor' \gset
SELECT subcontractor_id AS s_plmb FROM subcontractors WHERE business_name = 'AquaFlow Plumbing Services' \gset
INSERT INTO subcontractor_trades (subcontractor_id, trade) VALUES
 (:s_elec, 'Electrical Rough-ins & Wiring'), (:s_plmb, 'Plumbing & Sanitary');

INSERT INTO sub_jobs (subcontractor_id, project_id, scope, contract_amount, start_date, end_date, retention_pct, created_by)
VALUES (:s_elec, :aster, 'Whole-house rewiring, new panel board and lighting fixtures', 380000, CURRENT_DATE - 50, CURRENT_DATE + 30, 10, :sub_emp)
RETURNING job_id AS j_elec \gset
INSERT INTO sub_jobs (subcontractor_id, project_id, scope, contract_amount, start_date, end_date, retention_pct, created_by)
VALUES (:s_plmb, :aster, 'Kitchen and two bathrooms: new water lines, drains and fixtures', 265000, CURRENT_DATE - 35, CURRENT_DATE + 40, 10, :sub_emp)
RETURNING job_id AS j_plmb \gset

INSERT INTO sub_job_stages (job_id, stage_no, name, percent, reached_on, reached_by) VALUES
 (:j_elec, 1, 'Rough-in wiring',   40, CURRENT_DATE - 25, :sub_emp),
 (:j_elec, 2, 'Fixtures installed',40, NULL, NULL),
 (:j_elec, 3, 'Completion',        20, NULL, NULL),
 (:j_plmb, 1, 'Rough-in plumbing', 50, CURRENT_DATE - 6, :sub_emp),
 (:j_plmb, 2, 'Completion',        50, NULL, NULL);
SELECT stage_id AS st_elec1 FROM sub_job_stages WHERE job_id = :j_elec AND stage_no = 1 \gset
SELECT stage_id AS st_plmb1 FROM sub_job_stages WHERE job_id = :j_plmb AND stage_no = 1 \gset

INSERT INTO sub_progress (job_id, percent, progress_date, notes, created_by) VALUES
 (:j_elec, 40, CURRENT_DATE - 25, 'Rough-in done in all rooms, checked by the foreman', :sub_emp),
 (:j_elec, 55, CURRENT_DATE - 5,  'Panel board installed, fixtures on the 1st floor',   :sub_emp),
 (:j_plmb, 50, CURRENT_DATE - 6,  'Water lines and drains laid, pressure-tested',       :sub_emp);

-- voucher numbers continue after any already made (SV-0001, SV-0002, ...)
SELECT COALESCE(MAX(NULLIF(regexp_replace(voucher_no, '\D', '', 'g'), '')::int), 0) AS sv FROM sub_vouchers \gset
INSERT INTO sub_vouchers (voucher_no, job_id, stage_id, kind, gross_amount, retention_amount, payable_amount, status, prepared_by, decided_by, decided_at) VALUES
 ('SV-' || lpad((:sv + 1)::text, 4, '0'), :j_elec, :st_elec1, 'Stage', 152000, 15200, 136800, 'Paid',    :sub_emp, :sub_sup, NOW() - INTERVAL '23 days'),
 ('SV-' || lpad((:sv + 2)::text, 4, '0'), :j_plmb, :st_plmb1, 'Stage', 132500, 13250, 119250, 'Waiting', :sub_emp, NULL, NULL);
INSERT INTO sub_payouts (voucher_id, job_id, paid_on, amount, reference_no, method, created_by)
SELECT voucher_id, job_id, CURRENT_DATE - 22, 136800, 'BT-0911-2208', 'Bank Transfer', :sub_emp
FROM sub_vouchers WHERE job_id = :j_elec AND status = 'Paid';

-- ---------- Receipts: project costs, office overhead, one waiting, one with no receipt ----------
INSERT INTO receipts (receipt_no, purchase_date, merchant, amount, category, expense_type, project_id, physical_only, filed_location, no_receipt, no_receipt_reason, status, uploaded_by, decided_by, decided_at) VALUES
 ('OR-881204', CURRENT_DATE - 68, 'San Juan City Hall (Building Permit)', 18500, 'Permits',   'Project',  :aster, TRUE, 'Cabinet B, Aster',  FALSE, NULL, 'Approved', :rec_emp, :rec_sup, NOW() - INTERVAL '67 days'),
 ('OR-55912',  CURRENT_DATE - 52, 'Kuya Ben Hauling Services',            6500,  'Hauling',   'Project',  :aster, TRUE, 'Cabinet B, Aster',  FALSE, NULL, 'Approved', :rec_emp, :rec_sup, NOW() - INTERVAL '51 days'),
 ('SI-7710',   CURRENT_DATE - 36, 'Builders Fixtures Depot',   48750, 'Materials', 'Project',  :aster, TRUE, 'Cabinet B, Aster',  FALSE, NULL, 'Approved', :rec_emp, :rec_sup, NOW() - INTERVAL '35 days'),
 ('OR-3021',   CURRENT_DATE - 14, 'Debris Bin Rentals PH',                8000,  'Hauling',   'Project',  :aster, TRUE, 'Cabinet B, Aster',  FALSE, NULL, 'Approved', :rec_emp, :rec_sup, NOW() - INTERVAL '13 days'),
 ('SI-8015',   CURRENT_DATE - 2,  'Cabinet Hardware Center',              12350, 'Materials', 'Project',  :aster, TRUE, 'Tray on the desk',  FALSE, NULL, 'Waiting',  :rec_emp, NULL, NULL),
 (NULL,        CURRENT_DATE - 1,  'Tricycle fare to the site',            300,   'Transport', 'Project',  :aster, FALSE, NULL, TRUE, 'Driver gave no receipt', 'Waiting', :rec_emp, NULL, NULL),
 ('BILL-10-26',CURRENT_DATE - 12, 'Meralco (office electricity)',         9840,  'Utilities', 'Overhead', NULL,   TRUE, 'Cabinet C, Bills',  FALSE, NULL, 'Approved', :rec_emp, :rec_sup, NOW() - INTERVAL '11 days'),
 ('BILL-IN-77',CURRENT_DATE - 10, 'PLDT Fibr (office internet)',          2899,  'Utilities', 'Overhead', NULL,   TRUE, 'Cabinet C, Bills',  FALSE, NULL, 'Approved', :rec_emp, :rec_sup, NOW() - INTERVAL '9 days'),
 ('AR-2026-11',CURRENT_DATE - 7,  'Novaliches Office Space (rent)',       25000, 'Rent',      'Overhead', NULL,   TRUE, 'Cabinet C, Bills',  FALSE, NULL, 'Approved', :rec_emp, :rec_sup, NOW() - INTERVAL '6 days');

-- ---------- HR: five site workers, three weeks of attendance, cash advances ----------
INSERT INTO employees (full_name, contact_number, address, position, department, pay_schedule, biometric_id, gov_deductions, status, created_by) VALUES
 ('Bernardo "Ben" Aquino', '09171230001', 'Bagbag, Novaliches',  'Foreman',     'Site', 'Site', '201', FALSE, 'Active', :hr_emp),
 ('Ricardo Flores',        '09171230002', 'Gulod, Novaliches',   'Mason',       'Site', 'Site', '202', FALSE, 'Active', :hr_emp),
 ('Danilo Castillo',       '09171230003', 'Fairview, QC',        'Carpenter',   'Site', 'Site', '203', FALSE, 'Active', :hr_emp),
 ('Eduardo Torres',        '09171230004', 'Lagro, QC',           'Electrician', 'Site', 'Site', '204', FALSE, 'Active', :hr_emp),
 ('Nestor Lim',            '09171230005', 'Commonwealth, QC',    'Laborer',     'Site', 'Site', '205', FALSE, 'Active', :hr_emp);
INSERT INTO employee_rates (employee_id, daily_rate, effective_from, created_by)
SELECT employee_id, CASE biometric_id WHEN '201' THEN 1100 WHEN '205' THEN 645 ELSE 800 END, CURRENT_DATE - 365, :hr_emp
FROM employees WHERE biometric_id IN ('201', '202', '203', '204', '205');

-- Monday to Saturday for the last 3 weeks: site workers and the department staff. A few late days,
-- one missing scan (Danilo, 2 days ago) and one overtime waiting for approval (Ben, 3 days ago).
INSERT INTO attendance_days (employee_id, work_date, time_in, break_out, break_in, time_out, late_minutes, early_minutes, ot_minutes, ot_status, status)
SELECT e.employee_id, d::date,
       CASE WHEN (e.employee_id + EXTRACT(DOY FROM d)::int) % 11 = 0 THEN TIME '08:22' ELSE TIME '07:52' END,
       TIME '12:01', TIME '12:57',
       CASE WHEN e.biometric_id = '203' AND d::date = CURRENT_DATE - 2 THEN NULL
            WHEN e.biometric_id = '201' AND d::date = CURRENT_DATE - 3 THEN TIME '19:05'
            ELSE TIME '17:04' END,
       CASE WHEN (e.employee_id + EXTRACT(DOY FROM d)::int) % 11 = 0 THEN 22 ELSE 0 END,
       0,
       CASE WHEN e.biometric_id = '201' AND d::date = CURRENT_DATE - 3 THEN 120 ELSE 0 END,
       CASE WHEN e.biometric_id = '201' AND d::date = CURRENT_DATE - 3 THEN 'Waiting' ELSE 'None' END,
       CASE WHEN e.biometric_id = '203' AND d::date = CURRENT_DATE - 2 THEN 'Missing Scan'
            WHEN (e.employee_id + EXTRACT(DOY FROM d)::int) % 11 = 0 THEN 'Late'
            ELSE 'Present' END
FROM employees e
CROSS JOIN generate_series(CURRENT_DATE - 21, CURRENT_DATE - 1, INTERVAL '1 day') AS d
WHERE EXTRACT(DOW FROM d) <> 0
  AND e.biometric_id IN ('201', '202', '203', '204', '205', '11', '12', '13', '14', '15', '16', '17', '18')
ON CONFLICT (employee_id, work_date) DO NOTHING;

INSERT INTO cash_advances (employee_id, given_on, amount, per_payday, balance, reason, status, created_by, decided_by, decided_at)
SELECT employee_id, CURRENT_DATE - 18, 3000, 500, 3000, 'Family emergency', 'Active', :hr_emp, :hr_sup, NOW() - INTERVAL '17 days'
FROM employees WHERE biometric_id = '202';
INSERT INTO cash_advances (employee_id, given_on, amount, per_payday, balance, reason, status, created_by, decided_by, decided_at)
SELECT employee_id, CURRENT_DATE - 1, 2000, 500, 2000, 'School fees', 'Waiting', :hr_emp, NULL, NULL
FROM employees WHERE biometric_id = '204';

-- ---------- Website: Aster on the website (photos are added in Website Admin), three inquiries ----------
INSERT INTO website_projects (project_id, sort_order, is_visible, short_description, created_by)
VALUES (:aster, 1, TRUE, 'Refined interiors with a fresh, contemporary feel.', :web);

INSERT INTO inquiries (received_at, full_name, email, phone, project_location, service_required, message, status, notes) VALUES
 (NOW() - INTERVAL '3 hours', 'Pedro Manalo',    'pedro.manalo@example.com', '09190001122', 'Malolos, Bulacan', 'Residential Construction',
  'Good day! We plan to build a two-storey house on a 150 sqm lot next year. How much is the estimate per square meter?', 'New', NULL),
 (NOW() - INTERVAL '1 day',   'Sofia Reyes-Tan', 'sofia@cafe-sofia.example.com', '09175553210', 'Makati City', 'Commercial Construction',
  'Do you do coffee shop fit-outs? Around 80 sqm, ground floor of a mall.', 'New', NULL),
 (NOW() - INTERVAL '6 days',  'Luis Fernandez',  'luis.fernandez@example.com', '09223334455', 'Taguig City', 'Repairs and Home Extension',
  'Our roof leaks every time it rains hard. Can someone check it this week?', 'Contacted', 'Site visit set for Monday 9 AM. Bring the ladder.');
