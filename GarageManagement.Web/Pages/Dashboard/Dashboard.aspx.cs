using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Net.Http;

namespace GarageManagement.Web.Pages.Dashboard
{
    public partial class Dashboard : System.Web.UI.Page
    {
        // Shared across requests to this page — a new HttpClient per request risks
        // socket exhaustion under load. If more pages end up calling the API
        // server-side, promote this into a shared static helper instead of
        // duplicating it per page.
        private static readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiBaseUrl"])
        };

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await LoadDashboardStatsAsync();
            }
        }

        private async Task LoadDashboardStatsAsync()
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("/api/dashboard/stats");
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                // Newtonsoft.Json matches JSON property names case-insensitively by
                // default, so the API's camelCase response (monthlyRevenue,
                // statusDistribution) binds correctly to these PascalCase classes
                // with no extra configuration.
                var stats = JsonConvert.DeserializeObject<DashboardStatsResponse>(json);

                BindRevenueChart(stats.MonthlyRevenue);
                BindStatusChart(stats.StatusDistribution);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Unable to load dashboard data. " + ex.Message;
                lblMessage.CssClass = "msc-message msc-message-error";
                lblMessage.Visible = true;
            }
        }

        private void BindRevenueChart(List<MonthlyRevenuePoint> monthlyRevenue)
        {
            chartRevenue.Series["Revenue"].Points.Clear();
            foreach (var point in monthlyRevenue)
            {
                string label = $"{point.RevenueMonth:00}/{point.RevenueYear}";
                chartRevenue.Series["Revenue"].Points.AddXY(label, point.Revenue);
            }
        }

        private void BindStatusChart(List<StatusDistributionPoint> statusDistribution)
        {
            chartStatus.Series["Status"].Points.Clear();
            foreach (var point in statusDistribution)
            {
                chartStatus.Series["Status"].Points.AddXY(point.Status, point.JobCount);
            }
        }

        // Local response-shape classes — only exist to deserialize the API's
        // JSON, so they stay private to this page rather than living in Entities/.
        private class DashboardStatsResponse
        {
            public List<MonthlyRevenuePoint> MonthlyRevenue { get; set; }
            public List<StatusDistributionPoint> StatusDistribution { get; set; }
        }

        private class MonthlyRevenuePoint
        {
            public int RevenueYear { get; set; }
            public int RevenueMonth { get; set; }
            public decimal Revenue { get; set; }
        }

        private class StatusDistributionPoint
        {
            public string Status { get; set; }
            public int JobCount { get; set; }
        }
    }
}
