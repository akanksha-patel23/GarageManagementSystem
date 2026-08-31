using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using GarageManagement.Web.BAL;
using GarageManagement.Web.Entities;

namespace GarageManagement.Web.Pages.ServiceCategories
{
    public partial class ServiceCategoryList : System.Web.UI.Page
    {
        private readonly ServiceCategoryBAL _bal = new ServiceCategoryBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindTree();
            }
        }

        private void BindTree()
        {
            try
            {
                var categories = _bal.GetCategoryTree();
                tvCategories.Nodes.Clear();

                // The flat list is already ordered by Depth from spGetServiceCategoryTree,
                // but building the actual parent/child TreeNode structure (rather than
                // reusing the indentation trick the wizard's dropdown uses) is what makes
                // this a genuine TreeView rather than a flattened list wearing one.
                var roots = categories.Where(c => c.ParentCategoryID == null);
                foreach (var root in roots)
                {
                    tvCategories.Nodes.Add(BuildNode(root, categories));
                }

                if (tvCategories.Nodes.Count == 0)
                {
                    ShowMessage("No service categories yet — add one to get started.", isError: false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to load service categories. " + ex.Message, isError: true);
            }
        }

        private TreeNode BuildNode(ServiceCategory category, List<ServiceCategory> allCategories)
        {
            var node = new TreeNode(category.CategoryName)
            {
                NavigateUrl = "~/Pages/ServiceCategories/ServiceCategoryDetails.aspx?CategoryID=" + category.CategoryID,
                SelectAction = TreeNodeSelectAction.None
            };

            var children = allCategories.Where(c => c.ParentCategoryID == category.CategoryID);
            foreach (var child in children)
            {
                node.ChildNodes.Add(BuildNode(child, allCategories));
            }

            return node;
        }

        private void ShowMessage(string message, bool isError)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isError ? "msc-message msc-message-error" : "msc-message msc-message-success";
            lblMessage.Visible = true;
        }
    }
}