using ContentHub.Application.Interfaces.Tags;
using ContentHub.Domain.Entities;
using ContentHub.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ContentHub.Persistence.Repositories;

public class TagRepository : ITagRepository
{
    private readonly AppDbContext _context;

    public TagRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tag>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Tags.OrderBy(x => x.Name).ToListAsync();
    }

    public async Task<Tag?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        await _context.Tags.AddAsync(tag);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync();
    }
}
