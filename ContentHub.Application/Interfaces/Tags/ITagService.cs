using ContentHub.Application.DTOs.Tags;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Interfaces.Tags;

public interface ITagService
{
    Task<Guid> CreateAsync(CreateTagDto dto);
    Task<List<TagDto>> GetAllAsync();
}
