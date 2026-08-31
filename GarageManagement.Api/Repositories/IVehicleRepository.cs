using GarageManagement.Api.DTOs;

namespace GarageManagement.Api.Repositories
{
    /// <summary>
    /// Interface for Vehicle Repository
    /// spGetVehiclesByCustomer
    /// </summary>
    public interface IVehicleRepository
    {
        Task<IEnumerable<VehicleDto>> GetVehiclesByCustomerAsync(int customerId);
    }
}
