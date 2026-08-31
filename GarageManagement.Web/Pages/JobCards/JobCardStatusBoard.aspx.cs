using GarageManagement.Web.BAL;
using System;

namespace GarageManagement.Web.Pages.JobCards
{
    public partial class JobCardStatusBoard : System.Web.UI.Page
    {
        private readonly JobCardBAL _bal = new JobCardBAL();

        private int? JobCardId
        {
            get
            {
                if (int.TryParse(Request.QueryString["JobCardID"], out int id))
                    return id;
                return null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!JobCardId.HasValue)
            {
                ShowMessage("No JobCardID specified.", isError: true);
                statusTimer.Enabled = false;
                return;
            }

            if (!IsPostBack)
            {
                BindStatusBoard();
            }
        }

        protected void statusTimer_Tick(object sender, EventArgs e)
        {
            BindStatusBoard();
        }

        private void BindStatusBoard()
        {
            try
            {
                var jobCard = _bal.GetJobCardById(JobCardId.Value);
                if (jobCard == null)
                {
                    ShowMessage("Job card not found.", isError: true);
                    statusTimer.Enabled = false;
                    return;
                }

                lblCurrentStatus.Text = $"Job Card #{jobCard.JobCardId} — Current Status: {jobCard.Status}";

                rptStatusHistory.DataSource = _bal.GetStatusHistory(JobCardId.Value);
                rptStatusHistory.DataBind();

                // Stop polling once the job card reaches a terminal state.
                if (jobCard.Status == "Completed" || jobCard.Status == "Cancelled")
                {
                    statusTimer.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to load status history. " + ex.Message, isError: true);
            }
        }

        private void ShowMessage(string message, bool isError)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isError ? "msc-message msc-message-error" : "msc-message msc-message-success";
            lblMessage.Visible = true;
        }
    }
}
