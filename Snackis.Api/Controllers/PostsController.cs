using Microsoft.AspNetCore.Mvc;
using Snackis.Api.Dtos;
using Snackis.Api.Services;

namespace Snackis.Api.Controllers;

[ApiController]
[Route("api/posts")]
public class PostsController : ControllerBase
{
    private readonly IForumService _service;

    public PostsController(IForumService service)
    {
        _service = service;
    }

    // GET: api/posts
    [HttpGet]
    public async Task<ActionResult<List<PostDto>>> GetAll()
    {
        return await _service.GetAllPostsAsync();
    }
}