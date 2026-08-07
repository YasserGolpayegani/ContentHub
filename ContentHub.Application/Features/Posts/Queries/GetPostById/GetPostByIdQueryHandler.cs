using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Abstractions.Persistence;
using ContentHub.Application.DTOs.Posts;
using ContentHub.Application.Exceptions;
using ContentHub.Application.Mapping;
using MediatR;

namespace ContentHub.Application.Features.Posts.Queries.GetPostById;

public sealed class GetPostByIdQueryHandler : IQueryHandler<GetPostByIdQuery, PostDto>
{
    private readonly IPostRepository _postRepository;

    public GetPostByIdQueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<PostDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.Id, cancellationToken);

        if (post is null)
            throw new NotFoundException("Post was not found.");

        return MapToDto.PostDto(post);
    }
}

