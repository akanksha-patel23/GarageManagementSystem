using GarageManagement.Web.BAL;
using GarageManagement.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GarageManagement.Web.Pages.Technicians
{
    public partial class TechnicianDetails : System.Web.UI.Page
    {
        private readonly TechnicianBAL _bal = new TechnicianBAL();

        private int? TechnicianId
        {
            get
            {
                if (int.TryParse(Request.QueryString["TechnicianID"], out int id))
                    return id;
                return null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && TechnicianId.HasValue)
            {
                // No spGetTechnicianById exists (trimmed per the SP scope decision) —
                // reuse GetAllTechnicians() and find client-side in the BAL, same
                // philosophy as not adding a new SP for a single lookup.
                var technician = _bal.GetAllTechnicians().Find(t => t.TechnicianID == TechnicianId.Value);
                if (technician == null)
                {
                    ShowMessage("Technician not found.", isError: true);
                    return;
                }

                txtName.Text = technician.Name;
                txtSpecialization.Text = technician.Specialization;
                txtContactNumber.Text = technician.ContactNumber;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                var technician = new Technician
                {
                    TechnicianID = TechnicianId ?? 0,
                    Name = txtName.Text.Trim(),
                    Specialization = txtSpecialization.Text.Trim(),
                    ContactNumber = txtContactNumber.Text.Trim()
                };

                if (TechnicianId.HasValue)
                    _bal.UpdateTechnician(technician);
                else
                    _bal.InsertTechnician(technician);

                Response.Redirect("~/Pages/Technicians/TechnicianDirectory.aspx");
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to save technician. " + ex.Message, isError: true);
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