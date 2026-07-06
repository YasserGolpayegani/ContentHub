using ContentHub.Application.Interfaces;
using ContentHub.Infrastructure.Options;
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

    public string GenerateToken(Guid userId, string userName, IEnumerable<Claim> additionalClaims)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,userId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName,userName),
            new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        };

        claims.AddRange(additionalClaims);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
           issuer: _jwtOptions.Issuer,
           audience: _jwtOptions.Audience,
           claims: claims,
           expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes),
           signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
