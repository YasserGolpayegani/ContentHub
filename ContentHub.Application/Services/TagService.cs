using ContentHub.Application.DTOs.Tags;
using ContentHub.Application.Interfaces.Tags;
using ContentHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;

    public TagService(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<Guid> CreateAsync(CreateTagDto dto)
    {
        var tag = new Tag(dto.Name);

        await _tagRepository.AddAsync(tag);

        await _tagRepository.SaveChangesAsync();

        return tag.Id;
    }

    public async Task<List<TagDto>> GetAllAsync()
    {
        var tags = await _tagRepository.GetAllAsync();
        return tags.Select(x => new TagDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();
    }
}
