using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Features.Posts.Commands;

public sealed class CreatePostCommandValidator:AbstractValidator<CreatePostCommand>
{
	public CreatePostCommandValidator()
	{
		RuleFor(x => x.Title)
			.NotEmpty()
			.MaximumLength(200);

		RuleFor(x => x.Content)
			.NotEmpty();

		RuleFor(x => x.Slug)
			.NotEmpty();

		RuleFor(x => x.CategoryId)
			.NotEmpty();
	}
}
