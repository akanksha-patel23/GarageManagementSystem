using System;
using System.Web.UI.WebControls;
using GarageManagement.Web.BAL;

namespace GarageManagement.Web.Pages.Vehicles
{
    public partial class VehicleList : System.Web.UI.Page
    {
        private readonly VehicleBAL _bal = new VehicleBAL();

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
                gvVehicles.DataSource = _bal.GetAllVehicles();
                gvVehicles.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to load vehicles. " + ex.Message, isError: true);
            }
        }

        protected void gvVehicles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvVehicles.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvVehicles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int vehicleId = Convert.ToInt32(e.CommandArgument);
                try
                {
                    _bal.DeleteVehicle(vehicleId);
                    ShowMessage("Vehicle deleted.", isError: false);
                }
                catch (Exception ex)
                {
                    ShowMessage("Unable to delete vehicle. " + ex.Message, isError: true);
                }
                BindGrid();
            }
        }

        protected void gvVehicles_RowDeleting(object sender, GridViewDeleteEventArgs e)
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