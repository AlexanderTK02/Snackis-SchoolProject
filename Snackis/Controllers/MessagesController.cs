using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Snackis.Services;

[Authorize]
public class MessagesController : Controller
{
    private readonly IMessageService _messageService;
    private readonly UserManager<IdentityUser> _userManager;

    public MessagesController(IMessageService messageService, UserManager<IdentityUser> userManager)
    {
        _messageService = messageService;
        _userManager = userManager;
    }

    // GET: Messages  (list of conversations)
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User)!;
        return View(await _messageService.GetConversationsAsync(userId));
    }

    // POST: Messages/Start  (start a conversation by username)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Start(string username)
    {
        var otherId = string.IsNullOrWhiteSpace(username)
            ? null
            : await _messageService.FindUserIdAsync(username);

        if (otherId == null)
        {
            TempData["Error"] = "No user with that username was found.";
            return RedirectToAction(nameof(Index));
        }
        if (otherId == _userManager.GetUserId(User))
        {
            TempData["Error"] = "You can't start a conversation with yourself.";
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Conversation), new { id = otherId });
    }

    // GET: Messages/Conversation/{userId}
    public async Task<IActionResult> Conversation(string id)
    {
        var userId = _userManager.GetUserId(User)!;
        var model = await _messageService.GetConversationAsync(userId, id);
        if (model == null) return NotFound();

        return View(model);
    }

    // POST: Messages/Send/{userId}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Send(string id, string text)
    {
        var userId = _userManager.GetUserId(User)!;
        var error = await _messageService.SendAsync(userId, id, text);

        if (error != null)
            TempData["Error"] = error;

        return RedirectToAction(nameof(Conversation), new { id });
    }
}