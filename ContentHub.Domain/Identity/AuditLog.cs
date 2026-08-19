using ContentHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Domain.Identity;

/// <summary>
/// Represents an audit entry for a business operation.
/// </summary>
public sealed class AuditLog:BaseEntity
{
    private AuditLog()
    {
    }

    public AuditLog(
        Guid? userId,
        string? userName,
        string action,
        string entityName,
        string? entityId,
        DateTime timestampUtc,
        bool isSuccessful,
        string? failureReason,
        string? ipAddress,
        string? oldValues=null,
        string? newValues=null)
    {
        UserId = userId;
        UserName = userName;
        Action = action;
        EntityName = entityName;
        EntityId = entityId;
        TimestampUtc = timestampUtc;
        IsSuccessful = isSuccessful;
        FailureReason = failureReason;
        IpAddress = ipAddress;
        OldValues = oldValues;
        NewValues = newValues;
    }

    public Guid? UserId { get; }
    public string? UserName { get; }
    public string Action { get; }
    public string EntityName { get; }
    public string? EntityId { get; }
    public DateTime TimestampUtc { get; }
    public bool IsSuccessful { get; }
    public string? FailureReason { get; }
    public string? IpAddress { get; }
    public string? OldValues { get; }
    public string? NewValues { get; }
}