using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using GarageManagement.Web.BAL;

namespace GarageManagement.Web
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly UserBAL _userBal = new UserBAL();

        private static readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiBaseUrl"])
        };

        protected async void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var user = _userBal.VerifyLogin(txtUsername.Text.Trim(), txtPassword.Text);
            if (user == null)
            {
                ShowMessage("Invalid username or password.");
                return;
            }

            // FormsAuthentication ticket drives Web.config's page-level
            // <authorization> rules. Role travels in UserData so BasePage-style
            // checks (and Web.sitemap's securityTrimmingEnabled) can read it
            // without a second database round trip on every page.
            var ticket = new FormsAuthenticationTicket(
                1, user.Username, DateTime.Now, DateTime.Now.AddHours(8), false, user.Role);
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(authCookie);

            Session["FullName"] = user.FullName;
            Session["Role"] = user.Role;

            // Also obtain a JWT from the API so jobcard-wizard.js (and any other
            // browser-side AJAX) can call the now-[Authorize]-protected API
            // endpoints. This is a second, independent verification against the
            // same tblUsers row — kept separate rather than passing a shared
            // secret between layers, consistent with how every other table in
            // this project is read independently by both the Web Forms DAL and
            // the API's own repositories.
            try
            {
                var payload = new { username = txtUsername.Text.Trim(), password = txtPassword.Text };
                var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("/api/auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var result = JObject.Parse(json);
                    Session["JwtToken"] = result["token"]?.ToString();
                }
                else
                {
                    // Page navigation still works via FormsAuth even if this fails —
                    // only AJAX-driven features (the wizard) would be degraded.
                    Session["JwtToken"] = null;
                }
            }
            catch
            {
                Session["JwtToken"] = null;
            }

            Response.Redirect("~/Pages/Dashboard/Dashboard.aspx");
        }

        private void ShowMessage(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = true;
        }
    }
}