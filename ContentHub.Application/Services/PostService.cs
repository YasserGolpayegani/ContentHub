using ContentHub.Application.DTOs;
using ContentHub.Application.Interfaces;
using ContentHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;

    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<List<PostDto>> GetAllAsync()
    {
        var posts = await _postRepository.GetAllAsync();

        return posts.Select(x => new PostDto
        {
            Id = x.Id,
            Title = x.Title,
            Slug = x.Slug,
            IsPublished = x.IsPublished,
            CreatedOnUtc = x.CreatedOnUtc
        }).ToList();
    }

    public async Task<PostDto?> GetByIdAsync(Guid id)
    {
        var post = await _postRepository.GetByIdAsync(id);

        if (post == null)
            return null;

        return new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Slug = post.Slug,
            IsPublished = post.IsPublished,
            CreatedOnUtc = post.CreatedOnUtc
        };
    }

    public async Task<Guid> CreateAsync(CreatePostDto dto)
    {
        Post post = new Post(dto.Title, dto.Content, dto.Slug);

        await _postRepository.AddAsync(post);

        return post.Id;
    }
}
