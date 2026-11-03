// =====================================================================
// Tri-Stone website Contact form -> Google Apps Script (script.google.com)
// Copy ALL of this into the script's Code.gs, replacing what is there.
//
// What it does:
//  - doPost: the website's Contact form sends each message here.
//      1. saves it as a row in the "Tri-Stone Inquiries" Google Sheet
//      2. emails it to TO_EMAIL (same nice email as before)
//  - doGet: the Tri-Stone Website Admin app (Contact Us page) reads the
//      saved rows with the secret read key, and adds new ones to its list.
//  - setup: run ONCE by hand (Run button). Makes the Sheet and the read key,
//      and shows the key in the Execution log for Website Admin.
//
// After changing this code: Deploy > Manage deployments > pencil (Edit) >
// Version: New version > Deploy. The web address stays the same.
// =====================================================================

const TO_EMAIL = "tristoneconstructionservices@gmail.com";
const LOGO_URL = "https://clydezhit.github.io/tri-stone/images/tristone_logo_white.png";
const SHEET_HEADERS = ["id", "received", "name", "email", "phone", "service", "location", "message"];

// ---------- One-time setup (run by hand) ----------
function setup() {
  const props = PropertiesService.getScriptProperties();
  if (!props.getProperty("SHEET_ID")) {
    const ss = SpreadsheetApp.create("Tri-Stone Inquiries");
    const sh = ss.getSheets()[0];
    sh.getRange("A:H").setNumberFormat("@");   // plain text: keeps 0 in front of phone numbers
    sh.appendRow(SHEET_HEADERS);
    sh.setFrozenRows(1);
    props.setProperty("SHEET_ID", ss.getId());
  }
  if (!props.getProperty("READ_KEY")) {
    props.setProperty("READ_KEY", Utilities.getUuid().replace(/-/g, "") + Utilities.getUuid().replace(/-/g, ""));
  }
  Logger.log("Sheet: https://docs.google.com/spreadsheets/d/" + props.getProperty("SHEET_ID"));
  Logger.log("Read key for Website Admin: " + props.getProperty("READ_KEY"));
}

function inquirySheet() {
  const id = PropertiesService.getScriptProperties().getProperty("SHEET_ID");
  if (!id) throw new Error("setup() was never run");
  return SpreadsheetApp.openById(id).getSheets()[0];
}

// Text exactly as typed; a leading = + - @ must not turn into a sheet formula
function cell(v, max) {
  const s = String(v == null ? "" : v).slice(0, max);
  return /^[=+\-@]/.test(s) ? "'" + s : s;
}

function saveInquiry(d) {
  const lock = LockService.getScriptLock();
  lock.waitLock(10000);
  try {
    inquirySheet().appendRow([
      Utilities.getUuid(),
      Utilities.formatDate(new Date(), "Asia/Manila", "yyyy-MM-dd'T'HH:mm:ss"),
      cell(d.name, 100), cell(d.email, 150), cell(d.phone, 40),
      cell(d.service, 100), cell(d.location, 255), cell(d.message, 4000)
    ]);
  } finally {
    lock.releaseLock();
  }
}

function json(obj) {
  return ContentService.createTextOutput(JSON.stringify(obj)).setMimeType(ContentService.MimeType.JSON);
}

// ---------- Website Admin reads the saved inquiries ----------
function doGet(e) {
  const key = PropertiesService.getScriptProperties().getProperty("READ_KEY");
  if (!key || !e || !e.parameter || e.parameter.key !== key) return json({ ok: false, error: "wrong key" });
  try {
    const values = inquirySheet().getDataRange().getValues();
    const text = (v) => (v instanceof Date ? Utilities.formatDate(v, "Asia/Manila", "yyyy-MM-dd'T'HH:mm:ss") : String(v));
    const rows = values.slice(1).filter((r) => r[0]).map((r) => ({
      id: text(r[0]), received: text(r[1]), name: text(r[2]), email: text(r[3]),
      phone: text(r[4]), service: text(r[5]), location: text(r[6]), message: text(r[7])
    }));
    return json({ ok: true, rows: rows });
  } catch (err) {
    return json({ ok: false, error: String(err) });
  }
}

