using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Exceptions;

/// <summary>
/// Represents an unauthorized request.
/// </summary>
public sealed class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message)
    {

    }
}

