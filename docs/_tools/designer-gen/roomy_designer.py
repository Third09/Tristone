"""
Applies the ROOMY layout (see gen_designer.roomy) to a hand-made .Designer.cs that has no spec, by editing its
Location / Size lines. Run once per file; it refuses to run twice (it leaves a marker comment).

Usage:  python roomy_designer.py <file.Designer.cs> [<file2> ...]
"""
import re
import sys

from gen_designer import SX, SY, OX, OY, TALL, SECTION_FONT

MARK = "// ROOMY layout applied"


def apply(path):
    s = open(path, encoding="utf-8").read()
    if MARK in s:
        print("already roomy:", path)
        return
    types = dict(re.findall(r"^\s+(\w+) = new ([\w.]+)\(\);", s, re.M))
    nested = set(re.findall(r"^\s+\w+\.Controls\.Add\((\w+)\);", s, re.M))
    multiline = set(re.findall(r"^\s+(\w+)\.Multiline = true;", s, re.M))

    def loc(m):
        name, x, y = m.group(1), int(m.group(2)), int(m.group(3))
        off = (0, 0) if name in nested else (OX, OY)
        return f"{m.group(0).split('=')[0]}= new Point({round(x * SX) + off[0]}, {round(y * SY) + off[1]});"

    def size(m):
        name, w, h = m.group(1), int(m.group(2)), int(m.group(3))
        t = types.get(name, "")
        w2 = round(w * SX)
        if t.endswith("Button"):
            h2 = max(36, h)
        elif any(t.endswith(k) for k in TALL) or name in multiline or t.endswith("GroupBox"):
            h2 = round(h * SY)
        else:
            h2 = h
        return f"{m.group(0).split('=')[0]}= new Size({w2}, {h2});"

    s = re.sub(r"(?m)^\s+(\w+)\.Location = new Point\((\d+), (\d+)\);", loc, s)
    s = re.sub(r"(?m)^\s+(\w+)\.Size = new Size\((\d+), (\d+)\);", size, s)
    # the page / window itself
    s = re.sub(r"(?m)^(\s+)(Size|ClientSize) = new Size\((\d+), (\d+)\);",
               lambda m: f"{m.group(1)}{m.group(2)} = new Size({round(int(m.group(3)) * SX) + 2 * OX}, {round(int(m.group(4)) * SY) + 2 * OY});", s)
    # section title
    s = re.sub(r"(?m)^(\s+)lblFormMode\.AutoSize = true;", lambda m: f"{m.group(0)}\n{m.group(1)}lblFormMode.Font = {SECTION_FONT};", s, count=1)
    s = s.replace("        private void InitializeComponent()", f"        {MARK} (docs/_tools/designer-gen/roomy_designer.py)\n        private void InitializeComponent()", 1)
    open(path, "w", encoding="utf-8", newline="").write(s)
    print("roomy:", path)


if __name__ == "__main__":
    for p in sys.argv[1:]:
        apply(p)
