using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253551_URBANOVICH.Domain.Entities;
using WEB_253551_URBANOVICH.UI.Services.ProductService;

namespace WEB_253551_URBANOVICH.UI.Areas.Admin.Pages.Dishes;

public class DetailsModel : PageModel
{
    private readonly IProductService _productService;

    public Dish Dish { get; private set; } = new();

    public DetailsModel(IProductService productService) => _productService = productService;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var resp = await _productService.GetProductByIdAsync(id);
        if (!resp.IsSuccess || resp.Data == null)
            return RedirectToPage("Index");

        Dish = resp.Data;
        return Page();
    }
}
