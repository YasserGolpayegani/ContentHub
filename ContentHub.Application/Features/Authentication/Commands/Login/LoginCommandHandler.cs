using ContentHub.Application.Abstractions.Authentication;
using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Abstractions.Persistence;
using ContentHub.Application.Exceptions;
using ContentHub.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ContentHub.Application.Features.Authentication.Commands.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        UserManager<AppUser> userManager,
        IJwtService jwtService,
        IRefreshTokenService refreshTokenService,
        IRefreshTokenRepository refreshTokenRepository,
        ILogger<LoginCommandHandler> logger)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _refreshTokenService = refreshTokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _logger = logger;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            //_logger.LogWarning("Login attempt failed because the user was not found.");
            throw new UnauthorizedException("Invalid email or password.");
        }


        if (!user.IsActive)
        {
            //_logger.LogWarning("Login attempt failed because the user was not active.");
            throw new UnauthorizedException("Your account is inactive.");
        }

        var passwordIsValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!passwordIsValid)
        {
            //_logger.LogWarning("Login attempt failed because the provide credentials were invalid.");
            throw new UnauthorizedException("Invalid email or password.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var jwtToken = _jwtService.GenerateToken(user, roles);

        var refreshToken = _refreshTokenService.Generate(user);

        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        _logger.LogInformation("User logged in successfully.");

        return new LoginResponse(
            jwtToken.Token,
            jwtToken.ExpiresAtUtc,
            refreshToken.Token,
            refreshToken.ExpiresOnUtc);
    }
}