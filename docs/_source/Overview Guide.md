# Tri-Stone: Overview Guide

Everything built so far, in plain words, on a few pages. Read this first; the other guides go deeper:

| Guide | Read it when you want to know... |
|---|---|
| **Overview Guide** (this one) | what the system is, who uses what, and how it all fits together |
| **User Manual** | how to use each screen, step by step |
| **Feature Guide** | the exact rules: limits, checks, formulas, what each button changes |
| **Database Guide** | how the data is stored: tables, links, scripts |
| **Developer Guide** | how the code is organised, for whoever maintains it |
| **UI Elements Guide** | every button and box on every screen, with its name |
| **Project Outline** | the original plan and every decision added to it |

# 1. What Tri-Stone is

Tri-Stone is the company's own system, made of three parts:

| Part | Who uses it | What it's for |
|---|---|---|
| **Office app** | office staff on the 5 office PCs | running the business: projects, HR and payroll, inventory, subcontractors, receipts |
| **Website Admin** | Website Admins (and Admins) | changing what the public website shows and following up website inquiries |
| **Public website** | clients, on the internet | showing the company, its services and projects; sending inquiries |

The office app and Website Admin are **one program**, `TriStone.exe`. Everyone signs in on the same Sign In window, and the program opens the right part for that person.

All the data lives in **one database** (PostgreSQL) on the office server. Uploaded files (receipt scans, photos, papers) live in **one shared folder**; the database only remembers where each file is.

# 2. Who can do what

Every login has a **Level**. Office staff also have one **Department**: HR & Payroll, Inventory, Subcontractors or Receipts.

| Level | In short |
|---|---|
| **Employee** | does the daily work of their own department: adds and edits records, sends things for approval |
| **Supervisor** | everything their Employee does, plus **approves** and **deletes** in their own department, and sees the Dashboard for it |
| **Admin** | can **look at everything** but changes nothing in the departments; handles **accounts** (makes logins, sets passwords, unlocks). Can also edit the website |
| **Website Admin** | only the Website Admin screens |

Two special cases: the **Receipts Supervisor** also manages the **Projects** list and approves **change orders**.

**Rules for logins**

- Each login belongs to one office employee (site workers never get a login).
- Only Admins make logins and change passwords. A person who forgets theirs clicks **Forgot Password?**; an Admin sets a new one and gives it in person.
- 5 wrong passwords lock the account until an Admin unlocks it.
- After 5 minutes without activity a "Still there?" message appears; no answer signs the person out.

# 3. The office app, page by page

| Page | What happens there |
|---|---|
| **Dashboard** | the money at a glance: finished projects, revenue, expenses, profit, a chart of projects by status, the best projects. Admins see everything and the sign-in history; a Supervisor sees their own department |
| **Projects** | the one list of all projects (name, site, client, sector, price, dates, status). Every other part picks projects from here. Tick **Show on website** to offer a project to the website |
| **HR & Payroll** | employees and their daily rates; **Attendance** from the fingerprint scanner's file; **Payroll** computed from attendance; payslips; cash advances; settings (work hours, government rates, holidays) |
| **Inventory** | materials and stock; using materials on a project (needs approval); deliveries; equipment and maintenance; vendors; comparing quotes |
| **Subcontractors** | the subcontractor directory; jobs with payment stages; progress; vouchers and payouts; retention |
| **Receipts** | every receipt (for a project or company overhead); expenses with no receipt; **change orders** to a project's price or end date; reports |
| **User Accounts** | (Admins) making and managing logins |
| **My Account** | your own details; Admins change their password and security question here |

Every page that changes data also writes **Edit History**: who changed what, and the old and new values. Supervisors and Admins can read it.

# 4. How the work flows

**A project's money**

1. A project is added in **Projects** (contract price).
2. Materials used on it (approved in Inventory), receipts for it (approved in Receipts) and subcontractor payouts add up as its **expenses**.
3. Approved **change orders** raise or lower its **revenue**.
4. When the project is set to **Done**, the Dashboard counts its revenue, expenses and profit in that period.

Payroll and equipment repairs are **company overhead**, not project expenses.

**Asking and approving**

The pattern is the same everywhere: the **Employee** records it, the request waits, the **Supervisor** approves or rejects it. Only approved items count in the totals.

