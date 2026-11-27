using GarageManagement.Web.BAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace GarageManagement.Web
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly UserBAL _userBal = new UserBAL();

        // Static HttpClient is intentional — one instance per AppDomain avoids
        // socket exhaustion. BaseAddress is set once at initialisation time from
        // the same appSetting that every other server-side caller uses.
        private static readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiBaseUrl"]),
            // Give the API call a hard ceiling so a slow/unreachable API never
            // stalls the login page for the full IIS request-processing timeout.
            Timeout = TimeSpan.FromSeconds(10)
        };

        // -----------------------------------------------------------------------
        // NOT async void — that was the root cause of Session["JwtToken"] always
        // being null.
        //
        // async void event handlers in WebForms yield the thread at the first
        // await, the ASP.NET pipeline continues without waiting for the
        // continuation, and Response.Redirect (or the pipeline itself ending the
        // request) aborts the thread before the continuation can resume. The
        // runtime then throws ThreadAbortException inside
        // InvokeCancellableCallback, which catch(Exception) silently swallows and
        // sets the token to null on every single login.
        //
        // Fix: make the handler synchronous (void, no await). The HTTP call to
        // the API runs on a ThreadPool thread via Task.Run — that thread carries
        // no ASP.NET SynchronizationContext, so ConfigureAwait(false) inside it
        // is safe and there is no deadlock risk. GetAwaiter().GetResult() blocks
        // the current request thread (which is fine — it is already blocked
        // waiting for the page to render anyway) until the HTTP call completes.
        // -----------------------------------------------------------------------
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var user = _userBal.VerifyLogin(txtUsername.Text.Trim(), txtPassword.Text);
            if (user == null)
            {
                ShowMessage("Invalid username or password.");
                return;
            }

            // FormsAuthentication ticket — Role in UserData for
            // Application_AuthenticateRequest in Global.asax to rebuild
            // GenericPrincipal so User.IsInRole() and Web.config's
            // <allow roles="Admin"/> rules both work.
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: user.Username,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddHours(8),
                isPersistent: false,
                userData: user.Role);

            Response.Cookies.Add(new HttpCookie(
                FormsAuthentication.FormsCookieName,
                FormsAuthentication.Encrypt(ticket)));

            Session["FullName"] = user.FullName;
            Session["Role"] = user.Role;

            // Capture form values now — TextBox controls may not be accessible
            // after the response pipeline moves on.
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            // Obtain a JWT so browser-side AJAX in jobcard-wizard.js can reach
            // the [Authorize]-protected API endpoints. Runs synchronously on a
            // ThreadPool thread to guarantee Session["JwtToken"] is written
            // before Response.Redirect fires — the async void approach was
            // causing ThreadAbortException to abort the continuation every time.
            try
            {
                string token = Task.Run(async () =>
                {
                    var body = new StringContent(
                        JsonConvert.SerializeObject(new { username, password }),
                        Encoding.UTF8, "application/json");

                    // ConfigureAwait(false): the continuation inside Task.Run
                    // must NOT try to resume on the ASP.NET sync context — there
                    // is none on a ThreadPool thread, and attempting to capture it
                    // would deadlock.
                    HttpResponseMessage resp = await _httpClient
                        .PostAsync("/api/auth/login", body)
                        .ConfigureAwait(false);

                    if (!resp.IsSuccessStatusCode) return null;

                    string json = await resp.Content
                        .ReadAsStringAsync()
                        .ConfigureAwait(false);

                    return JObject.Parse(json)["token"]?.ToString();

                }).GetAwaiter().GetResult();   // blocks until the Task.Run lambda completes

                Session["JwtToken"] = token;
            }
            catch (Exception ex)
            {
                // Token acquisition failed — page navigation still works via
                // FormsAuth; only browser-side AJAX (the wizard) would be
                // degraded. Log so the cause is visible without crashing login.
                System.Diagnostics.Trace.TraceWarning(
                    "GarageManagement: POST /api/auth/login failed during login. " + ex.Message);
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