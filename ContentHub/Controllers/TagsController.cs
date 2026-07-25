using ContentHub.Application.DTOs.Tags;
using ContentHub.Application.Interfaces.Tags;
using Microsoft.AspNetCore.Mvc;

namespace ContentHub.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        //private readonly ITagService _tagService;

        //public TagsController(ITagService tagService)
        //{
        //    _tagService = tagService;
        //}

        //[HttpGet]
        //public async Task<ActionResult<List<TagDto>>> GetAll(CancellationToken cancellationToken = default)
        //{
        //    return Ok(await _tagService.GetAllAsync());
        //}

        //[HttpPost]
        //public async Task<ActionResult<Guid>> Create(CreateTagDto dto, CancellationToken cancellationToken = default)
        //{
        //    var id = await _tagService.CreateAsync(dto);

        //    return CreatedAtAction(nameof(GetAll), new { id }, id);
        //}
    }
}
