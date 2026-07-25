using ContentHub.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Domain.Entities;

public sealed class RefreshToken
{
    public RefreshToken()
    {
    }

    public RefreshToken(string token, Guid userId, DateTime expiresOnUtc)
    {
        Token = token;
        UserId = userId;
        ExpiresOnUtc = expiresOnUtc;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresOnUtc { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? RevokedOnUtc { get; private set; }
    public bool IsRevoked => RevokedOnUtc.HasValue;
    public string? ReplacedByToken { get; private set; }

    public Guid UserId { get; private set; }
    public AppUser User { get; private set; } = null!;

    public void Revoke()
    {
        if (!IsRevoked)
            RevokedOnUtc = DateTime.UtcNow;
    }

    public bool IsExpired() => DateTime.UtcNow >= ExpiresOnUtc;

    public bool IsActive() => !IsExpired() && !IsRevoked;

    public void SetReplacement(string token) => ReplacedByToken = token;
}