| What | Who records | Who approves |
|---|---|---|
| Using materials on a project | Inventory Employee | Inventory Supervisor |
| Buying from a vendor (choosing a quote) | Inventory Employee | Inventory Supervisor |
| Subcontractor voucher | Subcontractors Employee | Subcontractors Supervisor |
| Receipt, or an expense with no receipt | Receipts Employee | Receipts Supervisor |
| Change order | Receipts Employee | Receipts Supervisor |
| Overtime, cash advance, final payroll | HR Employee | HR Supervisor |

**Payroll in one line**

Scanner file → **Attendance** (each person's days: present, late, absent...) → HR fixes missing scans → **Payroll** computes pay for the period (days × daily rate − late minutes + approved overtime − cash advance − government deductions if switched on) → the HR Supervisor **finalizes** → **payslips** print, and those attendance days are locked.

# 5. Website Admin

The sidebar follows the Figma design: **Home, Projects, Services, About Us, Contact Us**.

| Page | What you change |
|---|---|
| **Home** | the big top picture, the headline and the line under it, with a live preview |
| **Projects** | which office projects show on the website (only ones ticked **Show on website**), their short description and up to 10 photos |
| **Services** | the service cards (any service you type), each with location, description, icon and picture |
| **About Us** | the sections (starting with About Tri-Stone, Our Principles, Vision & Mission): picture, heading, text; add, reorder, delete |
| **Contact Us** | the inquiries from the website's Contact form, the counts and website visits, where inquiries are emailed, and the company address, phone, email and footer |

**Save Changes** keeps your edits; **Publish Website** (top right) puts everything saved on the live website.

**Preview Website** shows the whole site with your saved changes before you publish. The Publish button shows how many edits are waiting, **Undo Publish** puts the previous version back, Ctrl+S saves, big photos are made smaller, and a Windows notification tells you about new inquiries. Publish sends your changes to the website's GitHub page, and Netlify (the website's host) shows them in about a minute. Each PC needs the GitHub token once (Website Settings).

# 6. How it looks

The design follows the Figma file "CCMS SUPERSYSTEM". One file, `Theme.cs`, holds the colors and styles, like a CSS file:

- **Orange** = the main action (Add, Save, Approve) and the open page. **Red** = remove or refuse. **Dark green** = Publish Website.
- **Tables** have a border, lines between cells, a gray header and shaded rows; "No records to show" when empty.
- **Website Admin** pages use white cards on a light gray background, picture boxes you can drag photos onto, live previews, and colored message boxes.
- Whatever is changed by hand in Visual Studio's designer wins over the theme.

# 7. Where things are

| Thing | Where |
|---|---|
| The program | `Apps\Tri-Stone\TriStone.exe` (built from Visual Studio, project **Company.Launcher**) |
| The code | `src\` in the TriStone folder (Visual Studio: `TriStone.slnx`) |
| Database changes | `database\` (numbered scripts 001-014; `setup-database.cmd` applies the new ones) |
| Uploaded files | the shared files folder (`C:\TriStoneFiles` on this PC) |
| Guides | `docs\` (Word files; their text is in `docs\_source`) |
| What was tested | `docs\Testing Log.md` (and the "Check at home" list) |
| Website | separate project: `tristone_website` (GitHub), hosted on Netlify |

# 8. What's done and what's next

**Done**

- Sign in, accounts, access levels, idle sign-out.
- Projects, Dashboard, HR & Payroll (employees, attendance, payroll, payslips, cash advances, settings), Inventory, Subcontractors, Receipts and change orders.
- Website Admin: Home, Projects, Services, About Us, Contact Us, Publish.
- The new look on every screen.

**Next**

1. Check the "Check at home" list in the Testing Log on real data.
2. The website side: the Contact form changes and the visit counter.
3. The **API**: receives inquiries and visits from the website.
4. **Phase 6, the office setup:** the server kit is ready (`server` folder: one install script for the Linux laptop with the database, shared files folder, firewall and nightly backups; a copy tool to move your data). Still to do: run it on the real laptop, Setup.exe for the 5 PCs, and the website connection through Cloudflare Tunnel.

# 9. Words used

| Word | Meaning |
|---|---|
| Level | Admin, Supervisor, Employee or Website Admin |
| Approve | a Supervisor accepting a request; only approved items count |
| Overhead | company costs not tied to a project (payroll, repairs, office receipts) |
| Change order | an agreed change to a project's price or end date |
| Retention | part of a subcontractor's pay held back until the job is checked |
| Voucher | the paper that allows paying a subcontractor stage |
| Finalize | locking a payroll so it can't change |
| Publish | putting the saved website changes on the live website |
| Edit History | the record of every change: who, when, old and new values |
| Theme | the one file that sets the app's colors and styles |
