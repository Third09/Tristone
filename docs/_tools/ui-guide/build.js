const fs = require("fs");
const {
  Document, Packer, Paragraph, TextRun, Table, TableRow, TableCell, HeadingLevel,
  WidthType, ShadingType, PageOrientation, BorderStyle, AlignmentType, LevelFormat,
} = require("docx");
const { shared, dashboard, projects, websiteAdmin } = require("./data");
const { hrPayroll, inventory, subcontractors, receipts } = require("./data2");

const out = process.argv[2];
const FONT = "Calibri";
// A4 landscape, 0.6" margins -> content width 11906+... use DXA
const PAGE_W = 11906, PAGE_H = 16838, MARGIN = 864;
const CONTENT_W = PAGE_H - 2 * MARGIN; // landscape width
const COLS = [2900, 3300, 2700, CONTENT_W - 2900 - 3300 - 2700];
const HEAD_FILL = "1F3864", ALT_FILL = "EEF2F8", COMP_FILL = "F2F2F2";

const border = { style: BorderStyle.SINGLE, size: 4, color: "BFBFBF" };
const borders = { top: border, bottom: border, left: border, right: border };

const p = (text, opts = {}) => new Paragraph({
  spacing: { after: opts.after ?? 80 },
  ...(opts.bullet ? { numbering: { reference: "bullets", level: 0 } } : {}),
  children: (Array.isArray(text) ? text : [text]).map(t =>
    typeof t === "string" ? new TextRun({ text: t, font: FONT, size: 21, ...opts.run }) : t),
});
const bold = (t) => new TextRun({ text: t, bold: true, font: FONT, size: 21 });
const plain = (t) => new TextRun({ text: t, font: FONT, size: 21 });

function cell(text, i, { header = false, fill } = {}) {
  return new TableCell({
    width: { size: COLS[i], type: WidthType.DXA },
    borders,
    shading: fill ? { type: ShadingType.CLEAR, color: "auto", fill } : undefined,
    margins: { top: 60, bottom: 60, left: 100, right: 100 },
    children: [new Paragraph({ children: [new TextRun({
      text, font: i === 2 && !header ? "Consolas" : FONT, size: header ? 20 : 20,
      bold: header, color: header ? "FFFFFF" : undefined,
    })] })],
  });
}

function table(rows) {
  const head = new TableRow({ tableHeader: true, children:
    ["Element (plain word, for Figma)", "WinForms control (Toolbox)", "Suggested (Name)", "What it does"]
      .map((t, i) => cell(t, i, { header: true, fill: HEAD_FILL })) });
  const body = rows.map((r, n) => {
    const comp = hidden(r[1]);
    const fill = comp ? COMP_FILL : (n % 2 ? ALT_FILL : undefined);
    return new TableRow({ cantSplit: true, children: r.map((t, i) => cell(t, i, { fill })) });
  });
  return new Table({ width: { size: CONTENT_W, type: WidthType.DXA }, columnWidths: COLS, rows: [head, ...body] });
}

// Rows that need no design: dragged components, or ones the code creates itself.
const hidden = (c) => c.startsWith("(component)") || c.startsWith("(made in code");

function countOf(screen, pred) {
  return screen.groups.flatMap(g => g.rows).filter(r => pred(r[1])).length;
}

function screenBlock(s) {
  const buttons = countOf(s, c => c === "Button");
  const links = countOf(s, c => c === "LinkLabel");
  const all = countOf(s, c => !hidden(c));
  const out = [
    new Paragraph({ heading: HeadingLevel.HEADING_2, spacing: { before: 280, after: 100 }, children: [new TextRun({ text: s.title, font: FONT })] }),
    p([bold("In Visual Studio: "), plain(s.form)]),
    p([bold("Opens from: "), plain(s.opensFrom)]),
    p([bold("Who sees it: "), plain(s.whoSees)]),
    p([bold("Count: "), plain(`${buttons} button${buttons === 1 ? "" : "s"}${links ? `, ${links} link` : ""}, ${all} visible element${all === 1 ? "" : "s"} in total`)]),
  ];
  if (s.note) out.push(p(s.note, { run: { italics: true, color: "404040" } }));
  for (const g of s.groups) {
    out.push(new Paragraph({ spacing: { before: 160, after: 60 }, keepNext: true,
      children: [new TextRun({ text: g.name, bold: true, font: FONT, size: 22, color: HEAD_FILL })] }));
    out.push(table(g.rows));
  }
  return out;
}

const prefixes = [
  ["Button", "Button", "btn", "btnSave"],
  ["Text box", "TextBox", "txt", "txtUsername"],
  ["Text / label", "Label", "lbl", "lblTitle"],
  ["Clickable text link", "LinkLabel", "lnk", "lnkForgotPassword"],
  ["Dropdown", "ComboBox", "cbo", "cboStatus"],
  ["Tick box / On-Off switch", "CheckBox", "chk", "chkShowClient"],
  ["Date picker", "DateTimePicker", "dtp", "dtpStart"],
  ["Table", "DataGridView", "dgv", "dgvInquiries"],
  ["Simple list", "ListBox", "lst", "lstSections"],
  ["Image box", "PictureBox", "pic", "picLogo"],
  ["Box / area", "Panel", "pnl", "pnlSidebar"],
  ["Labelled box (frame with a title)", "GroupBox", "grp", "grpChangePassword"],
  ["Tiles area (items flow side by side)", "FlowLayoutPanel", "flp", "flpGallery"],
  ["Tabs", "TabControl / TabPage", "tab", "tabWebsiteAdmin"],
  ["Number box (money, quantities)", "NumericUpDown", "num", "numContractPrice"],
  ["Tick list (pick several)", "CheckedListBox", "clb", "clbWorkDays"],
  ["Pick-one option (round dot)", "RadioButton", "rdo", "rdoOwned"],
  ["Progress bar", "ProgressBar", "prg", "prgJobProgress"],
  ["Chart", "FormsPlot (ScottPlot add-on)", "chart", "chartProjectStatus"],
];

