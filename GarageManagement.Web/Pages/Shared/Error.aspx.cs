using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GarageManagement.Web.Pages.Shared
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // No logic needed — Global.asax.cs already logs the exception to the
            // Windows Event Log before redirecting here.
        }
    }
}