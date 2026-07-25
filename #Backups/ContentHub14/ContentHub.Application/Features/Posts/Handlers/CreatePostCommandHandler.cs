using ContentHub.Application.Features.Posts.Commands;
using ContentHub.Application.Interfaces.Posts;
using ContentHub.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace ContentHub.Application.Features.Posts.Handlers;

public sealed class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
{
    private readonly IPostRepository _postRepository;

    public CreatePostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = new Post(request.Title, request.Content, request.Slug, request.CategoryId);

        await _postRepository.AddAsync(post, cancellationToken);

        await _postRepository.SaveChangesAsync(cancellationToken);

        return post.Id;
    }
}