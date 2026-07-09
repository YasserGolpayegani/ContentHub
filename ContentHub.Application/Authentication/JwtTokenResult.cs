using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Authentication;

public sealed record JwtTokenResult(string Token, DateTime ExpiresAtUtc);



