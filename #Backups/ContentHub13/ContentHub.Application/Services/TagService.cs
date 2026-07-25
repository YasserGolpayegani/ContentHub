using ContentHub.Application.DTOs.Tags;
using ContentHub.Application.Interfaces.Tags;
using ContentHub.Application.Mapping;
using ContentHub.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ContentHub.Application.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly ILogger<TagService> _logger;

    public TagService(ITagRepository tagRepository, ILogger<TagService> logger)
    {
        _tagRepository = tagRepository;
        _logger = logger;
    }

    public async Task<List<TagDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var tags = await _tagRepository.GetAllAsync();
        return tags.Select(MapToDto.TagDto).ToList();
    }

    public async Task<Guid> CreateAsync(CreateTagDto dto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating a new Tag with Name '{Name}'", dto.Name);
        var tag = new Tag(dto.Name);
        
        await _tagRepository.AddAsync(tag);

        await _tagRepository.SaveChangesAsync();

        _logger.LogInformation("Tag '{TagId}' created successfully.", tag.Id);

        return tag.Id;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _tagRepository.SaveChangesAsync();
    }
}
