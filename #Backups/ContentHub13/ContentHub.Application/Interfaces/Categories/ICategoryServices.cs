using ContentHub.Application.DTOs.Categories;

namespace ContentHub.Application.Interfaces.Categories;

public interface ICategoryServices
{
    Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CategoryDto?> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
