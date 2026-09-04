using GarageManagement.Web.BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GarageManagement.Web
{
    public partial class Register : System.Web.UI.Page
    {
        private readonly UserBAL _userBal = new UserBAL();

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                // Role is hardcoded here, never taken from the form — self-registration
                // must never be able to create an Admin account. Admin accounts are
                // create-only-by-an-existing-admin, via Pages/Users/UserDetails.aspx.
                _userBal.CreateUser(
                    txtUsername.Text.Trim(),
                    txtPassword.Text,
                    txtFullName.Text.Trim(),
                    "Customer");

                Response.Redirect("~/Login.aspx?registered=1");
            }
            catch (Exception ex)
            {
                // Duplicate usernames surface here as a generic "Database operation
                // failed" message via spInsertUser's UNIQUE constraint + GLOG pattern —
                // consistent with how every other duplicate-key case in this project
                // is handled, rather than adding special-case detection just for this form.
                ShowMessage("Unable to create account. " + ex.Message);
            }
        }

        private void ShowMessage(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = true;
        }
    }
}