# Tri-Stone: Logs Demo

A step-by-step demo showing that Tri-Stone keeps a record of **who did what and when**, and that each Supervisor sees only their own department while Admins see everything. It takes about 15 minutes.

# 1. What is recorded, and who can see it

| What happens | Where it is recorded | Who can see it |
|---|---|---|
| Adding, editing, approving, rejecting or deleting anything in **HR & Payroll** | HR & Payroll > **Edit History** | HR Supervisor, Admins |
| The same in **Inventory** | Inventory > **Edit History** | Inventory Supervisor, Admins |
| The same in **Subcontractors** | Subcontractors > **Edit History** | Subcontractors Supervisor, Admins |
| The same in **Receipts**, **Change Orders**, **Projects** and **Quotations** | Receipts > **Edit History** | Receipts Supervisor, Admins |
| Logins made, changed, unlocked, disabled or enabled | User Accounts > **Account History** | Admins only |
| Website Admin: page text, projects, photos, services, About Us, moves, inquiry status and notes, every Publish | Website Admin > **Edit History** | Website Admins, Admins |
| Sign in, sign out, wrong passwords, locked accounts, password changes, password requests (and dismissed ones) | Dashboard > **Log History** | Admins only |

Employees (\_emp logins) never see a history; they only do the work. Each change shows **Date & Time**, **Who**, **Action** (Added / Edited / Deleted), the **Record**, and for edits the **Field** with its **Old Value** and **New Value**. Passwords and government ID numbers are never shown in a history.

# 2. Before you start

- Use the **Test** version (Tri-Stone-Test-Setup), so nothing touches real data.
- Sign in at the **Sign In window** with each person below (password **Test1234**), not through demo.admin's list, so the sign-ins are recorded like real ones.
- After each part, click **Sign Out** (bottom left).

# 3. The demo

## Part A: a wrong password (Log History)

1. At the Sign In window type **hr_emp** and a wrong password, click **Sign In**. Tri-Stone says *Wrong username or password.*
2. Do it once more, then sign in properly with **Test1234**.

## Part B: HR & Payroll

1. Signed in as **hr_emp**: go to **HR & Payroll > Employees**, click any employee.
2. Change the **contact number** (for example 0917 555 1234) and the **daily rate**, click **Save**, then **Yes**.
3. **Sign Out**.
4. Sign in as **hr_sup**. Open **HR & Payroll > Edit History**.
5. Show the audience: the top rows are *hr_emp's* changes, with the old and new contact number and daily rate, the date and time.
6. Use the **Person** filter to pick hr_emp's name: only their changes stay.
7. Show the sidebar: hr_sup has **no Inventory, Subcontractors or Receipts** pages, so they can't see other departments' history.
8. **Sign Out**.

## Part C: Inventory (add, then approve)

1. Sign in as **inv_emp**: **Inventory > Materials > Add Material**, fill it in, **Save**. Then **Material Usage**: use 5 of it on *Aster Residences*, **Save**. **Sign Out**.
2. Sign in as **inv_sup**: **Material Usage**, select the new entry, **Approve**.
3. Open **Inventory > Edit History**: you see three rows: the material added by inv_emp, the usage added by inv_emp, and the approval by inv_sup.
4. **Sign Out**.

## Part D: Receipts (also covers Projects and Quotations)

1. Sign in as **rec_emp**: **Receipts > Receipt Vault > Add Receipt**, fill it in, **Save**. **Sign Out**.
2. Sign in as **rec_sup**: approve the receipt. Then open **Projects**, change a project's **target end date**, **Save**, **Yes**.
3. Open **Receipts > Edit History**: the receipt (added by rec_emp, approved by rec_sup) and the project change (old and new date) are both there.
4. **Sign Out**.

## Part E: Website Admin

1. Sign in as **Webadmin1**. Open **Contact Us**, double-click an inquiry, set the status to **Contacted**, type a note, **Save**.
2. Open **Services**, select a card, click **Move Down**.
3. Click **Edit History** in the sidebar: the inquiry's status and note (old and new) and the card's position (1 -> 2) are there, with your name and the time.
4. **Sign Out**.

## Part F: the Admin sees everything

1. Sign in as **admin2**.
2. Open **HR & Payroll > Edit History**, **Inventory > Edit History**, **Receipts > Edit History**: every change from Parts B to D is there.
3. Open **User Accounts**, select **hr_emp**, click **Disable**, then **Enable**. Click **Account History**: both changes are listed.
4. Open **Dashboard** and scroll to **Log History**:
   - every **sign in** and **sign out** from this demo, with the PC's name;
   - the two **Failed Login** rows for hr_emp from Part A;
   - set **Type** to *Failed Login* to show only those.
5. Click **Export** on any history to save it to Excel.

# 4. What to say if asked

- **Can someone erase their tracks?** No. No screen edits or deletes a history row; deleting a record adds a *Deleted* row with what it held.
- **Can a Supervisor see another department?** No. Only their own department's Edit History; Admins see all.
- **Can an Admin change department records?** No, since version 1.6 Admins only view office pages. They manage logins and passwords.
