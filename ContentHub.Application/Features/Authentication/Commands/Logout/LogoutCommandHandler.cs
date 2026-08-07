using ContentHub.Application.Abstractions.Authentication;
using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Abstractions.Persistence;
using ContentHub.Application.Exceptions;
using MediatR;

namespace ContentHub.Application.Features.Authentication.Commands.Logout;

public sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IRefreshTokenService _refreshTokenService;

    public LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository, IRefreshTokenService refreshTokenService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _refreshTokenService = refreshTokenService;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(
            request.RefreshToken,
            cancellationToken);

        if (refreshToken is null)
            throw new UnauthorizedException("Invalid refresh token.");

        if (!_refreshTokenService.IsValid(refreshToken))
            throw new UnauthorizedException("Refresh token has expired or has been revoked.");

        refreshToken.Revoke();

        _refreshTokenRepository.Update(refreshToken);        
    }
}
