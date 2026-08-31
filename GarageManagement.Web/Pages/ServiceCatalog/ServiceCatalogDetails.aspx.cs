using GarageManagement.Web.BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GarageManagement.Web.Pages.ServiceCatalog
{
    public partial class ServiceCatalogDetails : System.Web.UI.Page
    {
        private readonly ServiceCatalogBAL _bal = new ServiceCatalogBAL();
        private readonly ServiceCategoryBAL _categoryBal = new ServiceCategoryBAL();

        private int? ServiceId
        {
            get
            {
                if (int.TryParse(Request.QueryString["ServiceID"], out int id))
                    return id;
                return null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCategoryDropDown();

                if (ServiceId.HasValue)
                {
                    var service = _bal.GetAllServiceCatalog().Find(s => s.ServiceID == ServiceId.Value);
                    if (service == null)
                    {
                        ShowMessage("Service not found.", isError: true);
                        return;
                    }

                    ddlCategory.SelectedValue = service.CategoryID.ToString();
                    txtServiceName.Text = service.ServiceName;
                    txtBasePrice.Text = service.BasePrice.ToString("0.00");
                    txtEstimatedHours.Text = service.EstimatedHours?.ToString("0.0");
                }
            }
        }

        private void BindCategoryDropDown()
        {
            try
            {
                var categories = _categoryBal.GetCategoryTree();
                foreach (var category in categories)
                {
                    string prefix = category.Depth > 0 ? new string('-', category.Depth * 2) + " " : string.Empty;
                    ddlCategory.Items.Add(new System.Web.UI.WebControls.ListItem(
                        prefix + category.CategoryName, category.CategoryID.ToString()));
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to load categories. " + ex.Message, isError: true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                var service = new GarageManagement.Web.Entities.ServiceCatalog
                {
                    ServiceID = ServiceId ?? 0,
                    CategoryID = Convert.ToInt32(ddlCategory.SelectedValue),
                    ServiceName = txtServiceName.Text.Trim(),
                    BasePrice = Convert.ToDecimal(txtBasePrice.Text),
                    EstimatedHours = string.IsNullOrWhiteSpace(txtEstimatedHours.Text)
                        ? (decimal?)null
                        : Convert.ToDecimal(txtEstimatedHours.Text)
                };

                if (ServiceId.HasValue)
                    _bal.UpdateServiceCatalog(service);
                else
                    _bal.InsertServiceCatalog(service);

                Response.Redirect("~/Pages/ServiceCatalog/ServiceCatalogList.aspx");
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to save service. " + ex.Message, isError: true);
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