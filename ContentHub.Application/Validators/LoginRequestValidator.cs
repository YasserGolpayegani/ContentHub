using ContentHub.Application.DTOs.Auth;
using FluentValidation;

namespace ContentHub.Application.Validators;

public sealed class LoginRequestValidator: AbstractValidator<LoginRequestDto>
{
	public LoginRequestValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty()
			.WithMessage("Email is required.")			
			.EmailAddress()
			.WithMessage("Invalid email address");


		RuleFor(x => x.Password)
			.NotEmpty()
			.WithMessage("Password is required")
			.MinimumLength(3)
			.WithMessage("Password must be al least 6 characters.");			
	}		
}


