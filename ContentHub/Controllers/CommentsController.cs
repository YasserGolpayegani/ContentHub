using ContentHub.Application.DTOs.Comments;
using ContentHub.Application.Interfaces.Comments;
using Microsoft.AspNetCore.Mvc;

namespace ContentHub.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/posts/{postId:guid}/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        //private readonly ICommentService _commentService;

        //public CommentsController(ICommentService commentService)
        //{
        //    _commentService = commentService;
        //}

        //[HttpGet]
        //public async Task<ActionResult<List<CommentDto>>> GetByPostId(Guid postId, CancellationToken cancellationToken = default)
        //{
        //    return Ok(await _commentService.GetByPostIdAsync(postId));
        //}

        //[HttpPost]
        //public async Task<ActionResult<Guid>> Create(CreateCommentDto dto, CancellationToken cancellationToken = default)
        //{
        //    var id = await _commentService.CreateAsync(dto);

        //    return Created(string.Empty, id);
        //}
    }
}
