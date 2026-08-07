using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.DTOs.Users;
using ContentHub.Application.Exceptions;
using ContentHub.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ContentHub.Application.Features.Users.Queries.GetUserById;

public sealed class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly UserManager<AppUser> _userManager;

    public GetUserByIdHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user is null)
            throw new NotFoundException("User not found.");

        var roles =await _userManager.GetRolesAsync(user);

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? string.Empty,
            IsActive = user.IsActive,
            Roles = roles
        };
    }
}