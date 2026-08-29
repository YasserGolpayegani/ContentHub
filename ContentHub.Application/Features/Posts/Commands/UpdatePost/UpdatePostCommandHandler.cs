using ContentHub.Application.Abstractions.Caching;
using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Abstractions.Persistence;
using ContentHub.Application.Constants;
using ContentHub.Application.Exceptions;
using MediatR;

namespace ContentHub.Application.Features.Posts.Commands.UpdatePost;

public sealed class UpdatePostCommandHandler : ICommandHandler<UpdatePostCommand>
{
    private readonly IPostRepository _postRepository;
    private readonly ICacheService _cacheService;

    public UpdatePostCommandHandler(IPostRepository postRepository, ICacheService cacheService)
    {
        _postRepository = postRepository;
        _cacheService = cacheService;
    }

    async Task IRequestHandler<UpdatePostCommand>.Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.Id, cancellationToken);

        if (post is null)
            throw new NotFoundException("Post was not found.");

        post.Update(request.Title, request.Content, request.Slug, request.CategoryId);

        _postRepository.Update(post);

        await _cacheService.RemoveAsync(CacheKeys.Posts);

        //Saving perform on UnitOfWork
        //await _postRepository.SaveChangesAsync(cancellationToken);
    }
}

