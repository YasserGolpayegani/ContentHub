using ContentHub.Application.Exceptions;
using ContentHub.Application.Interfaces.Posts;
using MediatR;

namespace ContentHub.Application.Features.Posts.Commands.UpdatePost;

public sealed class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand>
{
    private readonly IPostRepository _postRepository;

    public UpdatePostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    async Task IRequestHandler<UpdatePostCommand>.Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.Id, cancellationToken);

        if (post is null)
            throw new NotFoundException("Post was not found.");

        post.Update(request.Title, request.Content, request.Slug, request.CategoryId);

        _postRepository.Update(post);

        await _postRepository.SaveChangesAsync(cancellationToken);
    }
}

