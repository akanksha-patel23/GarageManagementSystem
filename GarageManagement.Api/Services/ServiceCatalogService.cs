using GarageManagement.Api.DTOs;
using GarageManagement.Api.Repositories;

namespace GarageManagement.Api.Services
{
    /// <summary>
    /// Implementation of Service Catalog Service
    /// </summary>
    public class ServiceCatalogService : IServiceCatalogService
    {
        private readonly IServiceCatalogRepository _repository;

        public ServiceCatalogService(IServiceCatalogRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<ServiceCatalogDto>> GetServicesByCategoryAsync(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentException("Invalid CategoryID.", nameof(categoryId));

            return _repository.GetServicesByCategoryAsync(categoryId);
        }
    }
}
