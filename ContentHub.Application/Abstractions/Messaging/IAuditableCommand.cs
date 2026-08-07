using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Abstractions.Messaging;

/// <summary>
/// Represents a command that should be audited.
/// </summary>
public interface IAuditableCommand
{
    string Action { get; }
    string EntityName { get; }
    string? EntityId { get; }
}