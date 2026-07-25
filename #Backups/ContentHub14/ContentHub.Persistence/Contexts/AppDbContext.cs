using ContentHub.Domain.Common;
using ContentHub.Domain.Entities;
using ContentHub.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContentHub.Persistence.Contexts;

//public sealed class AppDbContext : DbContext
public class AppDbContext:IdentityDbContext<AppUser,IdentityRole<Guid>,Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Tag> Tags => Set<Tag>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.MarkAsCreated();

            if (entry.State == EntityState.Modified)
                entry.Entity.MarkAsModified();
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}
