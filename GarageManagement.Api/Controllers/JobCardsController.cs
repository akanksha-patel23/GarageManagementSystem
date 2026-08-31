using GarageManagement.Api.DTOs;
using GarageManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Api.Controllers
{
    /// <summary>
    /// JobCards Controller - POST /api/jobcards, GET /api/jobcards/{id}/status-history
    /// </summary>
    [ApiController]
    [Route("api/jobcards")]
    public class JobCardsController : ControllerBase
    {
        private readonly IJobCardService _jobCardService;

        public JobCardsController(IJobCardService jobCardService)
        {
            _jobCardService = jobCardService;
        }

        // POST /api/jobcards — called from JobCardWizard.aspx's final step (Flow B).
        // Unhandled exceptions here surface as ProblemDetails via Program.cs's
        // UseExceptionHandler() + AddProblemDetails() — no manual try/catch needed.
        [HttpPost]
        public async Task<ActionResult<JobCardResultDto>> Create([FromBody] JobCardCreateDto dto)
        {
            var result = await _jobCardService.CreateJobCardAsync(dto);
            return CreatedAtAction(nameof(GetStatusHistory), new { jobCardId = result.JobCardID }, result);
        }

        // GET /api/jobcards/{id}/status-history — for external/non-WebForms consumers.
        // The live status board itself uses Web Forms Flow E (UpdatePanel + Timer),
        // not this endpoint — see 10-canonical-flows.md.
        [HttpGet("{jobCardId:int}/status-history")]
        public async Task<ActionResult<IEnumerable<JobCardStatusHistoryDto>>> GetStatusHistory(int jobCardId)
        {
            var history = await _jobCardService.GetStatusHistoryAsync(jobCardId);
            return Ok(history);
        }
    }
}