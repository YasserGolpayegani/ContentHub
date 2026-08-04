using FluentValidation;

namespace ContentHub.Application.Features.Users.Queries.GetUserById;

public sealed class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User id is required.");
    }
}
