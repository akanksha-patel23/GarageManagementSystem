using GarageManagement.Api.DTOs;
using GarageManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST /api/auth/login — the only anonymous endpoint in the API.
        // Called from Login.aspx.cs server-side (to obtain a JWT for
        // jobcard-wizard.js's browser-side AJAX calls) after Web Forms has
        // already independently verified the same credentials for its own
        // FormsAuthentication ticket.
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (result == null)
                return Unauthorized(new { detail = "Invalid username or password." });

            return Ok(result);
        }
    }
}
