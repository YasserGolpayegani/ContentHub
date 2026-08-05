using ContentHub.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Interfaces.Audit;

public interface IAuditRepository
{
    IQueryable<AuditLog> GetAll();

    Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}