using ContentHub.Domain.Entities;

namespace ContentHub.Application.Interfaces.Comments;

public interface ICommentRepository
{
    Task<Comment?> GetByIdAsync(Guid id);
    Task<List<Comment>> GetByPostIdAsync(Guid postId);
    Task AddAsync(Comment comment);
    Task SaveChangesAsync();
}
