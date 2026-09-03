using ContentHub.Application.Abstractions.Caching;
using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Abstractions.Persistence;
using ContentHub.Application.Constants;
using ContentHub.Application.DTOs.Posts;
using ContentHub.Application.Mapping;
using MediatR;

namespace ContentHub.Application.Features.Posts.Queries.GetAllPostsQuery;

public sealed class GetAllPostsQueryHandler : IQueryHandler<GetAllPostsQuery, IReadOnlyList<PostDto>>
{
    private readonly IPostRepository _postRepository;
    private readonly ICacheService _cacheService;

    public GetAllPostsQueryHandler(IPostRepository postRepository, ICacheService cacheService)
    {
        _postRepository = postRepository;
        _cacheService = cacheService;
    }

    public async Task<IReadOnlyList<PostDto>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        var cachedPosts = await _cacheService.GetAsync<List<PostDto>>(CacheKeys.Posts);

        if (cachedPosts is not null)
            return cachedPosts;

        var posts = await _postRepository.GetAllAsync(cancellationToken);

        var postDtos = posts.Select(MapToDto.PostDto).ToList();

        await _cacheService.SetAsync(CacheKeys.Posts, postDtos, TimeSpan.FromMinutes(5));

        return postDtos ;
    }
}