const PREFIX_COLS = [4200, 4200, 2000, CONTENT_W - 10400];
const prefixTable = new Table({
  width: { size: CONTENT_W, type: WidthType.DXA }, columnWidths: PREFIX_COLS,
  rows: [["Plain word", "WinForms control", "Name starts with", "Example"], ...prefixes].map((r, n) =>
    new TableRow({ children: r.map((t, i) => new TableCell({
      width: { size: PREFIX_COLS[i], type: WidthType.DXA }, borders,
      shading: n === 0 ? { type: ShadingType.CLEAR, color: "auto", fill: HEAD_FILL } : (n % 2 === 0 ? { type: ShadingType.CLEAR, color: "auto", fill: ALT_FILL } : undefined),
      margins: { top: 50, bottom: 50, left: 100, right: 100 },
      children: [new Paragraph({ children: [new TextRun({ text: t, font: i === 3 && n ? "Consolas" : FONT, size: 20, bold: n === 0, color: n === 0 ? "FFFFFF" : undefined })] })],
    })) })),
});

const children = [
  new Paragraph({ heading: HeadingLevel.TITLE, children: [new TextRun({ text: "Tri-Stone Office App - UI Elements Guide", font: FONT })] }),
  p("What each screen needs: how many buttons, what each one does, and the other elements. For the UI designer (Figma) and for building the screens in Visual Studio (WinForms)."),
  new Paragraph({ heading: HeadingLevel.HEADING_1, children: [new TextRun({ text: "How to read this guide", font: FONT })] }),
  p("Every element has three names:", { after: 40 }),
  p([bold("Plain word: "), plain("what to call it in Figma (button, text box, table...).")], { bullet: true }),
  p([bold("WinForms control: "), plain("the item to drag from the Visual Studio Toolbox.")], { bullet: true }),
  p([bold("Suggested (Name): "), plain("what to type in the (Name) box of the Properties window, so the design, the screen and the code all use the same name.")], { bullet: true }),
  p("Placement is up to the designer. This guide only fixes two things: the sidebar is on the left, and every page loads inside the Main section of the Main Window.", { after: 40 }),
  p("Grey rows marked (component) or (made in code) are invisible while the app runs (timers, file pickers, print windows). They need no design.", { after: 160 }),
  prefixTable,
  new Paragraph({ heading: HeadingLevel.HEADING_1, pageBreakBefore: true, children: [new TextRun({ text: "Part A: Screens everyone sees", font: FONT })] }),
  ...shared.flatMap(screenBlock),
  new Paragraph({ heading: HeadingLevel.HEADING_1, pageBreakBefore: true, children: [new TextRun({ text: "Part 1: Dashboard", font: FONT })] }),
  ...dashboard.flatMap(screenBlock),
  new Paragraph({ heading: HeadingLevel.HEADING_1, pageBreakBefore: true, children: [new TextRun({ text: "Part 1b: Projects", font: FONT })] }),
  ...projects.flatMap(screenBlock),
  new Paragraph({ heading: HeadingLevel.HEADING_1, pageBreakBefore: true, children: [new TextRun({ text: "Part 2: HR & Payroll (and User Accounts)", font: FONT })] }),
  ...hrPayroll.flatMap(screenBlock),
  new Paragraph({ heading: HeadingLevel.HEADING_1, pageBreakBefore: true, children: [new TextRun({ text: "Part 3: Inventory & Vendors", font: FONT })] }),
  ...inventory.flatMap(screenBlock),
  new Paragraph({ heading: HeadingLevel.HEADING_1, pageBreakBefore: true, children: [new TextRun({ text: "Part 4: Subcontractors", font: FONT })] }),
  ...subcontractors.flatMap(screenBlock),
  new Paragraph({ heading: HeadingLevel.HEADING_1, pageBreakBefore: true, children: [new TextRun({ text: "Part 5: Receipts", font: FONT })] }),
  ...receipts.flatMap(screenBlock),
  new Paragraph({ heading: HeadingLevel.HEADING_1, pageBreakBefore: true, children: [new TextRun({ text: "Part 6: Website Admin (separate app)", font: FONT })] }),
  ...websiteAdmin.flatMap(screenBlock),
];

const doc = new Document({
  styles: {
    default: { document: { run: { font: FONT, size: 21 } } },
    paragraphStyles: [
      { id: "Heading1", name: "Heading 1", basedOn: "Normal", next: "Normal", quickFormat: true,
        run: { size: 32, bold: true, color: HEAD_FILL, font: FONT }, paragraph: { spacing: { before: 240, after: 120 }, outlineLevel: 0 } },
      { id: "Heading2", name: "Heading 2", basedOn: "Normal", next: "Normal", quickFormat: true,
        run: { size: 26, bold: true, color: "2E5597", font: FONT }, paragraph: { spacing: { before: 240, after: 100 }, outlineLevel: 1 } },
    ],
  },
  numbering: { config: [{ reference: "bullets", levels: [{ level: 0, format: LevelFormat.BULLET, text: "•", alignment: AlignmentType.LEFT,
    style: { paragraph: { indent: { left: 540, hanging: 270 } } } }] }] },
  sections: [{
    properties: { page: { size: { width: PAGE_W, height: PAGE_H, orientation: PageOrientation.LANDSCAPE },
      margin: { top: MARGIN, bottom: MARGIN, left: MARGIN, right: MARGIN } } },
    children,
  }],
});

Packer.toBuffer(doc).then(b => { fs.writeFileSync(out, b); console.log("written", out); });
