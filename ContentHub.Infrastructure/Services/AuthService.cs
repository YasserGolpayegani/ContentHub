using ContentHub.Application.Exceptions;
using ContentHub.Application.Features.Auth.Commands.Login;
using ContentHub.Application.Interfaces;
using ContentHub.Domain.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Infrastructure.Services;

public sealed class AuthService:IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IValidator<LoginCommand> _validator;

    public AuthService(UserManager<AppUser> userManager, IJwtService jwtService, IValidator<LoginCommand> validator)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _validator = validator;
    }
    

    public async Task<LoginResponse> LoginAsync(LoginCommand request, CancellationToken cancellationToken=default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new Application.Exceptions.ValidationException(string.Join(" | " ,validationResult.Errors));

        var user = await _userManager.FindByEmailAsync(request.Email);
        
        
        if (user == null)
            throw new UnauthorizedException("Email or password is incorrect.");

        if (!user.IsActive)
            throw new UnauthorizedException("Your account in inactive.");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid)
            throw new UnauthorizedException("Email or password is incorrect.");

        var roles = await _userManager.GetRolesAsync(user);

        var accessToken =  _jwtService.GenerateToken(user,roles);


        return new LoginResponse
        {
            AccessToken = accessToken.Token,
            ExpireAtUtc = accessToken.ExpiresAtUtc
        };
    }
}
