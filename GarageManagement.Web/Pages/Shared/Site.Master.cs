using System;

namespace GarageManagement.Web.Pages.Shared
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // No shared logic needed yet. Page-specific title overrides go through
            // the TitleContent ContentPlaceHolder on each .aspx page — do not set
            // Page.Title here, or it will override every page's own title.
        }
    }
}
