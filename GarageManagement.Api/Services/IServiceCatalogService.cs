using GarageManagement.Api.DTOs;

namespace GarageManagement.Api.Services
{
    /// <summary>
    /// Interface for Service Catalog Service
    /// </summary>
    public interface IServiceCatalogService
    {
        Task<IEnumerable<ServiceCatalogDto>> GetServicesByCategoryAsync(int categoryId);
    }
}
