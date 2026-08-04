using FluentValidation;

namespace ContentHub.Application.Features.Users.Commands.ChangeUserRoles;

public sealed class ChangeUserRolesCommandValidator : AbstractValidator<ChangeUserRolesCommand>
{
	public ChangeUserRolesCommandValidator()
	{
		RuleFor(x=>x.UserId)
			.NotEmpty();

		RuleFor(x => x.Roles)
			.NotEmpty()
			.WithMessage("At least one role must be specified.");

		RuleForEach(x => x.Roles)
			.NotEmpty()
			.MaximumLength(256);
	}
}