// ---------- Email (unchanged look) ----------
// Makes user input safe to show inside the email
function esc(s) {
  return String(s || "")
    .replace(/&/g, "&amp;")
    .replace(/</g, "&lt;")
    .replace(/>/g, "&gt;")
    .replace(/"/g, "&quot;");
}

function row(label, value) {
  return '<tr>' +
    '<td style="padding:12px 0;border-bottom:1px solid #eceef1;width:120px;font-size:12px;font-weight:700;letter-spacing:.06em;text-transform:uppercase;color:#7b8089;vertical-align:top;">' + label + '</td>' +
    '<td style="padding:12px 0;border-bottom:1px solid #eceef1;font-size:15px;color:#1c1f26;vertical-align:top;">' + value + '</td>' +
    '</tr>';
}

function buildHtml(d) {
  const when = Utilities.formatDate(new Date(), "Asia/Manila", "MMM d, yyyy 'at' h:mm a");
  const phone = d.phone
    ? '<a href="tel:' + esc(d.phone.replace(/\s/g, "")) + '" style="color:#1c1f26;text-decoration:none;">' + esc(d.phone) + '</a>'
    : '<span style="color:#9aa0a8;">Not provided</span>';
  const message = d.message
    ? esc(d.message).replace(/\n/g, "<br>")
    : '<span style="color:#9aa0a8;">No message</span>';

  return '' +
  '<div style="background:#f1f2f4;padding:32px 12px;font-family:Arial,Helvetica,sans-serif;">' +
    '<table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="max-width:600px;margin:0 auto;background:#ffffff;border-radius:8px;overflow:hidden;">' +

      // Header with logo
      '<tr><td style="background:#1c1f26;padding:28px 32px;text-align:left;">' +
        '<img src="' + LOGO_URL + '" alt="Tri-Stone Construction Services" height="44" style="display:block;height:44px;width:auto;border:0;">' +
      '</td></tr>' +
      '<tr><td style="background:#ff6a00;height:4px;font-size:0;line-height:0;">&nbsp;</td></tr>' +

      // Title
      '<tr><td style="padding:32px 32px 8px;">' +
        '<p style="margin:0 0 8px;font-size:12px;font-weight:700;letter-spacing:.14em;text-transform:uppercase;color:#ff6a00;">New Inquiry</p>' +
        '<h1 style="margin:0;font-size:24px;line-height:1.3;color:#1c1f26;">' + esc(d.name) + ' wants to talk about ' + esc(d.service) + '</h1>' +
        '<p style="margin:10px 0 0;font-size:13px;color:#7b8089;">Received ' + when + '</p>' +
      '</td></tr>' +

      // Details
      '<tr><td style="padding:16px 32px 8px;">' +
        '<table role="presentation" width="100%" cellpadding="0" cellspacing="0">' +
          row("Name", esc(d.name)) +
          row("Email", '<a href="mailto:' + esc(d.email) + '" style="color:#ff6a00;text-decoration:none;">' + esc(d.email) + '</a>') +
          row("Phone", phone) +
          row("Service", esc(d.service)) +
          (d.location ? row("Location", esc(d.location)) : '') +
        '</table>' +
      '</td></tr>' +

      // Message
      '<tr><td style="padding:16px 32px 8px;">' +
        '<p style="margin:0 0 8px;font-size:12px;font-weight:700;letter-spacing:.06em;text-transform:uppercase;color:#7b8089;">Message</p>' +
        '<div style="background:#f4f5f7;border-left:4px solid #ff6a00;border-radius:4px;padding:16px 18px;font-size:15px;line-height:1.6;color:#1c1f26;">' + message + '</div>' +
      '</td></tr>' +

      // Reply button
      '<tr><td style="padding:24px 32px 36px;">' +
        '<a href="mailto:' + esc(d.email) + '?subject=' + encodeURIComponent("Re: Your inquiry to Tri-Stone Construction") + '" ' +
          'style="display:inline-block;background:#ff6a00;color:#ffffff;font-size:14px;font-weight:700;letter-spacing:.05em;text-decoration:none;padding:14px 28px;border-radius:4px;">REPLY TO ' + esc(d.name).toUpperCase() + '</a>' +
      '</td></tr>' +

      // Footer
      '<tr><td style="background:#1c1f26;padding:20px 32px;text-align:center;">' +
        '<p style="margin:0;font-size:12px;line-height:1.6;color:#aeb3bb;">Sent from the contact form on your Tri-Stone website.<br>Tri-Stone Construction Services &middot; Quezon City, Metro Manila</p>' +
      '</td></tr>' +

    '</table>' +
  '</div>';
}

// ---------- The website's Contact form posts here ----------
function doPost(e) {
  try {
    const d = JSON.parse(e.postData.contents);

    // Save first, so the message is kept even if the email fails.
    // If saving fails, the email still goes out and says so.
    let saveProblem = "";
    try { saveInquiry(d); } catch (err) { saveProblem = String(err); }

    const plain =
      "New inquiry from your website\n\n" +
      "Name: " + d.name + "\n" +
      "Email: " + d.email + "\n" +
      "Phone: " + (d.phone || "Not provided") + "\n" +
      "Service: " + d.service + "\n" +
      (d.location ? "Location: " + d.location + "\n" : "") + "\n" +
      "Message:\n" + (d.message || "No message") +
      (saveProblem ? "\n\n(Not saved for Website Admin: " + saveProblem + ")" : "");

    MailApp.sendEmail({
      to: TO_EMAIL,
      replyTo: d.email,
      subject: "New inquiry from " + d.name + " (" + d.service + ")",
      body: plain,
      htmlBody: buildHtml(d) + (saveProblem ? '<p style="font-family:Arial;color:#b00;">Not saved for Website Admin: ' + esc(saveProblem) + '</p>' : ''),
      name: "Tri-Stone Website"
    });

    return json({ ok: true });
  } catch (err) {
    return json({ ok: false, error: String(err) });
  }
}
