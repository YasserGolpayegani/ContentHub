using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Abstractions.Persistence;
using ContentHub.Application.Features.Posts.Commands;
using ContentHub.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace ContentHub.Application.Features.Posts.Commands.CreatePost;

public sealed class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, Guid>
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

        //Saving perform on UnitOfWork
        //await _postRepository.SaveChangesAsync(cancellationToken);

        return post.Id;
    }
}