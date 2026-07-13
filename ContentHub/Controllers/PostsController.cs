using ContentHub.Application.DTOs.Posts;
using ContentHub.Application.Features.Posts.Commands.CreatePost;
using ContentHub.Application.Features.Posts.Commands.DeletePost;
using ContentHub.Application.Features.Posts.Commands.UpdatePost;
using ContentHub.Application.Interfaces.Posts;
using ContentHub.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContentHub.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        //private readonly IPostService _postService;
        private readonly ISender _sender;

        public PostsController(ISender sender)
        {
            _sender = sender;
        }

        //public PostsController(IPostService postService)
        //{
        //    _postService = postService;
        //}

        //[HttpGet]
        //public async Task<ActionResult<List<PostDto>>> GetAll(CancellationToken cancellationToken = default)
        //{
        //    var posts = await _postService.GetAllAsync();

        //    return Ok(posts);
        //}


        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PostDto>> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            //    var post = await _postService.GetByIdAsync(id);

            //    if (post is null)
            //        return NotFound();

            //return Ok(post);

            return Ok();
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.Author)]
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            CreatePostCommand command, CancellationToken cancellationToken)
        {
            var id = await _sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        //public async Task<ActionResult<Guid>> Create(CreatePostDto dto, CancellationToken cancellationToken = default)
        //{
        //    var id = await _postService.CreateAsync(dto);
        //    return CreatedAtAction(nameof(GetById), new { id }, id);
        //}


        //[Authorize(Roles = Roles.Author + "," + Roles.Admin)]  //Or
        [Authorize(Policy =Policies.AuthorOrAdmin)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            UpdatePostCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest();

            await _sender.Send(command, cancellationToken);

            return NoContent();
        }


        
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            //await _sender.Send(new DeletePostCommand(id), cancellationToken);

            //return NoContent();

            var header = Request.Headers.Authorization.ToString();
            return Ok(header);
        }
    }
}
