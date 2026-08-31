using System;
using System.Web.UI.WebControls;
using GarageManagement.Web.BAL;
using GarageManagement.Web.Entities;

namespace GarageManagement.Web.Pages.Customers
{
    public partial class CustomerDetails : System.Web.UI.Page
    {
        private readonly CustomerBAL _bal = new CustomerBAL();

        private int? CustomerId
        {
            get
            {
                if (int.TryParse(Request.QueryString["CustomerID"], out int id))
                    return id;
                return null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CustomerId.HasValue)
                {
                    var customer = _bal.GetCustomerById(CustomerId.Value);
                    if (customer == null)
                    {
                        ShowMessage("Customer not found.", isError: true);
                        dvCustomer.Visible = false;
                        return;
                    }

                    dvCustomer.DataSource = new[] { customer };
                    dvCustomer.DataBind();
                    dvCustomer.ChangeMode(DetailsViewMode.Edit);
                }
                else
                {
                    dvCustomer.DataSource = new[] { new Customer() };
                    dvCustomer.DataBind();
                    dvCustomer.ChangeMode(DetailsViewMode.Insert);
                }
            }
        }

        protected void dvCustomer_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            if (!Page.IsValid) { e.Cancel = true; return; }

            try
            {
                var customer = new Customer
                {
                    Name = e.Values["Name"]?.ToString(),
                    Phone = e.Values["Phone"]?.ToString(),
                    Email = e.Values["Email"]?.ToString(),
                    Address = e.Values["Address"]?.ToString()
                };

                _bal.InsertCustomer(customer);
                //Response.Redirect("~/Pages/Customers/CustomerList.aspx");
                dvCustomer.ChangeMode(DetailsViewMode.ReadOnly);
                if (CustomerId.HasValue)
                {
                    BindCustomerData();
                }
                ShowMessage("Customer added successfully.", isError: false);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                ShowMessage("Unable to save customer. " + ex.Message, isError: true);
            }
        }

        protected void dvCustomer_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            dvCustomer.ChangeMode(e.NewMode);
            if (e.NewMode == DetailsViewMode.Insert)
            {
                dvCustomer.DataSource = new[] { new Customer() };
                dvCustomer.DataBind();
            }
            else if (CustomerId.HasValue)
            {
                BindCustomerData();
            }
        }

        private void BindCustomerData()
        {
            var customer = _bal.GetCustomerById(CustomerId.Value);
            dvCustomer.DataSource = new[] { customer };
            dvCustomer.DataBind();
        }

        protected void dvCustomer_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            if (!Page.IsValid) { e.Cancel = true; return; }

            try
            {
                var customer = new Customer
                {
                    CustomerID = CustomerId ?? 0,
                    Name = e.NewValues["Name"]?.ToString(),
                    Phone = e.NewValues["Phone"]?.ToString(),
                    Email = e.NewValues["Email"]?.ToString(),
                    Address = e.NewValues["Address"]?.ToString()
                };

                _bal.UpdateCustomer(customer);
                //Response.Redirect("~/Pages/Customers/CustomerList.aspx");
                dvCustomer.ChangeMode(DetailsViewMode.ReadOnly);
                BindCustomerData();
                ShowMessage("Customer updated successfully.", isError: false);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                ShowMessage("Unable to update customer. " + ex.Message, isError: true);
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