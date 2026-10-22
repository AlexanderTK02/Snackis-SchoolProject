using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Snackis.Models;
using Snackis.Services;

public class PostsController : Controller
{
    private readonly IPostService _postService;
    private readonly ICategoryService _categoryService;
    private readonly ICommentService _commentService;
    private readonly UserManager<IdentityUser> _userManager;

    public PostsController(
        IPostService postService,
        ICategoryService categoryService,
        ICommentService commentService,
        UserManager<IdentityUser> userManager)
    {
        _postService = postService;
        _categoryService = categoryService;
        _commentService = commentService;
        _userManager = userManager;
    }

    // GET: Posts
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        return View(await _postService.GetAllAsync());
    }

    // GET: Posts/Details/5
    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var post = await _postService.GetAsync(id.Value);
        if (post == null) return NotFound();

        ViewBag.Comments = await _commentService.GetCommentsForPostAsync(post.Id);
        return View(post);
    }

    // GET: Posts/Create
    [Authorize]
    public async Task<IActionResult> Create()
    {
        await LoadCategoriesAsync();
        return View();
    }

    // POST: Posts/Create
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Content,CategoryId")] Post post)
    {
        ModelState.Remove(nameof(Post.UserId));
        ModelState.Remove(nameof(Post.User));
        ModelState.Remove(nameof(Post.Category));

        if (ModelState.IsValid)
        {
            await _postService.CreateAsync(post, _userManager.GetUserId(User)!);
            return RedirectToAction(nameof(Index));
        }

        await LoadCategoriesAsync(post.CategoryId);
        return View(post);
    }

    // GET: Posts/Edit/5
    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var post = await _postService.GetAsync(id.Value);
        if (post == null) return NotFound();
        if (!_postService.CanEdit(post, _userManager.GetUserId(User)!)) return Forbid();

        await LoadCategoriesAsync(post.CategoryId);
        return View(post);
    }

    // POST: Posts/Edit/5
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,CategoryId")] Post post)
    {
        if (id != post.Id) return NotFound();

        ModelState.Remove(nameof(Post.UserId));
        ModelState.Remove(nameof(Post.User));
        ModelState.Remove(nameof(Post.Category));

        if (ModelState.IsValid)
        {
            var result = await _postService.UpdateAsync(
                id, post.Title, post.Content, post.CategoryId, _userManager.GetUserId(User)!);

            return result switch
            {
                OperationResult.NotFound => NotFound(),
                OperationResult.Forbidden => Forbid(),
                _ => RedirectToAction(nameof(Index))
            };
        }

        await LoadCategoriesAsync(post.CategoryId);
        return View(post);
    }

    // GET: Posts/Delete/5
    [Authorize]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var post = await _postService.GetAsync(id.Value);
        if (post == null) return NotFound();
        if (!_postService.CanDelete(post, _userManager.GetUserId(User)!, User.IsInRole("Admin")))
            return Forbid();

        return View(post);
    }

    // POST: Posts/Delete/5
    [Authorize]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _postService.DeleteAsync(
            id, _userManager.GetUserId(User)!, User.IsInRole("Admin"));

        if (result == OperationResult.Forbidden) return Forbid();

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadCategoriesAsync(int? selected = null)
    {
        var categories = await _categoryService.GetAllAsync();
        ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", selected);
    }
}