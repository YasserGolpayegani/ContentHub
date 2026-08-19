using ContentHub.Application.Abstractions.Authentication;
using ContentHub.Domain.Abstractions;
using ContentHub.Domain.Common;
using ContentHub.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ContentHub.Persistence.Interceptors;

public sealed class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private static readonly HashSet<string> IgnoredProperties =
        [
            nameof(BaseAuditableEntity.ModifiedOnUtc),
            "PasswordHash",
            "SecurityStamp",
            "ConcurrencyStamp"
        ];

    private readonly ICurrentUserService _currentUserService;

    public AuditSaveChangesInterceptor(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken)
    {
        var context = eventData.Context;

        if (context == null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = context.ChangeTracker
            .Entries<IAuditableEntity>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();

        foreach (var entry in entries)
        {
            var changes = GetChanges(entry);

            if (changes.Count == 0) continue;

            var auditLog = CreateAuditLog(entry, changes);

            context.Set<AuditLog>().Add(auditLog);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }


    private AuditLog CreateAuditLog(EntityEntry entry, IReadOnlyList<AuditChange> changes)
    {
        string oldValues = JsonSerializer.Serialize(changes.ToDictionary(
            change => change.PropertyName,
            change => change.OldValue));

        string newValues = JsonSerializer.Serialize(changes.ToDictionary(
            change => change.PropertyName,
            change => change.NewValue));

        return new AuditLog(
            _currentUserService.UserId,
            _currentUserService.UserName,
            entry.State.ToString(),
            entry.Metadata.ClrType.Name,
            GetEntityId(entry),
            DateTime.UtcNow,
            true,
            null,
            _currentUserService.IpAddress,
            oldValues,
            newValues);
    }

    private static string? GetEntityId(EntityEntry entry)
    {
        var primaryKey = entry.Metadata.FindPrimaryKey();

        if (primaryKey is null) return null;

        var keyValues = primaryKey.Properties
            .Select(property =>
            entry.State == EntityState.Deleted
            ? entry.Property(property.Name).OriginalValue
            : entry.Property(property.Name).CurrentValue)
            .ToArray();

        return string.Join(",", keyValues.Select(value => value?.ToString()));
    }


    private static IReadOnlyList<AuditChange> GetChanges(EntityEntry entry)
    {
        var changes = new List<AuditChange>();

        foreach (var property in entry.Properties)
        {
            if (IgnoredProperties.Contains(property.Metadata.Name)) continue;

            switch (entry.State)
            {
                case EntityState.Added:
                    changes.Add(new AuditChange(property.Metadata.Name, null, property.CurrentValue));
                    break;

                case EntityState.Modified:
                    if (!property.IsModified) continue;

                    if(Equals(property.OriginalValue,property.CurrentValue)) continue;

                    changes.Add(new AuditChange(property.Metadata.Name, property.OriginalValue, property.CurrentValue));
                    break;

                case EntityState.Deleted:
                    changes.Add(new AuditChange(property.Metadata.Name, property.OriginalValue, null));
                    break;
            }
        }

        return changes;
    }


    private sealed record AuditChange(string PropertyName, object? OldValue, object? NewValue);    
}
