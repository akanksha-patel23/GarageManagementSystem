using GarageManagement.Api.DTOs;
using GarageManagement.Api.Repositories;

namespace GarageManagement.Api.Services
{
    /// <summary>
    /// Implementation of Vehicle Service
    /// </summary>
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _repository;

        public VehicleService(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<VehicleDto>> GetVehiclesByCustomerAsync(int customerId)
        {
            return _repository.GetVehiclesByCustomerAsync(customerId);
        }
    }
}
