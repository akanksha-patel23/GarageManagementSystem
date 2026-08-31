using System.Collections.Generic;

namespace GarageManagement.Api.DTOs
{
    public class DashboardStatsDto
    {
        public List<MonthlyRevenueDto> MonthlyRevenue { get; set; } = new List<MonthlyRevenueDto>();
        public List<StatusDistributionDto> StatusDistribution { get; set; } = new List<StatusDistributionDto>();
    }

    public class MonthlyRevenueDto
    {
        public int RevenueYear { get; set; }
        public int RevenueMonth { get; set; }
        public decimal Revenue { get; set; }
    }

    public class StatusDistributionDto
    {
        public string Status { get; set; }
        public int JobCount { get; set; }
    }
}