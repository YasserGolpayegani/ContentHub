using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Domain.Identity;

public static class Policies
{
    public const string AdminOnly = nameof(AdminOnly);
    public const string AuthorOrAdmin = nameof(AuthorOrAdmin);
}
