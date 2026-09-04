using GarageManagement.Web.BAL;
using System;
using System.Web.UI;

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
                //ShowMessage("No JobCardID specified.", isError: true);
                //statusTimer.Enabled = false;

                // The Web.sitemap links to this page without a query string so
                // users can reach it from the menu. Redirect to the job card list
                // rather than stranding them on a blank error page — they can
                // then click through to a specific card's status board from there.
                Response.Redirect("~/Pages/JobCards/JobCardList.aspx");
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
