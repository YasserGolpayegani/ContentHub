using ContentHub.Domain.Abstractions;
using ContentHub.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Domain.Identity;

public class AppUser : IdentityUser<Guid>, IAuditableEntity
{
    private readonly List<RefreshToken> _refreshTokens = new();

    private AppUser() { }

    public AppUser(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;

        Email = email;
        UserName = email;
    }

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public bool IsActive { get; private set; } = true;

    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();


    /// <summary>
    /// Activates the user account.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    /// Deactivates the user account.
    /// </summary>
    public void Activate() => IsActive = true;
}
