namespace Company.WebsiteAdmin.Pages
{
    partial class InquiryForm
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
            lblInqName = new Label();
            lblInqContact = new Label();
            lblInqEmail = new Label();
            btnCopyEmail = new Button();
            btnReplyGmail = new Button();
            lblInqLocation = new Label();
            lblInqService = new Label();
            lblInqReceived = new Label();
            lblInqMessage = new Label();
            txtInqMessage = new TextBox();
            lblInquiryStatus = new Label();
            cboInquiryStatus = new ComboBox();
            lblInquiryNotes = new Label();
            txtInquiryNotes = new TextBox();
            lblInquiryMsg = new Label();
            btnSaveInquiry = new Button();
            btnCloseInquiry = new Button();
            SuspendLayout();
            // 
            // lblInqName
            // 
            lblInqName.AutoSize = true;
            lblInqName.Location = new Point(38, 32);
            lblInqName.Name = "lblInqName";
            lblInqName.Text = "Name: -";
            // 
            // lblInqContact
            // 
            lblInqContact.AutoSize = true;
            lblInqContact.Location = new Point(38, 61);
            lblInqContact.Name = "lblInqContact";
            lblInqContact.Text = "Contact number: -";
            // 
            // lblInqEmail
            // 
            lblInqEmail.AutoSize = true;
            lblInqEmail.Location = new Point(38, 90);
            lblInqEmail.Name = "lblInqEmail";
            lblInqEmail.Text = "Email: -";
            // 
            // btnCopyEmail
            // 
            btnCopyEmail.Location = new Point(518, 82);
            btnCopyEmail.Name = "btnCopyEmail";
            btnCopyEmail.Size = new Size(136, 36);
            btnCopyEmail.Text = "Copy Email";
            btnCopyEmail.TabIndex = 0;
            btnCopyEmail.Click += btnCopyEmail_Click;
            //
            // btnReplyGmail  (opens a Gmail reply with the client's email and a short greeting filled in)
            //
            btnReplyGmail.Location = new Point(518, 124);
            btnReplyGmail.Name = "btnReplyGmail";
            btnReplyGmail.Size = new Size(136, 36);
            btnReplyGmail.Text = "Reply in Gmail";
            btnReplyGmail.TabIndex = 1;
            btnReplyGmail.Click += btnReplyGmail_Click;
            // 
            // lblInqLocation
            // 
            lblInqLocation.AutoSize = true;
            lblInqLocation.Location = new Point(38, 119);
            lblInqLocation.Name = "lblInqLocation";
            lblInqLocation.Text = "Project location: -";
            // 
            // lblInqService
            // 
            lblInqService.AutoSize = true;
            lblInqService.Location = new Point(38, 148);
            lblInqService.Name = "lblInqService";
            lblInqService.Text = "Service required: -";
            // 
            // lblInqReceived
            // 
            lblInqReceived.AutoSize = true;
            lblInqReceived.Location = new Point(38, 177);
            lblInqReceived.Name = "lblInqReceived";
            lblInqReceived.Text = "Received: -";
            // 
            // lblInqMessage
            // 
            lblInqMessage.AutoSize = true;
            lblInqMessage.Location = new Point(38, 211);
            lblInqMessage.Name = "lblInqMessage";
            lblInqMessage.Text = "Message";
            // 
            // txtInqMessage
            // 
            txtInqMessage.Location = new Point(38, 235);
            txtInqMessage.Name = "txtInqMessage";
            txtInqMessage.Size = new Size(616, 158);
            txtInqMessage.Multiline = true;
            txtInqMessage.ReadOnly = true;
            txtInqMessage.ScrollBars = ScrollBars.Vertical;
            txtInqMessage.TabIndex = 1;
            // 
            // lblInquiryStatus
            // 
            lblInquiryStatus.AutoSize = true;
            lblInquiryStatus.Location = new Point(38, 409);
            lblInquiryStatus.Name = "lblInquiryStatus";
            lblInquiryStatus.Text = "Status";
            // 
            // cboInquiryStatus
            // 
            cboInquiryStatus.Location = new Point(38, 433);
            cboInquiryStatus.Name = "cboInquiryStatus";
            cboInquiryStatus.Size = new Size(184, 23);
            cboInquiryStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboInquiryStatus.TabIndex = 2;
            // 
            // lblInquiryNotes
            // 
            lblInquiryNotes.AutoSize = true;
            lblInquiryNotes.Location = new Point(38, 473);
            lblInquiryNotes.Name = "lblInquiryNotes";
            lblInquiryNotes.Text = "Internal notes (never shown on the website)";
            // 
            // txtInquiryNotes
            // 
            txtInquiryNotes.Location = new Point(38, 496);
            txtInquiryNotes.Name = "txtInquiryNotes";
            txtInquiryNotes.Size = new Size(616, 119);
            txtInquiryNotes.Multiline = true;
            txtInquiryNotes.ScrollBars = ScrollBars.Vertical;
            txtInquiryNotes.TabIndex = 3;
            // 
            // lblInquiryMsg
            // 
            lblInquiryMsg.AutoSize = true;
            lblInquiryMsg.Location = new Point(38, 623);
            lblInquiryMsg.Name = "lblInquiryMsg";
            lblInquiryMsg.MaximumSize = new Size(330, 0);
            // 
            // btnSaveInquiry
            // 
            btnSaveInquiry.Location = new Point(440, 652);
            btnSaveInquiry.Name = "btnSaveInquiry";
            btnSaveInquiry.Size = new Size(103, 36);
            btnSaveInquiry.Text = "Save";
            btnSaveInquiry.TabIndex = 4;
            btnSaveInquiry.Click += btnSaveInquiry_Click;
            // 
            // btnCloseInquiry
            // 
            btnCloseInquiry.Location = new Point(551, 652);
            btnCloseInquiry.Name = "btnCloseInquiry";
            btnCloseInquiry.Size = new Size(103, 36);
            btnCloseInquiry.Text = "Close";
            btnCloseInquiry.DialogResult = DialogResult.Cancel;
            btnCloseInquiry.TabIndex = 5;
            // 
            // InquiryForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(692, 718);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Controls.Add(lblInqName);
            Controls.Add(lblInqContact);
            Controls.Add(lblInqEmail);
            Controls.Add(btnCopyEmail);
            Controls.Add(btnReplyGmail);
            Controls.Add(lblInqLocation);
            Controls.Add(lblInqService);
            Controls.Add(lblInqReceived);
            Controls.Add(lblInqMessage);
            Controls.Add(txtInqMessage);
            Controls.Add(lblInquiryStatus);
            Controls.Add(cboInquiryStatus);
            Controls.Add(lblInquiryNotes);
            Controls.Add(txtInquiryNotes);
            Controls.Add(lblInquiryMsg);
            Controls.Add(btnSaveInquiry);
            Controls.Add(btnCloseInquiry);
            Name = "InquiryForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblInqName;
        private Label lblInqContact;
        private Label lblInqEmail;
        private Button btnCopyEmail;
        private Button btnReplyGmail;
        private Label lblInqLocation;
        private Label lblInqService;
        private Label lblInqReceived;
        private Label lblInqMessage;
        private TextBox txtInqMessage;
        private Label lblInquiryStatus;
        private ComboBox cboInquiryStatus;
        private Label lblInquiryNotes;
        private TextBox txtInquiryNotes;
        private Label lblInquiryMsg;
        private Button btnSaveInquiry;
        private Button btnCloseInquiry;
    }
}
