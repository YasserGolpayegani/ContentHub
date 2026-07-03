using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Domain.Common;

/// <summary>
/// Represents the base entity for all domain entities.
/// It provides a primary key and audit information shared across the system.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; protected set; }    
}
