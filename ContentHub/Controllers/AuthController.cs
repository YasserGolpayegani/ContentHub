using ContentHub.Application.Features.Authentication.Commands.Login;
using ContentHub.Application.Features.Authentication.Commands.Logout;
using ContentHub.Application.Features.Authentication.Commands.Refresh;
using ContentHub.Application.Features.Authentication.Commands.Register;
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

        /// <summary>
        /// Authenticates a user and returns a JWT access token.
        /// </summary>
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


        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Refresh(
            [FromBody]RefreshCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);
            return Ok(result);
        }


        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout(
            [FromBody]LogoutCommand command,
            CancellationToken cancellationToken)
        {
            await _sender.Send(command,cancellationToken);

            return NoContent();
        }


        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            var header = Request.Headers.Authorization.ToString();
            return Ok(header);
            //return Ok(new { Message = "You are authenticated." });
        }

    }
}
