using ContentHub.Application.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Interfaces.Categories;

public interface ICategoryServices
{
    Task<List<CategoryDto>> GetAllAsync();
    Task<Guid> CreateAsync(CreateCategoryDto dto);
}
