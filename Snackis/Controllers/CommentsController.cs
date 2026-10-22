using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Snackis.Services;

namespace Snackis.Controllers;

public class CommentsController : Controller
{
    private readonly ICommentService _commentService;
    private readonly UserManager<IdentityUser> _userManager;

    public CommentsController(ICommentService commentService, UserManager<IdentityUser> userManager)
    {
        _commentService = commentService;
        _userManager = userManager;
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int postId, string text)
    {
        var userId = _userManager.GetUserId(User)!;
        await _commentService.AddCommentAsync(postId, text, userId);
        return RedirectToAction("Details", "Posts", new { id = postId});
    }
}
