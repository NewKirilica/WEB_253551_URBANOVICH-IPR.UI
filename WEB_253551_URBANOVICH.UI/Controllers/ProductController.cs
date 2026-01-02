using Microsoft.AspNetCore.Mvc;
using WEB_253551_URBANOVICH.Domain.Models;
using WEB_253551_URBANOVICH.Domain.Entities;
using WEB_253551_URBANOVICH.UI.Services.CategoryService;
using WEB_253551_URBANOVICH.UI.Services.ProductService;

namespace WEB_253551_URBANOVICH.UI.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(string? category, int pageNo = 1)
    {
        var categoriesResp = await _categoryService.GetCategoryListAsync();
        var categories = categoriesResp.Data ?? new List<Category>();

        var currentCategoryName = "Все";
        if (!string.IsNullOrWhiteSpace(category))
        {
            var cat = categories.FirstOrDefault(c => c.NormalizedName == category);
            if (cat != null) currentCategoryName = cat.Name;
        }

        ViewData["categories"] = categories;
        ViewData["currentCategory"] = currentCategoryName;
        ViewData["category"] = category;

        var listResp = await _productService.GetProductListAsync(category, pageNo);
        if (!listResp.IsSuccess || listResp.Data == null)
            return View(new ListModel<Dish>());

        return View(listResp.Data);
    }
}
