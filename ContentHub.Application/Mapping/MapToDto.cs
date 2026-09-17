using ContentHub.Application.DTOs.Categories;
using ContentHub.Application.DTOs.Comments;
using ContentHub.Application.DTOs.Posts;
using ContentHub.Application.DTOs.Tags;
using ContentHub.Domain.Entities;

namespace ContentHub.Application.Mapping;

public static class MapToDto
{
    public static PostDto PostDto(Post post)
    {
        return new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Slug = post.Slug,
            IsPublished = post.IsPublished,
            CreatedOnUtc = post.CreatedOnUtc
        };
    }

    public static PostV2Dto PostV2Dto(Post post)
    {
        return new PostV2Dto
        {
            Id = post.Id,
            Title = post.Title,
            Slug = post.Slug,
            IsPublished = post.IsPublished,
            CreatedOnUtc = post.CreatedOnUtc,
            AuthorName = "Author" //just for represent versioning feature.
        };
    }

    public static CategoryDto CategoryDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug
        };
    }

    public static TagDto TagDto(Tag tag)
    {
        return new TagDto
        {
            Id = tag.Id,
            Name = tag.Name
        };
    }

    public static CommentDto CommentDto(Comment comment)
    {
        return new CommentDto
        {
            Id = comment.Id,
            AuthorName = comment.AuthorName,
            Content = comment.Content,
            PostId = comment.PostId
        };
    }
    
}
