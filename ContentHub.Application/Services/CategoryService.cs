using ContentHub.Application.DTOs.Categories;
using ContentHub.Application.Interfaces.Categories;
using ContentHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Services;

public class CategoryService : ICategoryServices
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.Select(x => new CategoryDto
        {
            Id = x.Id,
            Name = x.Name,
            Slug = x.Slug
        }).ToList();
    }

    public async Task<Guid> CreateAsync(CreateCategoryDto dto)
    {
        var category = new Category(dto.Name, dto.Slug);
        
        await _categoryRepository.AddAsync(category);
        
        await _categoryRepository.SaveChangesAsync();
                
        return category.Id;
    }
}
