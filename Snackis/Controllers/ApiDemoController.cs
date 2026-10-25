using Microsoft.AspNetCore.Mvc;
using Snackis.Services;
using Snackis.ApiModels;

namespace Snackis.Controllers;

public class ApiDemoController : Controller
{
    private readonly IForumApiClient _api;
    private readonly IConfiguration _config;

    public ApiDemoController(IForumApiClient api, IConfiguration config)
    {
        _api = api;
        _config = config;
    }

    // GET: ApiDemo?categoryId=3
    public async Task<IActionResult> Index(int? categoryId)
    {
        var model = new ApiDemoViewModel
        {
            Categories = await _api.GetCategoriesAsync(),
            SelectedCategoryId = categoryId,
            ApiBaseUrl = _config["ApiSettings:BaseUrl"] ?? string.Empty
        };

        if (categoryId != null && model.Categories != null)
        {
            model.Posts = await _api.GetPostsAsync(categoryId.Value);
        }

        return View(model);
    }
}
