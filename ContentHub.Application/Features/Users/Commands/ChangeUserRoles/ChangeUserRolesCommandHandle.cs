using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Exceptions;
using ContentHub.Domain.Identity;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ContentHub.Application.Features.Users.Commands.ChangeUserRoles;

// NOTE:
// This command performs multiple identity operations.
// It will be wrapped by TransactionBehavior in a future session
// to guarantee atomicity.
public sealed class ChangeUserRolesCommandHandle : ICommandHandler<ChangeUserRolesCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public ChangeUserRolesCommandHandle(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Handle(ChangeUserRolesCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null)
            throw new NotFoundException("User not found.");

        foreach (var role in request.Roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                throw new ValidationException(
                 [
                    new ValidationFailure(
                    nameof(request.Roles),
                    $"Role '{role}' does not exist.")
                ]);
            }
        }

        var currentRoles = await _userManager.GetRolesAsync(user);

        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

        if (!removeResult.Succeeded)
            throw new IdentityOperationException(removeResult.Errors);

        var addResult = await _userManager.AddToRolesAsync(user, request.Roles);

        if (!addResult.Succeeded)
            throw new IdentityOperationException(addResult.Errors);
    }
}