"""
Generates plain WinForms .Designer.cs files from a short spec, so big forms don't have to be typed by hand.
The output is normal designer code: Visual Studio's designer opens and edits it like any other form.

Usage:  python gen_designer.py <spec.py> <output .Designer.cs>
A spec file defines: NAMESPACE, CLASS, SIZE, CONTROLS, GRID_COLUMNS, EVENTS, LOAD_EVENT.
"""
import runpy
import sys


def T(s):
    """C# string literal."""
    return '"' + s.replace('\\', '\\\\').replace('"', '\\"') + '"'


DDL = {"DropDownStyle": "ComboBoxStyle.DropDownList"}
TB = "DataGridViewTextBoxColumn"
NEEDS_INIT = ("DataGridView", "NumericUpDown", "PictureBox")


def generate(ns, cls, size, controls, grid_columns, events, load_event=None, is_form=False):
    out = []
    w = out.append
    w(f"namespace {ns}")
    w("{")
    w(f"    partial class {cls}")
    w("    {")
    w("        private System.ComponentModel.IContainer components = null;")
    w("")
    w("        protected override void Dispose(bool disposing)")
    w("        {")
    w("            if (disposing && (components != null))")
    w("            {")
    w("                components.Dispose();")
    w("            }")
    w("            base.Dispose(disposing);")
    w("        }")
    w("")
    w("        #region Component Designer generated code")
    w("")
    w("        private void InitializeComponent()")
    w("        {")
    for t, n, *_ in controls:
        w(f"            {n} = new {t}();")
    for cols in grid_columns.values():
        for ct, cn, *_ in cols:
            w(f"            {cn} = new {ct}();")
    for t, n, *_ in controls:
        if t in NEEDS_INIT:
            w(f"            ((System.ComponentModel.ISupportInitialize){n}).BeginInit();")
    w("            SuspendLayout();")

    tab = 0
    for t, n, x, y, cw, ch, extras in controls:
        w("            // ")
        w(f"            // {n}")
        w("            // ")
        auto = t in ("Label", "CheckBox") and cw is None
        if auto:
            w(f"            {n}.AutoSize = true;")
        if t == "DataGridView":
            names = ", ".join(c[1] for c in grid_columns[n])
            w(f"            {n}.AllowUserToAddRows = false;")
            w(f"            {n}.AllowUserToDeleteRows = false;")
            w(f"            {n}.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;")
            w(f"            {n}.Columns.AddRange(new DataGridViewColumn[] {{ {names} }});")
            w(f"            {n}.MultiSelect = false;")
            w(f"            {n}.ReadOnly = true;")
            w(f"            {n}.RowHeadersVisible = false;")
            w(f"            {n}.SelectionMode = DataGridViewSelectionMode.FullRowSelect;")
        w(f"            {n}.Location = new Point({x}, {y});")
        w(f"            {n}.Name = {T(n)};")
        if not auto and cw is not None:
            w(f"            {n}.Size = new Size({cw}, {ch});")
        for k, v in extras.items():
            w(f"            {n}.{k} = {v};")
        if t != "Label":
            w(f"            {n}.TabIndex = {tab};")
            tab += 1
        for event, handler in events.get(n, []):
            w(f"            {n}.{event} += {handler};")

    for cols in grid_columns.values():
        for ct, cn, prop, header, extras in cols:
            w("            // ")
            w(f"            // {cn}")
            w("            // ")
            w(f"            {cn}.DataPropertyName = {T(prop)};")
            w(f"            {cn}.HeaderText = {T(header)};")
            w(f"            {cn}.Name = {T(cn)};")
            for k, v in extras.items():
                w(f"            {cn}.{k} = {v};")

    w("            // ")
    w(f"            // {cls}")
    w("            // ")
    w("            AutoScaleDimensions = new SizeF(7F, 15F);")
    w("            AutoScaleMode = AutoScaleMode.Font;")
    if is_form:
        # A pop-up window: fixed size, centered on the window that opened it.
        w(f"            ClientSize = new Size({size[0]}, {size[1]});")
        w("            FormBorderStyle = FormBorderStyle.FixedDialog;")
        w("            MaximizeBox = false;")
        w("            MinimizeBox = false;")
        w("            StartPosition = FormStartPosition.CenterParent;")
    else:
        w("            AutoScroll = true;")
    for t, n, *_ in controls:
        w(f"            Controls.Add({n});")
    w(f"            Name = {T(cls)};")
    if not is_form:
        w(f"            Size = new Size({size[0]}, {size[1]});")
    if load_event:
        w(f"            Load += {load_event};")
    for t, n, *_ in controls:
        if t in NEEDS_INIT:
            w(f"            ((System.ComponentModel.ISupportInitialize){n}).EndInit();")
    w("            ResumeLayout(false);")
    w("            PerformLayout();")
    w("        }")
    w("")
    w("        #endregion")
    w("")
    for t, n, *_ in controls:
        w(f"        private {t} {n};")
    for cols in grid_columns.values():
        for ct, cn, *_ in cols:
            w(f"        private {ct} {cn};")
    w("    }")
    w("}")
    return "\n".join(out) + "\n"


if __name__ == "__main__":
    spec = runpy.run_path(sys.argv[1], init_globals={"T": T, "DDL": DDL, "TB": TB})
    code = generate(spec["NAMESPACE"], spec["CLASS"], spec["SIZE"], spec["CONTROLS"],
                    spec.get("GRID_COLUMNS", {}), spec.get("EVENTS", {}), spec.get("LOAD_EVENT"), spec.get("FORM", False))
    with open(sys.argv[2], "w", encoding="utf-8") as f:
        f.write(code)
    print("written", sys.argv[2])
