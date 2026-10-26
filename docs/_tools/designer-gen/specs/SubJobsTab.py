# Spec for src/Company.OfficeApp/Pages/Subcontractors/SubJobsTab.Designer.cs
NAMESPACE = "Company.OfficeApp.Pages.Subcontractors"
CLASS = "SubJobsTab"
SIZE = (960, 870)
LOAD_EVENT = "SubJobsTab_Load"
MONEY = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" }'
PCT = 'new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "0.##\'%\'" }'
DATE = 'new DataGridViewCellStyle { Format = "MMM d, yyyy" }'
STAMP = 'new DataGridViewCellStyle { Format = "MMM d, yyyy h:mm tt" }'

CONTROLS = [
    ("Label", "lblProjectFilter", 12, 15, None, None, {"Text": T("Project")}),
    ("ComboBox", "cboJobProjectFilter", 62, 12, 220, 23, DDL),
    ("Label", "lblSubFilter", 294, 15, None, None, {"Text": T("Subcontractor")}),
    ("ComboBox", "cboJobSubFilter", 380, 12, 220, 23, DDL),
    ("DataGridView", "dgvJobs", 12, 44, 930, 150, {}),
    ("Button", "btnNewJob", 12, 202, 90, 28, {"Text": T("New Job")}),
    ("Button", "btnDeleteJob", 108, 202, 90, 28, {"Text": T("Delete")}),
    ("Label", "lblFormMode", 12, 244, None, None, {"Text": T("New job")}),
    # job form (left)
    ("Label", "lblJobSubcontractor", 12, 268, None, None, {"Text": T("Subcontractor")}),
    ("ComboBox", "cboJobSubcontractor", 12, 286, 260, 23, DDL),
    ("Label", "lblJobProject", 12, 316, None, None, {"Text": T("Project")}),
    ("ComboBox", "cboJobProject", 12, 334, 260, 23, DDL),
    ("Label", "lblJobScope", 12, 364, None, None, {"Text": T("Scope of work")}),
    ("TextBox", "txtJobScope", 12, 382, 260, 56, {"Multiline": "true", "ScrollBars": "ScrollBars.Vertical"}),
    ("Label", "lblJobAmount", 12, 446, None, None, {"Text": T("Contract amount")}),
    ("NumericUpDown", "numJobAmount", 12, 464, 130, 23, {"DecimalPlaces": "2", "ThousandsSeparator": "true", "Maximum": "999999999999m"}),
    ("Label", "lblRetentionPct", 152, 446, None, None, {"Text": T("Retention %")}),
    ("NumericUpDown", "numRetentionPct", 152, 464, 70, 23, {"DecimalPlaces": "2", "Maximum": "99m", "Value": "10m"}),
    ("Label", "lblJobStart", 12, 494, None, None, {"Text": T("Start date")}),
    ("DateTimePicker", "dtpJobStart", 12, 512, 125, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblJobEnd", 147, 494, None, None, {"Text": T("End date")}),
    ("DateTimePicker", "dtpJobEnd", 147, 512, 125, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblJobMsg", 12, 546, None, None, {}),
    ("Button", "btnSaveJob", 12, 568, 90, 28, {"Text": T("Save Job")}),
    ("Button", "btnCancelJob", 108, 568, 90, 28, {"Text": T("Cancel")}),
    # payment stages (middle)
    ("Label", "lblStages", 300, 268, None, None, {"Text": T("Payment stages (must add up to 100%)")}),
    ("DataGridView", "dgvJobStages", 300, 286, 300, 150, {"ReadOnly": "false", "SelectionMode": "DataGridViewSelectionMode.CellSelect"}),
    ("Label", "lblStagesTotal", 300, 444, None, None, {"Text": T("Total: 100%")}),
    ("Button", "btnAddStage", 300, 466, 100, 28, {"Text": T("Add Stage")}),
    ("Button", "btnRemoveStage", 406, 466, 110, 28, {"Text": T("Remove Stage")}),
    # supporting papers (right)
    ("Label", "lblJobDocs", 628, 268, None, None, {"Text": T("Supporting papers (contract, scope)")}),
    ("DataGridView", "dgvJobDocs", 628, 286, 314, 150, {}),
    ("Button", "btnUploadJobDoc", 628, 444, 130, 28, {"Text": T("Upload Document")}),
    ("Button", "btnViewJobDoc", 764, 444, 80, 28, {"Text": T("View")}),
    # progress (bottom)
    ("Label", "lblProgressTitle", 12, 612, None, None, {"Text": T("Progress (selected job)")}),
    ("ProgressBar", "prgJobProgress", 12, 632, 260, 20, {}),
    ("Label", "lblProgressPct", 280, 634, None, None, {"Text": T("")}),
    ("Label", "lblProgressPctInput", 12, 662, None, None, {"Text": T("% completed")}),
    ("NumericUpDown", "numProgressPct", 12, 680, 70, 23, {"DecimalPlaces": "0"}),
    ("Label", "lblProgressDate", 92, 662, None, None, {"Text": T("Date")}),
    ("DateTimePicker", "dtpProgressDate", 92, 680, 125, 23, {"Format": "DateTimePickerFormat.Short"}),
    ("Label", "lblProgressNotes", 227, 662, None, None, {"Text": T("Notes")}),
    ("TextBox", "txtProgressNotes", 227, 680, 300, 23, {}),
    ("Button", "btnAttachProgressFile", 537, 678, 130, 28, {"Text": T("Attach Photo / File")}),
    ("Button", "btnAddProgressUpdate", 673, 678, 110, 28, {"Text": T("Add Update")}),
    ("Label", "lblProgressFile", 537, 710, None, None, {"Text": T("")}),
    ("Label", "lblProgressMsg", 12, 710, None, None, {}),
    ("DataGridView", "dgvProgressHistory", 12, 734, 930, 120, {}),
]

GRID_COLUMNS = {
    "dgvJobs": [
        (TB, "colSub", "Subcontractor", "Subcontractor", {}),
        (TB, "colProject", "Project", "Project", {}),
        (TB, "colScope", "Scope", "Scope", {}),
        (TB, "colAmount", "ContractAmount", "Contract Amount", {"DefaultCellStyle": MONEY}),
        (TB, "colDone", "PercentDone", "% Done", {"DefaultCellStyle": PCT}),
        (TB, "colStatus", "Status", "Status", {}),
    ],
    "dgvJobStages": [
        (TB, "colStageName", "Name", "Stage Name", {}),
        (TB, "colStagePct", "Percent", "%", {"DefaultCellStyle": PCT}),
        (TB, "colStageAmount", "Amount", "Amount", {"DefaultCellStyle": MONEY, "ReadOnly": "true"}),
    ],
    "dgvJobDocs": [
        (TB, "colDocFile", "OriginalName", "File", {}),
        (TB, "colDocUploaded", "UploadedAt", "Uploaded", {"DefaultCellStyle": STAMP}),
    ],
    "dgvProgressHistory": [
        (TB, "colPDate", "Date", "Date", {"DefaultCellStyle": DATE}),
        (TB, "colPPct", "Percent", "%", {"DefaultCellStyle": PCT}),
        (TB, "colPNotes", "Notes", "Notes", {}),
        (TB, "colPFile", "FileName", "Files", {}),
    ],
}

EVENTS = {
    "cboJobProjectFilter": [("SelectedIndexChanged", "Filter_Changed")],
    "cboJobSubFilter": [("SelectedIndexChanged", "Filter_Changed")],
    "dgvJobs": [("SelectionChanged", "dgvJobs_SelectionChanged")],
    "btnNewJob": [("Click", "btnNewJob_Click")],
    "btnDeleteJob": [("Click", "btnDeleteJob_Click")],
    "numJobAmount": [("ValueChanged", "Stages_Changed")],
    "btnSaveJob": [("Click", "btnSaveJob_Click")],
    "btnCancelJob": [("Click", "btnCancelJob_Click")],
    "dgvJobStages": [("CellEndEdit", "dgvJobStages_CellEndEdit"), ("DataError", "dgvJobStages_DataError")],
    "btnAddStage": [("Click", "btnAddStage_Click")],
    "btnRemoveStage": [("Click", "btnRemoveStage_Click")],
    "btnUploadJobDoc": [("Click", "btnUploadJobDoc_Click")],
    "btnViewJobDoc": [("Click", "btnViewJobDoc_Click")],
    "dgvProgressHistory": [("CellDoubleClick", "dgvProgressHistory_CellDoubleClick")],
    "btnAttachProgressFile": [("Click", "btnAttachProgressFile_Click")],
    "btnAddProgressUpdate": [("Click", "btnAddProgressUpdate_Click")],
}
