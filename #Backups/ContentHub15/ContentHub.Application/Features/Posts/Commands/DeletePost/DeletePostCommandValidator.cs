using FluentValidation;

namespace ContentHub.Application.Features.Posts.Commands.DeletePost;

public sealed class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
{
    public DeletePostCommandValidator()
    {
        RuleFor(x => x.id)
            .NotEmpty();
    }
}
