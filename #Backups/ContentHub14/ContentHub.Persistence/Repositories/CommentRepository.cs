using ContentHub.Application.Interfaces.Comments;
using ContentHub.Domain.Entities;
using ContentHub.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ContentHub.Persistence.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly AppDbContext _context;

    public CommentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Comment>> GetByPostIdAsync(Guid postId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments.Where(x => x.PostId == postId).ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        await _context.Comments.AddAsync(comment);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync();
    }
}
