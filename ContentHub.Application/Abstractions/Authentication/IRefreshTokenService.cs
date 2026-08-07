using ContentHub.Domain.Entities;
using ContentHub.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Abstractions.Authentication;

public interface IRefreshTokenService
{
    RefreshToken Generate(AppUser user);
    bool IsValid(RefreshToken refreshToken);    
    RefreshToken Rotate(RefreshToken refreshToken);
}
