using Microsoft.AspNetCore.Mvc;
using Snackis.Api.Dtos;
using Snackis.Api.Services;

namespace Snackis.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly IForumService _service;

    public CategoriesController(IForumService service)
    {
        _service = service;
    }

    // GET: api/categories
    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAll()
    {
        return await _service.GetCategoriesAsync();
    }

    // GET: api/categories/3/posts
    [HttpGet("{id:int}/posts")]
    public async Task<ActionResult<List<PostDto>>> GetPosts(int id)
    {
        var posts = await _service.GetPostsForCategoryAsync(id);
        if (posts == null) return NotFound();

        return posts;
    }
}