using ContentHub.Application.DTOs.Categories;
using ContentHub.Application.Interfaces.Categories;
using Microsoft.AspNetCore.Mvc;

namespace ContentHub.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
    //    private readonly ICategoryServices _categoryService;

    //    public CategoriesController(ICategoryServices categoryService)
    //    {
    //        _categoryService = categoryService;
    //    }


    //    [HttpGet]
    //    public async Task<ActionResult<List<CategoryDto>>> GetAll(CancellationToken cancellationToken = default)
    //    {
    //        var posts = await _categoryService.GetAllAsync();

    //        return Ok(posts);
    //    }


    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<CategoryDto>> GetById(Guid id, CancellationToken cancellationToken = default)
    //    {
    //        var post = await _categoryService.GetById(id);

    //        if (post == null)
    //            return NotFound();

    //        return Ok(post);
    //    }

    //    [HttpPost]
    //    public async Task<ActionResult<Guid>> Create(CreateCategoryDto dto, CancellationToken cancellationToken = default)
    //    {
    //        var id = await _categoryService.CreateAsync(dto);

    //        return CreatedAtAction(nameof(GetById), new { id }, id);
    //    }
    }
}
