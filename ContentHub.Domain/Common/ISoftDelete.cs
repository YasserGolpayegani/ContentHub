using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Domain.Common;

/// <summary>
/// Represent an entity that supports soft deletion
/// </summary>
public interface ISoftDelete
{
    bool IsDeleted { get; }
    DateTime? DeletedOnUtc { get; }
    void MarkAsDeleted();
}
