using GarageManagement.Web.BAL;
using System;
using System.Configuration;

namespace GarageManagement.Web.Pages.JobCards
{
    public partial class JobCardWizard : System.Web.UI.Page
    {
        private readonly CustomerBAL _customerBal = new CustomerBAL();
        private readonly ServiceCategoryBAL _categoryBal = new ServiceCategoryBAL();
        private readonly TechnicianBAL _technicianBal = new TechnicianBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            hdnApiBaseUrl.Value = ConfigurationManager.AppSettings["ApiBaseUrl"];

            if (!IsPostBack)
            {
                BindCustomerDropDown();
                BindCategoryDropDown();
                BindTechnicianDropDown();
            }
        }

        private void BindCustomerDropDown()
        {
            try
            {
                ddlCustomer.DataSource = _customerBal.GetAllCustomers();
                ddlCustomer.DataBind();
            }
            catch (Exception ex)
            {
                ShowWizardError("Unable to load customers. " + ex.Message);
            }
        }

        private void BindCategoryDropDown()
        {
            try
            {
                var categories = _categoryBal.GetCategoryTree();
                ddlCategory.Items.Clear();
                ddlCategory.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Category --", ""));

                foreach (var category in categories)
                {
                    // Indent by Depth so the flat tree reads as a hierarchy in a
                    // plain dropdown (e.g. Engine -> "-- Oil Change").
                    string prefix = category.Depth > 0 ? new string('-', category.Depth * 2) + " " : string.Empty;
                    ddlCategory.Items.Add(new System.Web.UI.WebControls.ListItem(
                        prefix + category.CategoryName, category.CategoryID.ToString()));
                }
            }
            catch (Exception ex)
            {
                ShowWizardError("Unable to load service categories. " + ex.Message);
            }
        }

        private void BindTechnicianDropDown()
        {
            try
            {
                foreach (var technician in _technicianBal.GetAllTechnicians())
                {
                    ddlTechnician.Items.Add(new System.Web.UI.WebControls.ListItem(
                        technician.Name, technician.TechnicianID.ToString()));
                }
            }
            catch (Exception ex)
            {
                ShowWizardError("Unable to load technicians. " + ex.Message);
            }
        }

        protected void calScheduledDate_SelectionChanged(object sender, EventArgs e)
        {
            txtScheduledDate.Value = calScheduledDate.SelectedDate.ToString("yyyy-MM-dd");
        }

        private void ShowWizardError(string message)
        {
            lblWizardError.Text = message;
            lblWizardError.Visible = true;
            lblWizardError.Style["display"] = "block";
        }
    }
}