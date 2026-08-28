using ContentHub.Api.Authorization;
using ContentHub.Api.Models.Users;
using ContentHub.Application.Authorization;
using ContentHub.Application.Features.Users.Commands.ChangeUserRoles;
using ContentHub.Application.Features.Users.Commands.UpdateUserStatusCommand;
using ContentHub.Application.Features.Users.Queries.GetUserById;
using ContentHub.Application.Features.Users.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ContentHub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }


    [HttpGet]
    [HasPermission(PermissionNames.Users.Read)]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var users = await _sender.Send(new GetUsersQuery(), cancellationToken);

        return Ok(users);
    }


    [HttpGet("{id:guid}")]
    [HasPermission(PermissionNames.Users.Read)]
    [EnableRateLimiting("user-api")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _sender.Send(new GetUserByIdQuery(id), cancellationToken);

        return Ok(user);
    }

    [HttpPut("{id:guid}/status")]
    [HasPermission(PermissionNames.Users.Activate)]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        UpdateUserStatusRequest request,
        CancellationToken cancellationToken)
    {
        await _sender.Send(new UpdateUserStatusCommand(id, request.IsActive), cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:guid}/roles")]
    [HasPermission(PermissionNames.Users.ChangeRole)]
    public async Task<IActionResult> ChangeRoles(
        Guid id,
        ChangeUserRolesRequest request,
        CancellationToken cancellationToken)
    {
        await _sender.Send(new ChangeUserRolesCommand(id, request.Roles), cancellationToken);

        return NoContent();
    }
}
