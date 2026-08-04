using FluentValidation;

namespace ContentHub.Application.Features.Users.Commands.UpdateUserStatusCommand;

public sealed class UpdateUserStatusCommandValidator : AbstractValidator<UpdateUserStatusCommand>
{
    public UpdateUserStatusCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User id is required.");
    }
}
