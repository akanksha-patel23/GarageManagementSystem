using System;
using System.Web.UI.WebControls;
using GarageManagement.Web.BAL;

namespace GarageManagement.Web.Pages.JobCards
{
    public partial class JobCardList : System.Web.UI.Page
    {
        private readonly JobCardBAL _bal = new JobCardBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            try
            {
                gvJobCards.DataSource = _bal.GetAllJobCards();
                gvJobCards.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to load job cards. " + ex.Message, isError: true);
            }
        }

        protected void gvJobCards_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvJobCards.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvJobCards_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int jobCardId = Convert.ToInt32(e.CommandArgument);

            try
            {
                switch (e.CommandName)
                {
                    case "Advance":
                        var jobCard = _bal.GetJobCardById(jobCardId);
                        string nextStatus = _bal.GetNextStatus(jobCard.Status);
                        if (nextStatus != null)
                        {
                            _bal.UpdateStatus(jobCardId, nextStatus, $"Advanced to {nextStatus} from Job Cards list.");
                            ShowMessage($"Job card #{jobCardId} moved to {nextStatus}.", isError: false);
                        }
                        break;

                    case "Cancel":
                        _bal.UpdateStatus(jobCardId, "Cancelled", "Cancelled from Job Cards list.");
                        ShowMessage($"Job card #{jobCardId} cancelled.", isError: false);
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Operation failed. " + ex.Message, isError: true);
            }

            BindGrid();
        }

        private void ShowMessage(string message, bool isError)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isError ? "msc-message msc-message-error" : "msc-message msc-message-success";
            lblMessage.Visible = true;
        }
    }
}
