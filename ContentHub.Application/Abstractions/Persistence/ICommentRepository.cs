using ContentHub.Domain.Entities;

namespace ContentHub.Application.Abstractions.Persistence;

public interface ICommentRepository
{
    Task<List<Comment>> GetByPostIdAsync(Guid postId, CancellationToken cancellationToken = default);
    Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Comment comment, CancellationToken cancellationToken = default);    
}
