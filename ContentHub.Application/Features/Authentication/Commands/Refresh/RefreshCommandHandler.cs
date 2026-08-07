using ContentHub.Application.Abstractions.Authentication;
using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Abstractions.Persistence;
using ContentHub.Application.Exceptions;
using ContentHub.Application.Features.Authentication.Commands.Login;
using ContentHub.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ContentHub.Application.Features.Authentication.Commands.Refresh;

public sealed class RefreshCommandHandler : ICommandHandler<RefreshCommand, LoginResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IJwtService _jwtService;
    private readonly UserManager<AppUser> _userManager;

    public RefreshCommandHandler(IRefreshTokenRepository refreshTokenRepository, IRefreshTokenService refreshTokenService, IJwtService jwtService, UserManager<AppUser> userManager)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _refreshTokenService = refreshTokenService;
        _jwtService = jwtService;
        _userManager = userManager;
    }


    public async Task<LoginResponse> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

        if (refreshToken is null)
            throw new UnauthorizedException("Invalid refresh token.");

        if (!_refreshTokenService.IsValid(refreshToken))
            throw new UnauthorizedException("Refresh token has expired or has been revoked.");

        var user = refreshToken.User;

        if (!user.IsActive)
            throw new UnauthorizedException("Your account is inactive.");

        var roles = await _userManager.GetRolesAsync(user);

        var jwtToken = _jwtService.GenerateToken(user, roles);

        var newRefreshToken = _refreshTokenService.Rotate(refreshToken);

        await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);

        _refreshTokenRepository.Update(refreshToken);        

        return new LoginResponse(
            jwtToken.Token,
            jwtToken.ExpiresAtUtc,
            newRefreshToken.Token,
            newRefreshToken.ExpiresOnUtc);
    }
}