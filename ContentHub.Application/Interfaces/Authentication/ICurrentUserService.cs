using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Interfaces.Authentication;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    string? IpAddress { get; }
    bool IsAuthenticated { get; }
}
