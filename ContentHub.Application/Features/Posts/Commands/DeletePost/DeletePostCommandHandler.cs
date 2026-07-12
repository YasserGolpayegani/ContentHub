using ContentHub.Application.Exceptions;
using ContentHub.Application.Interfaces.Posts;
using MediatR;

namespace ContentHub.Application.Features.Posts.Commands.DeletePost;

public sealed class DeletePostCommandHandler : IRequestHandler<DeletePostCommand>
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

        await _postRepository.SaveChangesAsync(cancellationToken);
    }
}