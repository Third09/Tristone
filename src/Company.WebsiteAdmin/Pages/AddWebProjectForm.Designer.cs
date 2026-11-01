namespace Company.WebsiteAdmin.Pages
{
    partial class AddWebProjectForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            lblChooseProject = new Label();
            cboProjectChoice = new ComboBox();
            lblNewProjectDesc = new Label();
            lblNewProjectDescCount = new Label();
            txtNewProjectDesc = new TextBox();
            lblAddProjectMsg = new Label();
            btnAddProjectOk = new Button();
            btnCancelAddProject = new Button();
            SuspendLayout();
            // 
            // lblChooseProject
            // 
            lblChooseProject.AutoSize = true;
            lblChooseProject.Location = new Point(12, 12);
            lblChooseProject.Name = "lblChooseProject";
            lblChooseProject.Text = "Project (from the office Projects list, ticked \"Show on website\")";
            // 
            // cboProjectChoice
            // 
            cboProjectChoice.Location = new Point(12, 32);
            cboProjectChoice.Name = "cboProjectChoice";
            cboProjectChoice.Size = new Size(436, 23);
            cboProjectChoice.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProjectChoice.TabIndex = 0;
            // 
            // lblNewProjectDesc
            // 
            lblNewProjectDesc.AutoSize = true;
            lblNewProjectDesc.Location = new Point(12, 66);
            lblNewProjectDesc.Name = "lblNewProjectDesc";
            lblNewProjectDesc.Text = "Project short description";
            // 
            // lblNewProjectDescCount
            // 
            lblNewProjectDescCount.AutoSize = true;
            lblNewProjectDescCount.Location = new Point(396, 66);
            lblNewProjectDescCount.Name = "lblNewProjectDescCount";
            lblNewProjectDescCount.Text = "0/100";
            // 
            // txtNewProjectDesc
            // 
            txtNewProjectDesc.Location = new Point(12, 84);
            txtNewProjectDesc.Name = "txtNewProjectDesc";
            txtNewProjectDesc.Size = new Size(436, 23);
            txtNewProjectDesc.TabIndex = 1;
            // 
            // lblAddProjectMsg
            // 
            lblAddProjectMsg.AutoSize = true;
            lblAddProjectMsg.Location = new Point(12, 116);
            lblAddProjectMsg.Name = "lblAddProjectMsg";
            lblAddProjectMsg.MaximumSize = new Size(436, 0);
            // 
            // btnAddProjectOk
            // 
            btnAddProjectOk.Location = new Point(262, 190);
            btnAddProjectOk.Name = "btnAddProjectOk";
            btnAddProjectOk.Size = new Size(90, 28);
            btnAddProjectOk.Text = "Add";
            btnAddProjectOk.TabIndex = 2;
            btnAddProjectOk.Click += btnAddProjectOk_Click;
            // 
            // btnCancelAddProject
            // 
            btnCancelAddProject.Location = new Point(358, 190);
            btnCancelAddProject.Name = "btnCancelAddProject";
            btnCancelAddProject.Size = new Size(90, 28);
            btnCancelAddProject.Text = "Cancel";
            btnCancelAddProject.DialogResult = DialogResult.Cancel;
            btnCancelAddProject.TabIndex = 3;
            // 
            // AddWebProjectForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(460, 230);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Controls.Add(lblChooseProject);
            Controls.Add(cboProjectChoice);
            Controls.Add(lblNewProjectDesc);
            Controls.Add(lblNewProjectDescCount);
            Controls.Add(txtNewProjectDesc);
            Controls.Add(lblAddProjectMsg);
            Controls.Add(btnAddProjectOk);
            Controls.Add(btnCancelAddProject);
            Name = "AddWebProjectForm";
            Load += AddWebProjectForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblChooseProject;
        private ComboBox cboProjectChoice;
        private Label lblNewProjectDesc;
        private Label lblNewProjectDescCount;
        private TextBox txtNewProjectDesc;
        private Label lblAddProjectMsg;
        private Button btnAddProjectOk;
        private Button btnCancelAddProject;
    }
}
