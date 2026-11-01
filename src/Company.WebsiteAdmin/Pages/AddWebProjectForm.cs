using Company.WebsiteAdmin.Services;

namespace Company.WebsiteAdmin.Pages;

/// <summary>
/// Pop-up for Projects > Add New Project: pick an office project ticked "Show on website" (not featured yet)
/// and give it a short description. Photos are added on the Projects page afterwards.
/// </summary>
public partial class AddWebProjectForm : Form
{
    /// <summary>The new featured project's id after Add; null when cancelled.</summary>
    public int? NewId { get; private set; }

    public AddWebProjectForm()
    {
        InitializeComponent();
        Text = "Add New Project";
        AcceptButton = btnAddProjectOk;
        CancelButton = btnCancelAddProject;
        EditorUi.Counter(txtNewProjectDesc, lblNewProjectDescCount, WebProjectService.MaxDescription);
        cboProjectChoice.DisplayMember = nameof(ProjectChoice.Text);
    }

    async void AddWebProjectForm_Load(object sender, EventArgs e)
    {
        try
        {
            var choices = await WebProjectService.ChoicesAsync();
            cboProjectChoice.DataSource = choices;
            cboProjectChoice.SelectedIndex = -1;   // never pick one by itself
            if (choices.Count == 0)
            {
                lblAddProjectMsg.Text = "No project is waiting. In the office app's Projects page, tick \"Show on website\" for a project first.";
                btnAddProjectOk.Enabled = false;
            }
        }
        catch (Exception ex) { lblAddProjectMsg.Text = "Can't reach the database. " + ex.Message; }
    }

    async void btnAddProjectOk_Click(object sender, EventArgs e)
    {
        if (cboProjectChoice.SelectedItem is not ProjectChoice choice) { lblAddProjectMsg.Text = "Choose a project."; return; }
        btnAddProjectOk.Enabled = false;
        try
        {
            var (id, error) = await WebProjectService.AddAsync(choice.ProjectId, txtNewProjectDesc.Text);
            if (id == null) { lblAddProjectMsg.Text = error; return; }
            NewId = id;
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) { lblAddProjectMsg.Text = "Can't reach the database. " + ex.Message; }
        finally { btnAddProjectOk.Enabled = true; }
    }
}
