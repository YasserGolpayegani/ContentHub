using ContentHub.Application.DTOs.Auth;
using ContentHub.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContentHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        public async Task<ActionResult<LoginRequestDto>> Login(LoginRequestDto request, CancellationToken cancellationToken)
        {
            var response = await _authService.LoginAsync(request, cancellationToken);

            return Ok(response);
        }

        //forTest
        [Authorize]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            var header = Request.Headers.Authorization.ToString();
            return Ok(header);
            //return Ok(new { Message = "You are authenticated." });
        }

    }
}
