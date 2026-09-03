using ContentHub.Application.DTOs.Comments;
using ContentHub.Application.Interfaces.Comments;
using ContentHub.Application.Mapping;
using ContentHub.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ContentHub.Application.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly ILogger<CommentService> _logger;

    public CommentService(ICommentRepository commentRepository, ILogger<CommentService> commentService)
    {
        _commentRepository = commentRepository;
        _logger = commentService;
    }


    public async Task<List<CommentDto>> GetByPostIdAsync(Guid postId, CancellationToken cancellationToken = default)
    {
        var comments = await _commentRepository.GetByPostIdAsync(postId);
        return comments.Select(MapToDto.CommentDto).ToList();
    }

    public async Task<Guid> CreateAsync(CreateCommentDto dto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating a new comment by '{Author}'", dto.AuthorName);

        var comment = new Comment(dto.AuthorName, dto.Content, dto.PostId);

        await _commentRepository.AddAsync(comment);

        await _commentRepository.SaveChangesAsync();

        _logger.LogInformation("Category '{CategoryId}' created successfully'", comment.Id);

        return comment.Id;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _commentRepository.SaveChangesAsync();
    }
}
