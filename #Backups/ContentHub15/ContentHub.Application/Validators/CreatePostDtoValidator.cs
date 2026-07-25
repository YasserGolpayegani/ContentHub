using ContentHub.Application.DTOs.Posts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Validators;

public class CreatePostDtoValidator:AbstractValidator<CreatePostDto>
{
	public CreatePostDtoValidator()
	{
		//RuleFor(x => x.Title)
		//	.NotEmpty()
		//	.MaximumLength(200);

		//RuleFor(x => x.Content)
		//	.NotEmpty();

		//RuleFor(x => x.Slug)
		//	.NotEmpty()
		//	.MaximumLength(200);

		//RuleFor(x => x.CategoryId)
		//	.NotEmpty();
	}
}


