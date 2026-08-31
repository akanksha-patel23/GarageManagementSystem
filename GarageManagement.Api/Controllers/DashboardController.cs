using GarageManagement.Api.DTOs;
using GarageManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Api.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // GET /api/dashboard/stats
        // Consumed server-side by Dashboard.aspx.cs via HttpClient (Flow F) —
        // not by client-side JS, since the native Chart control can't be
        // updated from the browser. See 10-canonical-flows.md.
        [HttpGet("stats")]
        public async Task<ActionResult<DashboardStatsDto>> GetStats()
        {
            var stats = await _dashboardService.GetDashboardStatsAsync();
            return Ok(stats);
        }
    }
}
