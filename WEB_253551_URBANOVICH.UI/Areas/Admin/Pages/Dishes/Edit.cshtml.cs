using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253551_URBANOVICH.Domain.Entities;
using WEB_253551_URBANOVICH.UI.Services.CategoryService;
using WEB_253551_URBANOVICH.UI.Services.ProductService;

namespace WEB_253551_URBANOVICH.UI.Areas.Admin.Pages.Dishes;

public class EditModel : PageModel
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public List<Category> Categories { get; private set; } = new();

    [BindProperty]
    public Dish Dish { get; set; } = new();

    [BindProperty]
    public IFormFile? Image { get; set; }

    public EditModel(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Categories = (await _categoryService.GetCategoryListAsync()).Data ?? new List<Category>();

        var resp = await _productService.GetProductByIdAsync(id);
        if (!resp.IsSuccess || resp.Data == null)
            return RedirectToPage("Index");

        Dish = resp.Data;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        Categories = (await _categoryService.GetCategoryListAsync()).Data ?? new List<Category>();

        if (!ModelState.IsValid)
            return Page();

        await _productService.UpdateProductAsync(id, Dish, Image);
        return RedirectToPage("Index");
    }
}
