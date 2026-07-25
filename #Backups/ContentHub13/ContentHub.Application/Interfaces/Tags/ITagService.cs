using ContentHub.Application.DTOs.Tags;

namespace ContentHub.Application.Interfaces.Tags;

public interface ITagService
{
    Task<List<TagDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(CreateTagDto dto, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
