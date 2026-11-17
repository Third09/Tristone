-- =====================================================================
-- Tri-Stone TEST Setup data (owner request 2026-11-17): run by build.ps1 -FromThisPc on a COPY of this PC's
-- database (zz_demo_build), never on the real one. It
--   1. removes the throwaway test entries (gibberish projects, materials, vendor, employee, inquiries, service, quotations),
--   2. clears sign-in/edit history and resets every password to the shared test password (no real hash leaves the PC),
--   3. fills the Materials side: about 50 more materials, 4 more vendors, every vendor's price list, open quotes,
--      approved purchases and material usage on Aster Residences.
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

-- ---------- 2. history and passwords ----------
DELETE FROM audit_logs;
DELETE FROM login_logs;
DELETE FROM password_requests;
UPDATE users SET password_hash = crypt('Test1234', gen_salt('bf', 11)), failed_login_count = 0,
                 security_question = NULL, security_answer_hash = NULL,
                 status = CASE WHEN status = 'Locked' THEN 'Active' ELSE status END;
-- the owner's own employee record: no personal ID numbers in a file given to others
UPDATE employees SET sss_no = NULL, philhealth_no = NULL, pagibig_no = NULL, tin = NULL
 WHERE employee_id IN (SELECT employee_id FROM users WHERE username IN ('ITmorilla', 'yumi'));

-- ---------- 3. materials and everything connected to them ----------
INSERT INTO vendors (name, contact_person, phone, email, address, vat_status, created_by) VALUES
 ('Ilaw Electrical Supply',  'Rowena Dizon', '02-8711-2040', 'sales@ilaw-electrical.example.com', 'Quezon City',   'VAT',     130),
 ('Tubo Plumbing Center',    'Arnel Pascual','02-8932-1188', 'orders@tubo-plumbing.example.com',  'Caloocan City', 'VAT',     130),
 ('Kulay Paint Center',      'Joy Manalastas','02-8655-7301','hello@kulay-paint.example.com',     'Marikina City', 'Non-VAT', 130),
 ('Kahoy Lumber Yard',       'Berto Salonga','0917-555-0144','kahoy.lumber@example.com',          'Valenzuela City','Non-VAT',130);

