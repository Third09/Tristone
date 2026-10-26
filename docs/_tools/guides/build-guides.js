// Turns the Markdown sources in docs/_source into Word files in docs/.
// Step 1 (here): Markdown -> styled HTML, with <!-- include: path --> replaced by that file's code.
// Step 2 (to-word.ps1): Word opens the HTML and saves it as .docx.
//
// Usage:  node build-guides.js        (then run to-word.ps1, or use build-guides.cmd for both)

const fs = require("fs");
const path = require("path");
const { marked } = require("marked");

const ROOT = path.resolve(__dirname, "..", "..", "..");      // TriStone/
const SRC = path.join(ROOT, "docs", "_source");
const OUT = path.join(__dirname, "html");
fs.mkdirSync(OUT, { recursive: true });

const LANG = { ".sql": "sql", ".ps1": "powershell", ".cmd": "bat", ".cs": "csharp", ".json": "json", ".js": "javascript", ".sh": "bash", ".xml": "xml", ".csproj": "xml", ".slnx": "xml" };

// <!-- include: database/001_login_security.sql --> becomes a code block with the file's current contents.
function expandIncludes(md) {
  return md.replace(/<!--\s*include:\s*(.+?)\s*-->/g, (_, rel) => {
    const file = path.join(ROOT, rel);
    const code = fs.readFileSync(file, "utf8").replace(/\s+$/, "");
    const lang = LANG[path.extname(file).toLowerCase()] || "";
    return `*File: \`${rel}\`*\n\n\`\`\`${lang}\n${code}\n\`\`\``;
  });
}

const CSS = `
  body { font-family: Calibri, sans-serif; font-size: 11pt; color: #1a1a1a; }
  h1 { font-size: 22pt; color: #1F3864; border-bottom: 2px solid #1F3864; padding-bottom: 4px; margin-top: 24pt; }
  h2 { font-size: 15pt; color: #2E5597; margin-top: 18pt; }
  h3 { font-size: 12.5pt; color: #2E5597; margin-top: 12pt; }
  code { font-family: Consolas, monospace; font-size: 9.5pt; background: #F2F2F2; }
  pre { font-family: Consolas, monospace; font-size: 9pt; background: #F5F7FA; border: 1px solid #D0D7E2; padding: 8px; }
  pre code { background: none; }
  table { border-collapse: collapse; margin: 6pt 0; }
  th { background: #1F3864; color: #FFFFFF; padding: 4px 8px; border: 1px solid #BFBFBF; text-align: left; }
  td { padding: 4px 8px; border: 1px solid #BFBFBF; vertical-align: top; }
  blockquote { border-left: 4px solid #2E5597; margin-left: 0; padding-left: 10px; color: #404040; background: #EEF2F8; }
`;

for (const f of fs.readdirSync(SRC).filter(n => n.endsWith(".md"))) {
  const md = expandIncludes(fs.readFileSync(path.join(SRC, f), "utf8"));
  const title = path.basename(f, ".md");
  const html = `<!DOCTYPE html><html><head><meta charset="utf-8"><title>${title}</title><style>${CSS}</style></head><body>${marked.parse(md)}</body></html>`;
  fs.writeFileSync(path.join(OUT, `${title}.html`), html);
  console.log("html:", title);
}
