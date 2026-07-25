using ContentHub.Application.DTOs.Posts;
using ContentHub.Application.Interfaces.Posts;
using ContentHub.Application.Mapping;
using ContentHub.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ContentHub.Application.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly ILogger<PostService> _logger;

    public PostService(IPostRepository postRepository, ILogger<PostService> logger)
    {
        _postRepository = postRepository;
        _logger = logger;
    }

    public async Task<List<PostDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var posts = await _postRepository.GetAllAsync();

        return posts.Select(MapToDto.PostDto).ToList();
    }

    public async Task<PostDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var post = await _postRepository.GetByIdAsync(id);

        if (post == null)
            return null;

        return MapToDto.PostDto(post);
    }

    public async Task<Guid> CreateAsync(CreatePostDto dto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating a new post with title '{Title}'.", dto.Title);

        Post post = new Post(dto.Title, dto.Content, dto.Slug, dto.CategoryId);

        await _postRepository.AddAsync(post);


        await _postRepository.SaveChangesAsync();

        _logger.LogInformation("Post '{PostId}' created successfully.", post.Id);

        return post.Id;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _postRepository.SaveChangesAsync();
    }
}
