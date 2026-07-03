using ContentHub.Domain.Entities;

namespace ContentHub.Application.Interfaces.Tags;

public interface ITagRepository
{
    Task<Tag?> GetByIdAsync(Guid id);
    Task<List<Tag>> GetAllAsync();
    Task AddAsync(Tag tag);
    Task SaveChangesAsync();
}
