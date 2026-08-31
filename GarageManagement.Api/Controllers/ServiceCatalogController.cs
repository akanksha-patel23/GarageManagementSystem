using GarageManagement.Api.DTOs;
using GarageManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Api.Controllers
{
    /// <summary>
    /// ServiceCatalog Controller - GET /api/servicecatalog?categoryId=
    /// </summary>
    [ApiController]
    [Route("api/servicecatalog")]
    public class ServiceCatalogController : ControllerBase
    {
        private readonly IServiceCatalogService _serviceCatalogService;

        public ServiceCatalogController(IServiceCatalogService serviceCatalogService)
        {
            _serviceCatalogService = serviceCatalogService;
        }

        // GET /api/servicecatalog?categoryId=X
        // Consumed only by JobCardWizard.aspx's cascading Services CheckBoxList (Flow D).
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceCatalogDto>>> GetByCategory([FromQuery] int categoryId)
        {
            var services = await _serviceCatalogService.GetServicesByCategoryAsync(categoryId);
            return Ok(services);
        }
    }
}