INSERT INTO materials (name, category, unit, unit_price, stock_qty, minimum_stock, created_by) VALUES
 ('White Cement 2kg',                'Cement',     'Bag',         95,    40,  10, 129),
 ('Tile Adhesive 25kg',              'Cement',     'Bag',         385,   22,  15, 129),
 ('Masonry Cement 40kg',             'Cement',     'Bag',         230,   55,  30, 129),
 ('Skim Coat 20kg',                  'Cement',     'Bag',         480,   8,   10, 129),
 ('Crushed Gravel 1/2',              'Aggregates', 'Cubic Meter', 1550,  6,   4,  129),
 ('Fine Sand',                       'Aggregates', 'Cubic Meter', 1250,  3,   4,  129),
 ('Filling Material (Escombro)',     'Aggregates', 'Cubic Meter', 650,   10,  5,  129),
 ('Deformed Bar 12mm x 6m',          'Steel',      'Piece',       265,   120, 80, 129),
 ('Deformed Bar 16mm x 6m',          'Steel',      'Piece',       470,   45,  40, 129),
 ('Tie Wire #16',                    'Steel',      'Kilo',        95,    25,  20, 129),
 ('Angle Bar 1 x 1 x 1/4',           'Steel',      'Piece',       640,   12,  6,  129),
 ('C-Purlins 2 x 4 x 6m',            'Steel',      'Piece',       780,   0,   10, 129),
 ('Welded Wire Mesh 6x6',            'Steel',      'Roll',        3950,  4,   2,  129),
 ('Coco Lumber 2x2x10',              'Wood',       'Piece',       85,    150, 100,129),
 ('Coco Lumber 2x3x12',              'Wood',       'Piece',       145,   90,  60, 129),
 ('Good Lumber 2x4x12',              'Wood',       'Piece',       540,   18,  10, 129),
 ('Ordinary Plywood 1/4 x 4x8',      'Wood',       'Sheet',       410,   35,  20, 129),
 ('Phenolic Board 3/4 x 4x8',        'Wood',       'Sheet',       1680,  14,  10, 129),
 ('THHN Wire 2.0mm',                 'Electrical', 'Roll',        2150,  6,   4,  129),
 ('THHN Wire 5.5mm',                 'Electrical', 'Roll',        5200,  2,   2,  129),
 ('PVC Conduit 1/2 in',              'Electrical', 'Piece',       65,    80,  50, 129),
 ('Utility Box',                     'Electrical', 'Piece',       28,    120, 60, 129),
 ('Circuit Breaker 20A',             'Electrical', 'Piece',       420,   10,  8,  129),
 ('LED Bulb 9W',                     'Electrical', 'Piece',       95,    48,  30, 129),
 ('Convenience Outlet Duplex',       'Electrical', 'Piece',       145,   36,  20, 129),
 ('PVC Sanitary Pipe 4 in x 3m',     'Plumbing',   'Piece',       520,   20,  12, 129),
 ('PVC Pipe 2 in x 3m',              'Plumbing',   'Piece',       230,   24,  12, 129),
 ('PPR Pipe 1/2 in x 4m',            'Plumbing',   'Piece',       210,   30,  20, 129),
 ('Gate Valve 1/2 in',               'Plumbing',   'Piece',       350,   9,   6,  129),
 ('Water Closet Set',                'Plumbing',   'Set',         4800,  3,   2,  129),
 ('Lavatory Sink with Faucet',       'Plumbing',   'Set',         3200,  1,   2,  129),
 ('Teflon Tape',                     'Plumbing',   'Roll',        25,    60,  30, 129),
 ('Concrete Primer',                 'Paint',      'Liter',       260,   24,  12, 129),
 ('Elastomeric Paint White',         'Paint',      'Liter',       310,   40,  20, 129),
 ('Paint Thinner',                   'Paint',      'Liter',       120,   15,  10, 129),
 ('Roof Paint Red',                  'Paint',      'Liter',       290,   0,   8,  129),
 ('Common Nail 3 in',                'Hardware',   'Kilo',        85,    40,  25, 129),
 ('Concrete Nail 2 in',              'Hardware',   'Kilo',        120,   12,  10, 129),
 ('Tekscrew 2 in',                   'Hardware',   'Box',         380,   7,   5,  129),
 ('Door Hinges 4 in',                'Hardware',   'Piece',       95,    30,  20, 129),
 ('Door Knob Lockset',               'Hardware',   'Set',         650,   6,   5,  129),
 ('Contact Cement',                  'Hardware',   'Liter',       340,   5,   4,  129),
 ('Sandpaper #100',                  'Hardware',   'Piece',       18,    100, 50, 129),
 ('Corrugated GI Sheet Ga.26 x 8ft', 'Other',      'Sheet',       520,   30,  20, 129),
 ('Fiber Cement Board 4.5mm',        'Other',      'Sheet',       560,   16,  10, 129),
 ('Waterproofing Membrane',          'Other',      'Roll',        2950,  3,   2,  129),
 ('Safety Helmet',                   'Other',      'Piece',       180,   20,  10, 129),
 ('Cement Mixer Hose 1 in',          'Other',      'Meter',       60,    50,  30, 129);

-- every material: its usual supplier at ~3% below the list price, and the hardware store at ~4% above
CREATE TEMP TABLE supplier_by_category (category text, vendor text);
INSERT INTO supplier_by_category VALUES
 ('Cement', 'Metro North Cement Trading'), ('Aggregates', 'Metro North Cement Trading'),
 ('Steel', 'Rizal Steel & Rebar Supply'), ('Wood', 'Kahoy Lumber Yard'), ('Lumber', 'Kahoy Lumber Yard'),
 ('Electrical', 'Ilaw Electrical Supply'), ('Plumbing', 'Tubo Plumbing Center'), ('Paint', 'Kulay Paint Center'),
 ('Tiles', 'San Juan Tile Center');
INSERT INTO vendor_materials (vendor_id, material_id, price, updated_by)
SELECT v.vendor_id, m.material_id, ROUND(m.unit_price * 0.97, 2), 129
  FROM materials m JOIN supplier_by_category s ON s.category = m.category JOIN vendors v ON v.name = s.vendor;
INSERT INTO vendor_materials (vendor_id, material_id, price, updated_by)
SELECT v.vendor_id, m.material_id, ROUND(m.unit_price * 1.04, 2), 129
  FROM materials m JOIN vendors v ON v.name = 'Bagong Lipunan Hardware & Construction Supply'
ON CONFLICT (vendor_id, material_id) DO NOTHING;

-- open quotes for things that are low or out (Price Comparison has something to compare)
INSERT INTO quotes (material_id, vendor_id, quoted_on, product_name, description, category, unit_price, quantity, status, created_by)
SELECT m.material_id, v.vendor_id, CURRENT_DATE - q.days, m.name, q.note, m.category, q.price, q.qty, 'Open', 129
  FROM (VALUES
        ('C-Purlins 2 x 4 x 6m',  'Rizal Steel & Rebar Supply',                    760,  40, 2, 'Pick-up at their Caloocan yard'),
        ('C-Purlins 2 x 4 x 6m',  'Bagong Lipunan Hardware & Construction Supply', 815,  40, 1, 'Free delivery within QC'),
        ('Roof Paint Red',        'Kulay Paint Center',                            280,  24, 3, NULL),
        ('Roof Paint Red',        'Bagong Lipunan Hardware & Construction Supply', 302,  24, 1, NULL),
        ('Skim Coat 20kg',        'Metro North Cement Trading',                    465,  30, 2, NULL),
        ('Skim Coat 20kg',        'Bagong Lipunan Hardware & Construction Supply', 499,  30, 2, 'Brand: any equivalent'),
        ('Fine Sand',             'Metro North Cement Trading',                    1200, 6,  1, 'Delivered by 6-wheeler'))
       AS q(material, vendor, price, qty, days, note)
  JOIN materials m ON m.name = q.material JOIN vendors v ON v.name = q.vendor;

