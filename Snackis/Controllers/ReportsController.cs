using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Snackis.Services;

[Authorize]
public class ReportsController : Controller
{
    private readonly IReportService _reportService;
    private readonly UserManager<IdentityUser> _userManager;

    public ReportsController(IReportService reportService, UserManager<IdentityUser> userManager)
    {
        _reportService = reportService;
        _userManager = userManager;
    }

    // GET: Reports/Create?postId=5
    [HttpGet]
    public IActionResult Create(int postId)
    {
        ViewBag.PostId = postId;
        return View();
    }

    // POST: Reports/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int postId, string reason)
    {
        var userId = _userManager.GetUserId(User)!;
        var error = await _reportService.ReportPostAsync(postId, userId, reason);

        if (error != null)
        {
            ViewBag.PostId = postId;
            ViewBag.Error = error;
            return View();
        }

        TempData["Success"] = "Thank you, the post has been reported.";
        return RedirectToAction("Details", "Posts", new { id = postId });
    }

    // GET: Reports  (admin only)
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        return View(await _reportService.GetAllReportsAsync());
    }

    // POST: Reports/MarkReviewed/3  (admin only)
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkReviewed(int id)
    {
        await _reportService.MarkReviewedAsync(id);
        return RedirectToAction(nameof(Index));
    }
}