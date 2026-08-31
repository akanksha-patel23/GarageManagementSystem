using GarageManagement.Api.DTOs;
using GarageManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Api.Controllers
{
    /// <summary>
    /// Vehicles Controller - GET /api/vehicles/{customerId}
    /// </summary>
    [ApiController]
    [Route("api/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        // GET /api/vehicles/{customerId}
        // Consumed only by JobCardWizard.aspx's vehicle dropdown (Flow C).
        [HttpGet("{customerId:int}")]
        public async Task<ActionResult<System.Collections.Generic.IEnumerable<VehicleDto>>> GetByCustomer(int customerId)
        {
            var vehicles = await _vehicleService.GetVehiclesByCustomerAsync(customerId);
            return Ok(vehicles);
        }
    }
}