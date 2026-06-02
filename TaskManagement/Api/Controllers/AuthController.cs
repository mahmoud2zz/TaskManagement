using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.ServicesAuth;

namespace TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [Authorize]
        [HttpGet("me")]
      
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirst("uid")?.Value;

            if (userId == null)
                return Unauthorized();

            var result = await _authService.GetCurrentUserAsync(userId);

            if (!result.Status)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto model)
        {
            var result = await _authService.RegisterAsync(model);

            if (!result.Status)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginDto model)
        {
            var result = await _authService.LoginAsync(model);

            if (!result.Status)
                return Unauthorized(result);

            return Ok(result);
        }
    }
}