using ContentHub.Application.Authentication;
using ContentHub.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ContentHub.Application.Abstractions.Authentication;

public interface IJwtService
{
    JwtTokenResult GenerateToken(AppUser user, IEnumerable<string> roles);
}
