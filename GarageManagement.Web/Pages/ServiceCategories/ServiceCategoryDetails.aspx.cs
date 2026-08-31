using GarageManagement.Web.BAL;
using GarageManagement.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GarageManagement.Web.Pages.ServiceCategories
{
    public partial class ServiceCategoryDetails : System.Web.UI.Page
    {
        private readonly ServiceCategoryBAL _bal = new ServiceCategoryBAL();

        private int? CategoryId
        {
            get
            {
                if (int.TryParse(Request.QueryString["CategoryID"], out int id))
                    return id;
                return null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindParentDropDown();

                if (CategoryId.HasValue)
                {
                    var category = _bal.GetCategoryTree().Find(c => c.CategoryID == CategoryId.Value);
                    if (category == null)
                    {
                        ShowMessage("Category not found.", isError: true);
                        return;
                    }

                    txtCategoryName.Text = category.CategoryName;
                    if (category.ParentCategoryID.HasValue)
                        ddlParentCategory.SelectedValue = category.ParentCategoryID.Value.ToString();

                    btnDelete.Visible = true;
                }
            }
        }

        private void BindParentDropDown()
        {
            try
            {
                var categories = _bal.GetCategoryTree();

                // Prevent a category from being set as its own parent (or a parent
                // of one of its own descendants) — exclude self and descendants
                // when editing.
                if (CategoryId.HasValue)
                {
                    var excluded = GetDescendantIds(CategoryId.Value, categories);
                    excluded.Add(CategoryId.Value);
                    categories = categories.Where(c => !excluded.Contains(c.CategoryID)).ToList();
                }

                foreach (var category in categories)
                {
                    string prefix = category.Depth > 0 ? new string('-', category.Depth * 2) + " " : string.Empty;
                    ddlParentCategory.Items.Add(new System.Web.UI.WebControls.ListItem(
                        prefix + category.CategoryName, category.CategoryID.ToString()));
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to load parent categories. " + ex.Message, isError: true);
            }
        }

        private System.Collections.Generic.List<int> GetDescendantIds(int categoryId, System.Collections.Generic.List<ServiceCategory> all)
        {
            var descendants = new System.Collections.Generic.List<int>();
            var children = all.Where(c => c.ParentCategoryID == categoryId);
            foreach (var child in children)
            {
                descendants.Add(child.CategoryID);
                descendants.AddRange(GetDescendantIds(child.CategoryID, all));
            }
            return descendants;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                var category = new ServiceCategory
                {
                    CategoryID = CategoryId ?? 0,
                    CategoryName = txtCategoryName.Text.Trim(),
                    ParentCategoryID = string.IsNullOrEmpty(ddlParentCategory.SelectedValue)
                        ? (int?)null
                        : Convert.ToInt32(ddlParentCategory.SelectedValue)
                };

                if (CategoryId.HasValue)
                    _bal.UpdateServiceCategory(category);
                else
                    _bal.InsertServiceCategory(category);

                Response.Redirect("~/Pages/ServiceCategories/ServiceCategoryList.aspx");
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to save category. " + ex.Message, isError: true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!CategoryId.HasValue) return;

            try
            {
                _bal.DeleteServiceCategory(CategoryId.Value);
                Response.Redirect("~/Pages/ServiceCategories/ServiceCategoryList.aspx");
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to delete category — it may still have services or sub-categories under it. " + ex.Message, isError: true);
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