-- earlier purchases that were approved (their quotes Selected / Not Selected)
CREATE TEMP TABLE past_buys (material text, winner text, loser text, price numeric, loser_price numeric, qty numeric, days int);
INSERT INTO past_buys VALUES
 ('Deformed Bar 12mm x 6m',      'Rizal Steel & Rebar Supply', 'Bagong Lipunan Hardware & Construction Supply', 258, 276, 150, 30),
 ('THHN Wire 2.0mm',             'Ilaw Electrical Supply',     'Bagong Lipunan Hardware & Construction Supply', 2090, 2240, 8, 21),
 ('PVC Sanitary Pipe 4 in x 3m', 'Tubo Plumbing Center',       'Bagong Lipunan Hardware & Construction Supply', 505, 545, 24, 14),
 ('Elastomeric Paint White',     'Kulay Paint Center',         'Bagong Lipunan Hardware & Construction Supply', 300, 322, 48, 9);
INSERT INTO quotes (material_id, vendor_id, quoted_on, product_name, category, unit_price, quantity, status, created_by)
SELECT m.material_id, v.vendor_id, CURRENT_DATE - b.days - 2, m.name, m.category, b.loser_price, b.qty, 'Not Selected', 129
  FROM past_buys b JOIN materials m ON m.name = b.material JOIN vendors v ON v.name = b.loser;
INSERT INTO quotes (material_id, vendor_id, quoted_on, product_name, category, unit_price, quantity, status, created_by)
SELECT m.material_id, v.vendor_id, CURRENT_DATE - b.days - 2, m.name, m.category, b.price, b.qty, 'Selected', 129
  FROM past_buys b JOIN materials m ON m.name = b.material JOIN vendors v ON v.name = b.winner;
INSERT INTO purchases (quote_id, material_id, vendor_id, quantity, unit_price, approved_by, approved_at)
SELECT q.quote_id, q.material_id, q.vendor_id, q.quantity, q.unit_price, 130, (CURRENT_DATE - b.days)::timestamp + TIME '10:30'
  FROM past_buys b JOIN materials m ON m.name = b.material JOIN vendors v ON v.name = b.winner
  JOIN quotes q ON q.material_id = m.material_id AND q.vendor_id = v.vendor_id AND q.status = 'Selected';

-- material used on Aster Residences: approved (cost to the project) and some still waiting
INSERT INTO material_usages (material_id, project_id, quantity, used_on, notes, status, cost, requested_by, decided_by, decided_at)
SELECT m.material_id, p.project_id, u.qty, CURRENT_DATE - u.days, u.note, u.status,
       CASE WHEN u.status = 'Approved' THEN ROUND(u.qty * m.unit_price, 2) END, 129,
       CASE WHEN u.status = 'Approved' THEN 130 END,
       CASE WHEN u.status = 'Approved' THEN (CURRENT_DATE - u.days)::timestamp + TIME '16:00' END
  FROM (VALUES
        ('Deformed Bar 12mm x 6m',      60, 28, 'Second floor slab',          'Approved'),
        ('Tie Wire #16',                12, 28, 'Second floor slab',          'Approved'),
        ('Coco Lumber 2x2x10',          80, 25, 'Formworks',                  'Approved'),
        ('Phenolic Board 3/4 x 4x8',    10, 25, 'Formworks',                  'Approved'),
        ('PVC Sanitary Pipe 4 in x 3m', 8,  12, 'Toilet and bath lines',      'Approved'),
        ('THHN Wire 2.0mm',             3,  10, 'Lighting circuits',          'Approved'),
        ('Elastomeric Paint White',     16, 4,  'Exterior walls, first coat', 'Approved'),
        ('Skim Coat 20kg',              6,  2,  'Interior walls',             'Waiting'),
        ('Door Knob Lockset',           4,  1,  'Bedroom doors',              'Waiting'),
        ('Water Closet Set',            2,  1,  'Two bathrooms',              'Waiting'))
       AS u(material, qty, days, note, status)
  JOIN materials m ON m.name = u.material
  JOIN projects p ON p.project_name = 'Aster Residences';

COMMIT;
