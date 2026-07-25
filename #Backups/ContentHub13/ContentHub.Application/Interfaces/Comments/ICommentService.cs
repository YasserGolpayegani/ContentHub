using ContentHub.Application.DTOs.Comments;

namespace ContentHub.Application.Interfaces.Comments;

public interface ICommentService
{
    Task<List<CommentDto>> GetByPostIdAsync(Guid postId, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(CreateCommentDto dto, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
