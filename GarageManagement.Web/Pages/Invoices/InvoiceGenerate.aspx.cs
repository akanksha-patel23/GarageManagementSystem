using GarageManagement.Web.BAL;
using GarageManagement.Web.Entities;
using System;
using System.Web.UI;

namespace GarageManagement.Web.Pages.Invoices
{
    public partial class InvoiceGenerate : System.Web.UI.Page
    {
        private readonly InvoiceBAL _bal = new InvoiceBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                var invoice = new Invoice
                {
                    JobCardID = Convert.ToInt32(txtJobCardId.Text),
                    TotalAmount = Convert.ToDecimal(txtTotalAmount.Text),
                    PaymentMode = rblPaymentMode.SelectedValue
                };

                _bal.InsertInvoice(invoice);
                Response.Redirect("~/Pages/Invoices/InvoiceList.aspx");
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to generate invoice. " + ex.Message, isError: true);
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