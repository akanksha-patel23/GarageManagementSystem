using GarageManagement.Api.DTOs;

namespace GarageManagement.Api.Repositories
{
    /// <summary>
    /// Interface for Dashboard Repository
    /// spGetDashboardStats via QueryMultiple
    /// </summary>
    public interface IDashboardRepository
    {
        Task<DashboardStatsDto> GetDashboardStatsAsync();
    }
}
