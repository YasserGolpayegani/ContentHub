using ContentHub.Domain.Entities;


namespace ContentHub.Application.Abstractions.Persistence;

public interface IPostRepository
{
    Task<IReadOnlyList<Post>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Post?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Post post, CancellationToken cancellationToken = default);
    void Update(Post post);
    void Delete(Post post); 
}
