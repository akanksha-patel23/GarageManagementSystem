using GarageManagement.Api.DTOs;

namespace GarageManagement.Api.Services
{
    /// <summary>
    /// Interface for Vehicle Service
    /// </summary>
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleDto>> GetVehiclesByCustomerAsync(int customerId);
    }
}
