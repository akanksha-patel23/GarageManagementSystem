using GarageManagement.Api.DTOs;

namespace GarageManagement.Api.Repositories
{
    /// <summary>
    /// Interface for Service Catalog Repository
    /// spGetServicesByCategory
    /// </summary>
    public interface IServiceCatalogRepository
    {
        Task<IEnumerable<ServiceCatalogDto>> GetServicesByCategoryAsync(int categoryId);
    }
}
