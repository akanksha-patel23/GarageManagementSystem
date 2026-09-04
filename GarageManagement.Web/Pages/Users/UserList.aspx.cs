using GarageManagement.Web.BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GarageManagement.Web.Pages.Users
{
    public partial class UserList : System.Web.UI.Page
    {
        private readonly UserBAL _bal = new UserBAL();

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
                gvUsers.DataSource = _bal.GetAllUsers();
                gvUsers.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to load users. " + ex.Message, isError: true);
            }
        }

        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ToggleActive")
            {
                int userId = Convert.ToInt32(e.CommandArgument);
                try
                {
                    var user = _bal.GetAllUsers().FirstOrDefault(u => u.UserID == userId);
                    if (user != null)
                    {
                        _bal.SetUserActive(userId, !user.IsActive);
                        ShowMessage($"User '{user.Username}' {(!user.IsActive ? "enabled" : "disabled")}.", isError: false);
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Unable to update user. " + ex.Message, isError: true);
                }
                BindGrid();
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