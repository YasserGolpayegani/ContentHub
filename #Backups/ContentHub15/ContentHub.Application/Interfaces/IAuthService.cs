using ContentHub.Application.DTOs.Auth;

namespace ContentHub.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken=default);
}


