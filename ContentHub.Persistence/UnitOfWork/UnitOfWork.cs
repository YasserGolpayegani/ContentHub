using ContentHub.Application.Interfaces.Persistence;
using ContentHub.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace ContentHub.Persistence.UnitOfWork;

public sealed class UnitOfWork:IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
