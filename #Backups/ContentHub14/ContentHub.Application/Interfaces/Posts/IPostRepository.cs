using ContentHub.Domain.Entities;


namespace ContentHub.Application.Interfaces.Posts;

public interface IPostRepository
{
    Task<List<Post>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Post?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Post post, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

}
