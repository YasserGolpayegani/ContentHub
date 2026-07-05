using ContentHub.Application.DTOs.Categories;
using ContentHub.Application.Interfaces.Categories;
using ContentHub.Application.Mapping;
using ContentHub.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ContentHub.Application.Services;

public class CategoryService : ICategoryServices
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    public async Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.Select(MapToDto.CategoryDto).ToList();
    }

    public async Task<Guid> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating a new category with name '{Title}'.", dto.Name);

        var category = new Category(dto.Name, dto.Slug);

        await _categoryRepository.AddAsync(category);

        await _categoryRepository.SaveChangesAsync();

        _logger.LogInformation("Category '{CategoryId}' created successfully.", category.Id);

        return category.Id;
    }

    public async Task<CategoryDto?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
            return null;

        return MapToDto.CategoryDto(category);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _categoryRepository.SaveChangesAsync();
    }
}
