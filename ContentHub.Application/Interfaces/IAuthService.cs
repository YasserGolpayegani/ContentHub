using ContentHub.Application.Features.Auth.Commands.Login;

namespace ContentHub.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginCommand request, CancellationToken cancellationToken=default);
}


