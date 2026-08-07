using ContentHub.Domain.Entities;

namespace ContentHub.Application.Abstractions.Persistence;

public interface ITagRepository
{
    Task<List<Tag>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Tag?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Tag tag, CancellationToken cancellationToken = default);    
}
