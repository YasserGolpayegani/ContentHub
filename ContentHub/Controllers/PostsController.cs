using ContentHub.Api.Authorization;
using ContentHub.Application.Authorization;
using ContentHub.Application.DTOs.Posts;
using ContentHub.Application.Features.Posts.Commands.CreatePost;
using ContentHub.Application.Features.Posts.Commands.DeletePost;
using ContentHub.Application.Features.Posts.Commands.UpdatePost;
using ContentHub.Application.Features.Posts.Queries.GetAllPostsQuery;
using ContentHub.Application.Features.Posts.Queries.GetPostById;
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


        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PostDto>> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var post = await _sender.Send(new GetPostByIdQuery(id), cancellationToken);
            //var post = await _postService.GetByIdAsync(id);

            if (post is null)
                return NotFound();

            return Ok(post);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<PostDto>>> GetAll(CancellationToken cancellationToken)
        {
            var posts = await _sender.Send(new GetAllPostsQuery(), cancellationToken);
            return Ok(posts);
        }


        //[Authorize(Roles = Roles.Admin + "," + Roles.Author)]
        //[Authorize(Policy = Policies.CreatePosts)]
        //[Authorize(Policy = PermissionNames.Posts.Create)]
        [HasPermission(PermissionNames.Posts.Create)]
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            CreatePostCommand command, CancellationToken cancellationToken)
        {
            var id = await _sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }


        //[Authorize(Roles = Roles.Author + "," + Roles.Admin)]  //Or
        //[Authorize(Policy = Policies.AuthorOrAdmin)]
        [HasPermission(PermissionNames.Posts.Update)]
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



        //[Authorize(Roles = Roles.Admin)]
        [HasPermission(PermissionNames.Posts.Update)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _sender.Send(new DeletePostCommand(id), cancellationToken);

            return NoContent();           
        }
    }
}
