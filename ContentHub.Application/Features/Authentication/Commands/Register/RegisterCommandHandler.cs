using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Authorization;
using ContentHub.Application.Exceptions;
using ContentHub.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ContentHub.Application.Features.Authentication.Commands.Register;

public sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, Guid>
{
    private readonly UserManager<AppUser> _userManager;

    public RegisterCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var exist = await _userManager.FindByEmailAsync(request.Email);

        if (exist is not null)
            throw new ValidationException("Email already exists.");

        var user = new AppUser(request.FirstName, request.LastName, request.Email);

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new ValidationException(string.Join(Environment.NewLine,
                    result.Errors.Select(x => x.Description)));

        await _userManager.AddToRoleAsync(user, Roles.User);

        return user.Id;
    }
}