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

            // Page.Title is not set here — page-specific titles go through the
            // TitleContent ContentPlaceHolder on each .aspx page.

            if (Session["FullName"] != null)
            {
                lblLoggedInAs.Text = $"{Session["FullName"]} ({Session["Role"]})";
            }

            hdnJwtToken.Value = Session["JwtToken"] as string ?? string.Empty;

        }
    }
}
