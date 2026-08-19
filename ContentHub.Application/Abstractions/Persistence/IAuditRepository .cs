using ContentHub.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Abstractions.Persistence;

public interface IAuditRepository
{
    Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken = default);
}