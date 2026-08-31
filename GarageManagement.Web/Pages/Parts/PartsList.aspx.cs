using System;
using System.Web.UI.WebControls;
using GarageManagement.Web.BAL;

namespace GarageManagement.Web.Pages.Parts
{
    public partial class PartsList : System.Web.UI.Page
    {
        private readonly PartBAL _bal = new PartBAL();

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
                gvParts.DataSource = _bal.GetAllParts();
                gvParts.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to load parts. " + ex.Message, isError: true);
            }
        }

        protected void gvParts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvParts.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvParts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int partId = Convert.ToInt32(e.CommandArgument);

            try
            {
                switch (e.CommandName)
                {
                    case "Delete":
                        _bal.DeletePart(partId);
                        ShowMessage("Part deleted.", isError: false);
                        break;

                    case "StockUp":
                        _bal.AdjustStock(partId, 1);
                        ShowMessage("Stock increased.", isError: false);
                        break;

                    case "StockDown":
                        _bal.AdjustStock(partId, -1);
                        ShowMessage("Stock decreased.", isError: false);
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Operation failed. " + ex.Message, isError: true);
            }

            BindGrid();
        }

        protected void gvParts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Deletion is handled via RowCommand above - intentional no-op.
        }

        private void ShowMessage(string message, bool isError)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isError ? "msc-message msc-message-error" : "msc-message msc-message-success";
            lblMessage.Visible = true;
        }
    }
}
