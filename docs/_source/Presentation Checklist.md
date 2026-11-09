# Tri-Stone: Presentation Checklist

# 1. Before the day

- [ ] Pull the latest and build. Open Tri-Stone from the Desktop shortcut.
- [ ] Sample data is loaded (Aster Residences). Unpublish or delete the test project "12312312" in Website Admin.
- [ ] Publish the website, then publish the deploy in Netlify. Check tristone.netlify.app on a laptop and on a phone.
- [ ] Have the logins ready (the credentials sheet): admin2, rec_sup, inv_emp, inv_sup, hr_sup, Webadmin1.
- [ ] Turn off notifications, close other apps, plug in the charger.

# 2. Order of the demo (about 10 minutes)

1. [ ] **Sign In:** one login for everyone, the lock after 5 wrong passwords, the Admin's choice window.
2. [ ] **Access levels:** inv_emp can add but not approve; inv_sup approves and deletes; admin2 can only look.
3. [ ] **Projects and Dashboard:** Aster Residences with its revenue, expenses and profit; the approved change order; the KPI cards and chart.
4. [ ] **Inventory:** record a material use, the Supervisor approves it, the stock goes down.
5. [ ] **Subcontractors:** job stages, a voucher, a payout, retention.
6. [ ] **Receipts:** project or overhead, Needs Review, a printed report.
7. [ ] **HR & Payroll:** attendance (late, missing scan, overtime), Compute payroll, print a payslip.
8. [ ] **Website Admin:** edit Home with the live preview, add a project photo, Preview Website, Publish, then show it on the live site.
9. [ ] **Contact Us:** send an inquiry from the website, open it in Website Admin, Reply in Gmail.
10. [ ] **Behind the scenes:** Edit History, Excel export, the server kit with nightly backups, Setup.exe.

# 3. Be ready to answer

- [ ] Why PostgreSQL and a desktop app: it runs on the office network, fast and private.
- [ ] How the data is kept safe: passwords, roles that limit what each person can do, a locked database login, nightly backups.
- [ ] How the money is counted: revenue = contract price + approved change orders; expenses = materials + receipts + subcontractor payouts; a project counts on the Dashboard when it is Done.
- [ ] What's next: moving the database to the office server and installing Tri-Stone on the 5 office PCs.
