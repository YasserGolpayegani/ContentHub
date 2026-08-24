using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Abstractions.Persistence;
using ContentHub.Application.DTOs.Posts;
using ContentHub.Application.Mapping;

namespace ContentHub.Application.Features.Posts.Queries.GetAllPostsQuery;

public sealed class GetAllPostsV2QueryHandler : IQueryHandler<GetAllPostsV2Query, IReadOnlyList<PostV2Dto>>
{
    private readonly IPostRepository _postRepository;

    public GetAllPostsV2QueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }
   
    public async Task<IReadOnlyList<PostV2Dto>> Handle(GetAllPostsV2Query request, CancellationToken cancellationToken)
    {
        var posts = await _postRepository.GetAllAsync();

        return posts.Select(MapToDto.PostV2Dto).ToList();
    }
}


