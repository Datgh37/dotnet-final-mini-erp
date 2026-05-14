using Microsoft.AspNetCore.Mvc;
using MiniERP_API.Models.DTOs;
using MiniERP_API.Services.Interfaces;

namespace MiniERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            try { return Ok(_authService.Login(request)); }
            catch (System.Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken(RefreshTokenRequest request)
        {
            return Ok(_authService.RefreshToken(request));
        }
    }
}
