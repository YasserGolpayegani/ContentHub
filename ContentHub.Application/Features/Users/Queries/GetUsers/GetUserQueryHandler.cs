using ContentHub.Application.DTOs.Users;
using ContentHub.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ContentHub.Application.Features.Users.Queries.GetUsers;

public sealed class GetUserQueryHandler : IRequestHandler<GetUsersQuery, IReadOnlyCollection<UserDto>>
{
    private readonly UserManager<AppUser> _userManager;

    public GetUserQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IReadOnlyCollection<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ToListAsync();

        var result = new List<UserDto>(users.Count);

        foreach (var user in users)
        {
            var roles=await _userManager.GetRolesAsync(user);

            result.Add(new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                IsActive = user.IsActive,
                Roles = roles
            });
        }

        return result;
    }
}

