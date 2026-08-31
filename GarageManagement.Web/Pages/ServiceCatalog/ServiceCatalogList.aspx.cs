using GarageManagement.Web.BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GarageManagement.Web.Pages.ServiceCatalog
{
    public partial class ServiceCatalogList : System.Web.UI.Page
    {
        private readonly ServiceCatalogBAL _bal = new ServiceCatalogBAL();

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
                gvServiceCatalog.DataSource = _bal.GetAllServiceCatalog();
                gvServiceCatalog.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to load service catalog. " + ex.Message, isError: true);
            }
        }

        protected void gvServiceCatalog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvServiceCatalog.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvServiceCatalog_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int serviceId = Convert.ToInt32(e.CommandArgument);
                try
                {
                    _bal.DeleteServiceCatalog(serviceId);
                    ShowMessage("Service deleted.", isError: false);
                }
                catch (Exception ex)
                {
                    ShowMessage("Unable to delete service. " + ex.Message, isError: true);
                }
                BindGrid();
            }
        }

        protected void gvServiceCatalog_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Deletion is handled via RowCommand above — intentional no-op.
        }

        private void ShowMessage(string message, bool isError)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isError ? "msc-message msc-message-error" : "msc-message msc-message-success";
            lblMessage.Visible = true;
        }
    }
}