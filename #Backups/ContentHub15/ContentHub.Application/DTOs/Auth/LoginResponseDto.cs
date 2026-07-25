namespace ContentHub.Application.DTOs.Auth;

public sealed class LoginResponseDto
{
    public string AccessToken { get; init; } = string.Empty;
    public DateTime ExpireAtUtc { get; init; }
}


