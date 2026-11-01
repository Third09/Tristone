using Company.OfficeApp.UI;
using Company.Shared.Models;
using Company.WebsiteAdmin.Services;

namespace Company.WebsiteAdmin.Pages;

/// <summary>
/// One inquiry, opened from Contact Us: read the message, copy the email to reply from Gmail, and follow it up
/// (status New / Contacted / Closed + internal notes). Save lists the changes in a Yes/No message first.
/// </summary>
public partial class InquiryForm : Form
{
    record FollowUp(string Status, string? Notes);

    readonly Inquiry inquiry;
    readonly FollowUp original;

    /// <summary>True when the follow-up was saved (the list then reloads).</summary>
    public bool Saved { get; private set; }

    public InquiryForm(Inquiry i)
    {
        InitializeComponent();
        inquiry = i;
        Text = $"Inquiry from {i.FullName}";
        CancelButton = btnCloseInquiry;
        cboInquiryStatus.Items.AddRange(InquiryStatuses.All);
        txtInquiryNotes.MaxLength = WebsiteInquiries.MaxNotesLength;
        lblInqName.Text = $"Name: {i.FullName}";
        lblInqContact.Text = $"Contact number: {i.Phone ?? "-"}";
        lblInqEmail.Text = $"Email: {i.Email ?? "-"}";
        btnCopyEmail.Enabled = i.Email != null;
        lblInqLocation.Text = $"Project location: {i.ProjectLocation ?? "-"}";
        lblInqService.Text = $"Service required: {i.ServiceRequired ?? "-"}";
        lblInqReceived.Text = $"Received: {i.ReceivedAt:MMM d, yyyy h:mm tt}";
        txtInqMessage.Text = i.Message.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
        cboInquiryStatus.SelectedItem = i.Status;
        txtInquiryNotes.Text = i.Notes ?? "";
        original = ReadForm();
    }

    FollowUp ReadForm() => new((string?)cboInquiryStatus.SelectedItem ?? "", string.IsNullOrWhiteSpace(txtInquiryNotes.Text) ? null : txtInquiryNotes.Text.Trim());

    void btnCopyEmail_Click(object sender, EventArgs e)
    {
        if (inquiry.Email is not { } email) return;
        Clipboard.SetText(email);
        lblInquiryMsg.Text = $"Copied {email}. Paste it in Gmail to reply.";
    }

    async void btnSaveInquiry_Click(object sender, EventArgs e)
    {
        var input = ReadForm();
        var answer = RecordForm.ConfirmSave(this, $"the inquiry from {inquiry.FullName}", original, input);
        if (answer == SaveAnswer.NothingChanged) { lblInquiryMsg.Text = "Nothing changed."; return; }
        if (answer == SaveAnswer.No) { lblInquiryMsg.Text = "Not saved."; return; }
        btnSaveInquiry.Enabled = false;
        try
        {
            var error = await WebsiteInquiries.UpdateAsync(inquiry.InquiryId, input.Status, input.Notes);
            if (error != null) { lblInquiryMsg.Text = error; return; }
            Saved = true;
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) { lblInquiryMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnSaveInquiry.Enabled = true; }
    }
}
