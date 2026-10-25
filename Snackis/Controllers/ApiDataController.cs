using Microsoft.AspNetCore.Mvc;
using Snackis.Services;

public class ApiDataController : Controller
{
    private readonly IForumApiClient _api;

    public ApiDataController(IForumApiClient api)
    {
        _api = api;
    }

    // GET: ApiData/Categories
    public async Task<IActionResult> Categories()
    {
        return View(await _api.GetCategoriesAsync());
    }

    // GET: ApiData/Posts
    public async Task<IActionResult> Posts()
    {
        return View(await _api.GetAllPostsAsync());
    }
}