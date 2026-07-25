using ContentHub.Application.DTOs.Posts;
using ContentHub.Application.Interfaces.Posts;
using ContentHub.Application.Mapping;
using MediatR;

namespace ContentHub.Application.Features.Posts.Queries.GetAllPostsQuery;

public sealed class GetAllPostsQueryHandler: IRequestHandler<GetAllPostsQuery, IReadOnlyList<PostDto>>
{
    private readonly IPostRepository _postRepository;

    public GetAllPostsQueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<IReadOnlyList<PostDto>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _postRepository.GetAllAsync();

        return posts.Select(MapToDto.PostDto).ToList();        
    }
}


