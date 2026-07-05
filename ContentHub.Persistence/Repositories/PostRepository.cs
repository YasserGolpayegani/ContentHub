using ContentHub.Application.Interfaces.Posts;
using ContentHub.Domain.Entities;
using ContentHub.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ContentHub.Persistence.Repositories;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _context;

    public PostRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Post>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Posts.OrderByDescending(x => x.CreatedOnUtc).ToListAsync();
    }

    public async Task<Post?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Post post, CancellationToken cancellationToken = default)
    {
        await _context.Posts.AddAsync(post);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync();
    }
}
