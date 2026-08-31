using System;
using System.Web.UI.WebControls;
using GarageManagement.Web.BAL;

namespace GarageManagement.Web.Pages.Customers
{
    public partial class CustomerList : System.Web.UI.Page
    {
        private readonly CustomerBAL _bal = new CustomerBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid(string searchTerm = null)
        {
            try
            {
                gvCustomers.DataSource = string.IsNullOrWhiteSpace(searchTerm)
                    ? _bal.GetAllCustomers()
                    : _bal.SearchCustomers(searchTerm);
                gvCustomers.DataBind();
            }
            catch (Exception ex)
            {
                // Log full exception details for debugging
                System.Diagnostics.EventLog.WriteEntry("Application",
                    $"CustomerList.BindGrid failed: {ex.GetType().FullName}: {ex.Message}\n{ex.StackTrace}",
                    System.Diagnostics.EventLogEntryType.Error);
                ShowMessage("Unable to load customers. " + ex.Message, isError: true);
            }
        }

        protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomers.PageIndex = e.NewPageIndex;
            BindGrid(txtSearch.Text);
        }

        protected void gvCustomers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int customerId = Convert.ToInt32(e.CommandArgument);
                try
                {
                    _bal.DeleteCustomer(customerId);
                    ShowMessage("Customer deleted.", isError: false);
                }
                catch (Exception ex)
                {
                    ShowMessage("Unable to delete customer. " + ex.Message, isError: true);
                }
                BindGrid(txtSearch.Text);
            }
        }

        protected void gvCustomers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Deletion is handled via RowCommand (LinkButton CommandName="Delete") above.
            // This handler exists only because GridView requires it when a Delete
            // CommandName is present on a row control � intentional no-op.
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvCustomers.PageIndex = 0;
            BindGrid(txtSearch.Text);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            gvCustomers.PageIndex = 0;
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