using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GarageManagement.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // IIS resolves this as the site's default document (<defaultDocument> entry in Web.config).
            // The application's real home is the Dashboard per Web.sitemap, so redirect straight there.
            Response.Redirect("/Pages/Dashboard/Dashboard.aspx", endResponse: true);
        }
    }
}