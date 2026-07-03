using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ContentHub.Domain.Common;

public class BaseAuditableEntity : BaseEntity, ISoftDelete
{
    /// <summary>
    /// Represents an auditable entity in the system.
    /// </summary>
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ModifiedOnUtc { get; private set; }
    public DateTime? DeletedOnUtc { get; private set; }
    public bool IsDeleted { get; private set; }

    public void MarkAsCreated()
    {
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void MarkAsModified()
    {
        ModifiedOnUtc = DateTime.UtcNow;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
        DeletedOnUtc = DateTime.UtcNow;
    }
}
