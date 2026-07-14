using ContentHub.Application.Features.Auth.Commands.Login;
using ContentHub.Application.Features.Auth.Commands.Register;
using MediatR;
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
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginCommand>> Login(LoginCommand command, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(command, cancellationToken);

            return Ok(response);
        }


        [HttpPost("register")]
        public async Task<ActionResult<Guid>> Register(RegisterCommand request, CancellationToken cancellationToken)
        {
            var userId = await _sender.Send(request, cancellationToken);

            return CreatedAtAction(nameof(Register), new { id = userId }, userId);
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
