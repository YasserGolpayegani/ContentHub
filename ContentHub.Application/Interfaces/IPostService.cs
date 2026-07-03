using System;
using System.Collections.Generic;
using System.Text;
using ContentHub.Application.DTOs;
namespace ContentHub.Application.Interfaces;

public interface IPostService
{
    Task<List<PostDto>> GetAllAsync();
    Task<PostDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CreatePostDto dto);
}
