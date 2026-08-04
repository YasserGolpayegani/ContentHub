using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Exceptions;

public sealed class IdentityOperationException:Exception
{
    public IEnumerable<IdentityError> Errors { get; }

    public IdentityOperationException(IEnumerable<IdentityError> errors)
        :base("One or more identity operations failed.")
    {
        Errors = errors.ToArray();
    }
}
