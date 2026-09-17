using ContentHub.Application.Abstractions.Caching;
using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Abstractions.Persistence;
using ContentHub.Application.Constants;
using ContentHub.Domain.Entities;


namespace ContentHub.Application.Features.Posts.Commands.CreatePost;

public sealed class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, Guid>
{
    private readonly IPostRepository _postRepository;
    private readonly ICacheService _cacheService;
    public CreatePostCommandHandler(IPostRepository postRepository, ICacheService cacheService)
    {
        _postRepository = postRepository;
        _cacheService = cacheService;
    }

    public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = new Post(request.Title, request.Content, request.Slug, request.CategoryId);

        await _postRepository.AddAsync(post, cancellationToken);

        await _cacheService.RemoveAsync(CacheKeys.Posts);

        //Saving perform on UnitOfWork
        //await _postRepository.SaveChangesAsync(cancellationToken);

        return post.Id;
    }
}