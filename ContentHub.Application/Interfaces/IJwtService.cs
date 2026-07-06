using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ContentHub.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(Guid userId, string userName, IEnumerable<Claim> additionalClaims);
}
