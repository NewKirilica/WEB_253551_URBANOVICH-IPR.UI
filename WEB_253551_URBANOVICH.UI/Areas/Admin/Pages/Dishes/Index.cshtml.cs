using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253551_URBANOVICH.Domain.Entities;
using WEB_253551_URBANOVICH.Domain.Models;
using WEB_253551_URBANOVICH.UI.Services.CategoryService;
using WEB_253551_URBANOVICH.UI.Services.ProductService;

namespace WEB_253551_URBANOVICH.UI.Areas.Admin.Pages.Dishes;

public class IndexModel : PageModel
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ListModel<Dish> Data { get; private set; } = new();
    public List<Category> Categories { get; private set; } = new();
    public string? CurrentCategory { get; private set; }

    public IndexModel(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task OnGetAsync(string? category, int pageNo = 1)
    {
        CurrentCategory = category;

        var catResp = await _categoryService.GetCategoryListAsync();
        Categories = catResp.Data ?? new List<Category>();

        var resp = await _productService.GetProductListAsync(category, pageNo);
        Data = resp.Data ?? new ListModel<Dish>();
    }
}
