using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Abstractions.Persistence;
using ContentHub.Application.Exceptions;
using MediatR;

namespace ContentHub.Application.Features.Posts.Commands.DeletePost;

public sealed class DeletePostCommandHandler : ICommandHandler<DeletePostCommand>
{
    private readonly IPostRepository _postRepository;

    public DeletePostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.id, cancellationToken);

        if (post is null)
            throw new NotFoundException("Post was not found.");

        _postRepository.Delete(post);

        //Saving perform on UnitOfWork
        //await _postRepository.SaveChangesAsync(cancellationToken);
    }
}