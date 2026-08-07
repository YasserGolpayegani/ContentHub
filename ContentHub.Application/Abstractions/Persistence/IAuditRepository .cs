using ContentHub.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Abstractions.Persistence;

public interface IAuditRepository
{
    //IQueryable<AuditLog> GetAll();

    Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken = default);

    //SaveChanges will ads in UnitOfWork
    //Task SaveChangesAsync(CancellationToken cancellationToken = default);
}