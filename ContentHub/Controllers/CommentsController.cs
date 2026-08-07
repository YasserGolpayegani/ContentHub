using ContentHub.Application.DTOs.Comments;
using Microsoft.AspNetCore.Mvc;

namespace ContentHub.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/posts/{postId:guid}/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        
    }
}
