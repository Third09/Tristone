# Tri-Stone: Tester Guide

Thank you for testing **Tri-Stone**, the office program of Tri-Stone Construction Services. Everything you see is a **test copy**: nothing you do reaches the real company records or the website, so feel free to click around.

# 1. Install

1. Double-click **Tri-Stone-Test-Setup-1.6.0.exe**.
2. If Windows says *"Windows protected your PC"*: click **More info**, then **Run anyway**.
3. Click **Next** until **Install**, then **Finish**. Tri-Stone opens by itself.
4. The first start takes about **15 seconds** (it prepares its data). After that it opens quickly.

Later, open it from the **Start menu > Tri-Stone Demo**.

# 2. Sign in

| Type this | Password | What happens |
|---|---|---|
| **demo.admin** | **Demo1234** | A list of every person opens (**Test as...**). Double-click a person to use Tri-Stone as them. |

**Sign Out** (bottom left) brings you back to the list, so you can try another person. Every other login uses the password **Test1234**.

| Pick this person | To test |
|---|---|
| **admin2** (Admin) | looking at every page, making logins in **User Accounts** |
| **hr_emp** / **hr_sup** | **HR & Payroll**: employees, attendance, payroll, cash advances |
| **inv_emp** / **inv_sup** | **Inventory**: materials, usage, vendors, price comparison |
| **sub_emp** / **sub_sup** | **Subcontractors**: directory, jobs, payments |
| **rec_emp** / **rec_sup** | **Receipts**, **Quotations** and **Projects** |
| **Webadmin1** | the **Website Admin** screens (publishing is turned off) |

**Employees** (\_emp) add and send things for approval. **Supervisors** (\_sup) approve, reject and delete.

# 3. Things to try

1. **Add a material** (inv_emp): Inventory > Materials > **Add Material**. Then **Material Usage**: use some on *Aster Residences*.
2. **Approve it** (inv_sup): Material Usage > select it > **Approve**. The stock goes down.
3. **Compare prices** (inv_emp, then inv_sup): Price Comparison > add 2 quotes > the Supervisor clicks **Approve as Purchase**. The stock goes up.
4. **Add an employee** (hr_emp): HR & Payroll > Employees > **Add Employee**. Choose the department, then the role.
5. **Make a quotation** (rec_emp): Quotations > **New Quotation**. Fill in the customer, the works and the costs, **Save**, **Send for Approval**. Then rec_sup clicks **Approve** and **Print**.
6. **Add a receipt** (rec_emp): Receipts > Receipt Vault > **Add Receipt**. rec_sup approves it.
7. **Change order** (rec_emp): Receipts > Change Orders > **New Change Order** for *Aster Residences*.
8. **Look at the numbers** (admin2): **Dashboard** and **Projects**.

Try mistakes too: leave boxes empty, type letters in number boxes, very big numbers, dates far in the future. Tri-Stone should say what's wrong instead of saving.

# 4. If something goes wrong

- If a window says **"Something went wrong on this screen"**: click OK and keep going. Please **write down what you clicked just before** and take a screenshot. The details are saved for the developer in **errors.log** (the message shows where).
- **5 wrong passwords** lock that login for **15 minutes**. Pick another person in the meantime.
- **To start over** with the original test data: close Tri-Stone, then **Start menu > Reset Tri-Stone Demo (back to the sample data)** > **Yes**.

# 5. Please tell us

For each problem: **who you were signed in as**, **the page**, **what you clicked**, **what you expected**, **what happened** (a screenshot helps).
