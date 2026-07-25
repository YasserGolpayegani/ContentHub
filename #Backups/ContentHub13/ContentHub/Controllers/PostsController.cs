using ContentHub.Application.DTOs.Posts;
using ContentHub.Application.Interfaces.Posts;
using Microsoft.AspNetCore.Mvc;

namespace ContentHub.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PostDto>>> GetAll(CancellationToken cancellationToken = default)
        {
            var posts = await _postService.GetAllAsync();

            return Ok(posts);
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PostDto>> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var post = await _postService.GetByIdAsync(id);

            if (post is null)
                return NotFound();

            return Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreatePostDto dto, CancellationToken cancellationToken = default)
        {
            var id = await _postService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
    }
}
