using GarageManagement.Web.BAL;
using GarageManagement.Web.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace GarageManagement.Web.Pages.Invoices
{
    public partial class InvoiceGenerate : System.Web.UI.Page
    {
        private readonly InvoiceBAL _bal = new InvoiceBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateJobCardDropdown();
                PopulateJobCardDataForClient();
            }
        }

        private void PopulateJobCardDropdown()
        {
            DataTable dt = _bal.GetAllJobCardsForDropdown();
            
            ddlJobCardId.DataSource = dt;
            ddlJobCardId.DataBind();
        }

        private void PopulateJobCardDataForClient()
        {
            DataTable dt = _bal.GetAllJobCardsForDropdown();
            var jobCards = new List<dynamic>();

            foreach (DataRow row in dt.Rows)
            {
                jobCards.Add(new
                {
                    JobCardID = Convert.ToInt32(row["JobCardID"]),
                    TotalAmount = Convert.ToDecimal(row["TotalAmount"])
                });
            }

            hfJobCardData.Value = JsonConvert.SerializeObject(jobCards);
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            // Validate job card selection
            if (string.IsNullOrEmpty(ddlJobCardId.SelectedValue))
            {
                ShowMessage("Please select a job card.", isError: true);
                return;
            }

            // Validate total amount from hidden field (read-only fields don't postback their values)
            if (!decimal.TryParse(hfTotalAmount.Value, out decimal totalAmount) || totalAmount < 0)
            {
                ShowMessage("Please select a valid job card with a total amount.", isError: true);
                return;
            }

            try
            {
                var invoice = new Invoice
                {
                    JobCardID = Convert.ToInt32(ddlJobCardId.SelectedValue),
                    TotalAmount = totalAmount,
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