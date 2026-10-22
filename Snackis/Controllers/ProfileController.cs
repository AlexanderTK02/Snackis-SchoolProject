using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Snackis.Services;

namespace Snackis.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly IProfileService _profileService;
    private readonly UserManager<IdentityUser> _userManager;

    public ProfileController(IProfileService profileService, UserManager<IdentityUser> userManager)
    {
        _profileService = profileService;
        _userManager = userManager;
    }

    // GET: Profile
    public IActionResult Index()
    {
        ViewBag.UserId = _userManager.GetUserId(User);
        return View();
    }

    // POST: Profile/Upload
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(IFormFile? image)
    {
        var userId = _userManager.GetUserId(User)!;
        var error = await _profileService.SaveImageAsync(userId, image);

        if (error == null)
            TempData["Success"] = "Profile picture uploaded.";
        else
            TempData["Error"] = error;

        return RedirectToAction(nameof(Index));
    }

    // GET: Profile/Image/{userId} (Anyone can view pictures)
    // AI helped me a bit with figuring this one out, ImageData is the code behind a picture and ImageContentType is the type of picture it is
    // This helps with reconstructing the picture once we need it. All profile pictures are in the Database.
    [AllowAnonymous]
    public async Task<IActionResult> Image(string id)
    {
        var profile = await _profileService.GetProfileAsync(id);
        if (profile?.ImageData == null || profile.ImageContentType == null)
            return NotFound();

        return File(profile.ImageData, profile.ImageContentType);
    }
}
