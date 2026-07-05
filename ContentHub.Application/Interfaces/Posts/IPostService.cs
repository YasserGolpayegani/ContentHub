using ContentHub.Application.DTOs.Posts;

namespace ContentHub.Application.Interfaces.Posts;

public interface IPostService
{
    Task<List<PostDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PostDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(CreatePostDto dto, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
