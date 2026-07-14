namespace ContentHub.Application.Features.Auth.Commands.Login;

public sealed class LoginResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public DateTime ExpireAtUtc { get; init; }
}


