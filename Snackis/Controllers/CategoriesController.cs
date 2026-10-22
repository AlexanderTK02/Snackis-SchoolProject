using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Snackis.Models;
using Snackis.Services;

[Authorize(Roles = "Admin")]
public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: Categories
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        return View(await _categoryService.GetAllAsync());
    }

    // GET: Categories/Create
    public async Task<IActionResult> Create()
    {
        await LoadParentsAsync();
        return View();
    }

    // POST: Categories/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,ParentCategoryId")] Category category)
    {
        ModelState.Remove(nameof(Category.ParentCategory));

        if (ModelState.IsValid)
        {
            await _categoryService.CreateAsync(category);
            return RedirectToAction(nameof(Index));
        }

        await LoadParentsAsync(null, category.ParentCategoryId);
        return View(category);
    }

    // GET: Categories/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var category = await _categoryService.GetAsync(id.Value);
        if (category == null) return NotFound();

        await LoadParentsAsync(id, category.ParentCategoryId);
        return View(category);
    }

    // POST: Categories/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ParentCategoryId")] Category category)
    {
        if (id != category.Id) return NotFound();

        ModelState.Remove(nameof(Category.ParentCategory));

        if (ModelState.IsValid)
        {
            var error = await _categoryService.UpdateAsync(id, category.Name, category.ParentCategoryId);
            if (error == null) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("ParentCategoryId", error);
        }

        await LoadParentsAsync(id, category.ParentCategoryId);
        return View(category);
    }

    // GET: Categories/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var category = await _categoryService.GetAsync(id.Value);
        if (category == null) return NotFound();

        return View(category);
    }

    // POST: Categories/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var error = await _categoryService.DeleteAsync(id);
        if (error == null) return RedirectToAction(nameof(Index));

        ModelState.AddModelError("", error);
        var category = await _categoryService.GetAsync(id);
        if (category == null) return RedirectToAction(nameof(Index));

        return View(category);
    }

    private async Task LoadParentsAsync(int? excludeId = null, int? selected = null)
    {
        var parents = await _categoryService.GetPossibleParentsAsync(excludeId);
        ViewData["ParentCategoryId"] = new SelectList(parents, "Id", "Name", selected);
    }
}