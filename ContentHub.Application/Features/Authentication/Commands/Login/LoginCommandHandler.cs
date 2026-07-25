using ContentHub.Application.Exceptions;
using ContentHub.Application.Interfaces.Authentication;
using ContentHub.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ContentHub.Application.Features.Authentication.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LoginCommandHandler(
        UserManager<AppUser> userManager,
        IJwtService jwtService,
        IRefreshTokenService refreshTokenService,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _refreshTokenService = refreshTokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            throw new UnauthorizedException("Invalid email or password.");

        if (!user.IsActive)
            throw new UnauthorizedException("Your account is inactive.");

        var passwordIsValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!passwordIsValid)
            throw new UnauthorizedException("Invalid email or password.");

        var roles = await _userManager.GetRolesAsync(user);

        var jwtToken = _jwtService.GenerateToken(user, roles);

        var refreshToken = _refreshTokenService.Generate(user);

        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);


        return new LoginResponse(
            jwtToken.Token,
            jwtToken.ExpiresAtUtc,
            refreshToken.Token,
            refreshToken.ExpiresOnUtc);
    }
}