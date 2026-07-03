using ContentHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace ContentHub.Application.Interfaces.Posts;

public interface IPostRepository
{
    Task<List<Post>> GetAllAsync();
    Task<Post?> GetByIdAsync(Guid id);
    Task AddAsync(Post post);
    Task SaveChangesAsync();

}
