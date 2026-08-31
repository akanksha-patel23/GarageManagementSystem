using GarageManagement.Web.BAL;
using System;
using System.Web.UI.WebControls;

namespace GarageManagement.Web.Pages.Invoices
{
    public partial class InvoiceList : System.Web.UI.Page
    {
        private readonly InvoiceBAL _bal = new InvoiceBAL();

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
                lvInvoices.DataSource = _bal.GetAllInvoices();
                lvInvoices.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to load invoices. " + ex.Message, isError: true);
            }
        }

        protected void lvInvoices_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "MarkPaid")
            {
                int invoiceId = Convert.ToInt32(e.CommandArgument);
                try
                {
                    _bal.UpdatePaymentStatus(invoiceId, "Paid");
                    ShowMessage($"Invoice #{invoiceId} marked as paid.", isError: false);
                }
                catch (Exception ex)
                {
                    ShowMessage("Unable to update payment status. " + ex.Message, isError: true);
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
