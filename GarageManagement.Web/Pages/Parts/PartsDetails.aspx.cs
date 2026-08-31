using GarageManagement.Web.BAL;
using GarageManagement.Web.Entities;
using System;

namespace GarageManagement.Web.Pages.Parts
{
    public partial class PartsDetails : System.Web.UI.Page
    {
        private readonly PartBAL _bal = new PartBAL();

        private int? PartId
        {
            get
            {
                if (int.TryParse(Request.QueryString["PartID"], out int id))
                    return id;
                return null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && PartId.HasValue)
            {
                // No spGetPartById exists (trimmed per SP scope decision) — reuse
                // GetAllParts() and find client-side in the BAL.
                var part = _bal.GetAllParts().Find(p => p.PartID == PartId.Value);
                if (part == null)
                {
                    ShowMessage("Part not found.", isError: true);
                    return;
                }

                txtPartName.Text = part.PartName;
                txtStockQty.Text = part.StockQty.ToString();
                txtStockQty.Enabled = false; // editing stock here would bypass the +1/-1 audit trail
                lblStockHint.Visible = true;
                txtUnitPrice.Text = part.UnitPrice.ToString("0.00");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                var part = new Part
                {
                    PartID = PartId ?? 0,
                    PartName = txtPartName.Text.Trim(),
                    //StockQty = int.TryParse(txtStockQty.Text, out int stock) ? stock : 0,
                    //UnitPrice = decimal.TryParse(txtUnitPrice.Text, out decimal price) ? price : 0m
                    StockQty = Convert.ToInt32(txtStockQty.Text),
                    UnitPrice = Convert.ToDecimal(txtUnitPrice.Text)
                };

                if (PartId.HasValue)
                    _bal.UpdatePart(part);  // StockQty ignored server-side by spUpdatePart
                else
                    _bal.InsertPart(part);

                Response.Redirect("~/Pages/Parts/PartsList.aspx");
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to save part. " + ex.Message, isError: true);
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
