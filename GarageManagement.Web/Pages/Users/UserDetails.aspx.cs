using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GarageManagement.Web.BAL;

namespace GarageManagement.Web.Pages.Users
{
    public partial class UserDetails : System.Web.UI.Page
    {
        private readonly UserBAL _bal = new UserBAL();
 
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
 
            try
            {
                _bal.CreateUser(
                    txtUsername.Text.Trim(),
                    txtPassword.Text,
                    txtFullName.Text.Trim(),
                    rblRole.SelectedValue);
 
                Response.Redirect("~/Pages/Users/UserList.aspx");
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to create user. " + ex.Message, isError: true);
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