using GarageManagement.Web.BAL;
using System;
using System.Web.UI.WebControls;

namespace GarageManagement.Web.Pages.Technicians
{
    public partial class TechnicianDirectory : System.Web.UI.Page
    {
        private readonly TechnicianBAL _bal = new TechnicianBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindList();
            }
        }

        private void BindList()
        {
            try
            {
                dlTechnicians.DataSource = _bal.GetAllTechnicians();
                dlTechnicians.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to load technicians. " + ex.Message, isError: true);
            }
        }

        protected void dlTechnicians_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int technicianId = Convert.ToInt32(e.CommandArgument);
                try
                {
                    _bal.DeleteTechnician(technicianId);
                    ShowMessage("Technician deleted.", isError: false);
                }
                catch (Exception ex)
                {
                    ShowMessage("Unable to delete technician. " + ex.Message, isError: true);
                }
                BindList();
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
