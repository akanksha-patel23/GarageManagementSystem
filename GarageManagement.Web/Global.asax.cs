using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace GarageManagement.Web
{
    /// <summary>
    /// Global Application Class
    /// Handles application-level events and settings
    /// </summary>
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Route/bundle registration goes here if introduced later.
        }

        // FormsAuthentication alone only authenticates the request — it does
        // NOT automatically populate User.IsInRole() from the ticket's
        // UserData. Without this, EVERY User.IsInRole("Admin") check (Web.config's
        // <allow roles="Admin"/> rules AND Web.sitemap's securityTrimmingEnabled)
        // silently returns false for everyone, including actual admins.
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated && Context.User?.Identity is FormsIdentity identity)
            {
                string[] roles = identity.Ticket.UserData.Split(',');
                Context.User = new GenericPrincipal(identity, roles);
            }
        }

        void Application_End(object sender, EventArgs e)
        {
            // Code that runs on application shutdown
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Exception ex = Server.GetLastError();
            if (ex == null) return;

            // No client-side logging sink is defined for exceptions above the
            // BAL/DAL boundary (e.g. unhandled exceptions in code-behind) —
            // GLOG.spLogDBErrorDetail only covers database-originated failures.
            // Write to the Windows Event Log so these are never silently lost.

            string currentPath = Request.AppRelativeCurrentExecutionFilePath ?? "";
            if (currentPath.IndexOf("Error.aspx", StringComparison.OrdinalIgnoreCase) >= 0)
                return; // let ASP.NET show the raw error for Error.aspx itself


            try
            {
                //EventLog.WriteEntry("GarageManagementWeb", ex.ToString(), EventLogEntryType.Error);
                System.Diagnostics.EventLog.WriteEntry(
            "GarageManagementWeb", ex.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }
            catch
            {
                // Never let logging itself crash the error handler.
            }

            //Server.ClearError();
            //Response.Redirect("~/Pages/Shared/Error.aspx");
        }
    }
}