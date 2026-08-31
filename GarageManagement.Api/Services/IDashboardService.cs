using GarageManagement.Api.DTOs;

namespace GarageManagement.Api.Services
{
    /// <summary>
    /// Interface for Dashboard Service
    /// </summary>
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetDashboardStatsAsync();
    }
}
