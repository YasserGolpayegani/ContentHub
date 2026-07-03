using System;
using System.Collections.Generic;
using System.Text;
using ContentHub.Application.DTOs.Posts;

namespace ContentHub.Application.Interfaces.Posts;

public interface IPostService
{
    Task<List<PostDto>> GetAllAsync();
    Task<PostDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CreatePostDto dto);
}
