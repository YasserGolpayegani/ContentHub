using ContentHub.Application.Interfaces.Authentication;
using ContentHub.Domain.Entities;
using ContentHub.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace ContentHub.Infrastructure.Services;

public sealed class RefreshTokenService:IRefreshTokenService
{
    public RefreshToken Generate(AppUser user)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        return new RefreshToken(token, user.Id, DateTime.UtcNow.AddDays(7));
    }

    public bool IsValid(RefreshToken refreshToken)
    {
        return refreshToken.IsActive();
    }    

    public RefreshToken Rotate(RefreshToken refreshToken)
    {
        refreshToken.Revoke();

        var newRefreshToken = Generate(refreshToken.User);

        refreshToken.SetReplacement(newRefreshToken.Token);

        return newRefreshToken;      
    }
}
