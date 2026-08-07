using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Exceptions;
using ContentHub.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ContentHub.Application.Features.Users.Commands.UpdateUserStatusCommand;

public sealed class UpdateUserStatusCommandHandler : ICommandHandler<UpdateUserStatusCommand>
{
    private readonly UserManager<AppUser> _userManager;

    public UpdateUserStatusCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null)
            throw new NotFoundException("User not found.");

        if (request.IsActive)
            user.Activate();

        else
            user.Deactivate();
                    
        var result =await _userManager.UpdateAsync(user);

        if (!result.Succeeded) 
            throw new IdentityOperationException(result.Errors);
    }
}