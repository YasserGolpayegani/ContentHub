using ContentHub.Application.Authentication;
using ContentHub.Application.Interfaces.Authentication;
using ContentHub.Domain.Identity;
using ContentHub.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContentHub.Infrastructure.Services;

public sealed class JwtService : IJwtService
{
    private readonly JwtOptions _jwtOptions;

    public JwtService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public JwtTokenResult GenerateToken(AppUser user, IEnumerable<string> roles)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        var now = DateTime.UtcNow;
        
        var expireAtUtc = now.AddMinutes(_jwtOptions.ExpirationInMinutes);
        
        var claims = CreateClaims(user, roles);
       
        var signingCredentials = CreateSigningCredentials();

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims = claims,
            notBefore: now,
            expires: expireAtUtc,
            signingCredentials: signingCredentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new JwtTokenResult(accessToken, expireAtUtc);
    }


    private List<Claim> CreateClaims(AppUser user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub,user.Id.ToString()),

            new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),

            new(JwtRegisteredClaimNames .UniqueName,user.UserName??string.Empty),

            new(JwtRegisteredClaimNames.Email,user.Email??string.Empty),

            new(JwtRegisteredClaimNames.NameId,user.Id.ToString()),

            new(ClaimTypes.Name,user.UserName??string.Empty)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return claims;
    }


    private SigningCredentials CreateSigningCredentials()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

        return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

}
