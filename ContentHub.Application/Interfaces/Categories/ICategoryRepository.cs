using ContentHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Interfaces.Categories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid id);
    Task AddAsync(Category category);
    Task SaveChangesAsync();
}
