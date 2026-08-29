using ContentHub.Application.Abstractions.Caching;
using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Abstractions.Persistence;
using ContentHub.Application.Constants;
using ContentHub.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ContentHub.Application.Features.Posts.Commands.DeletePost;

public sealed class DeletePostCommandHandler : ICommandHandler<DeletePostCommand>
{
    private readonly IPostRepository _postRepository;
    private readonly ILogger<DeletePostCommandHandler> _logger;
    private readonly ICacheService _cacheService;

    public DeletePostCommandHandler(IPostRepository postRepository, ILogger<DeletePostCommandHandler> logger, ICacheService cacheService)
    {
        _postRepository = postRepository;
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.id, cancellationToken);

        if (post is null)
            throw new NotFoundException("Post was not found.");

        _postRepository.Delete(post);
        
        _logger.LogInformation("The post {postId} is deleted", post.Id);

        await _cacheService.RemoveAsync(CacheKeys.Posts);
        //Saving perform on UnitOfWork
        //await _postRepository.SaveChangesAsync(cancellationToken);
    }
}