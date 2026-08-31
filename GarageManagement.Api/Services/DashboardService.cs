using GarageManagement.Api.DTOs;
using GarageManagement.Api.Repositories;

namespace GarageManagement.Api.Services
{
    /// <summary>
    /// Implementation of Dashboard Service
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;

        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository;
        }

        public Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            return _repository.GetDashboardStatsAsync();
        }
    }
}
