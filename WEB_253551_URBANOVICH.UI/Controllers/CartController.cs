using Microsoft.AspNetCore.Mvc;
using WEB_253551_URBANOVICH.UI.Extensions;
using WEB_253551_URBANOVICH.UI.Models;
using WEB_253551_URBANOVICH.UI.Services.ProductService;

namespace WEB_253551_URBANOVICH.UI.Controllers;

public class CartController : Controller
{
    private const string CartKey = "CART";
    private readonly IProductService _productService;

    public CartController(IProductService productService)
    {
        _productService = productService;
    }

    public IActionResult Index(string? returnUrl = null)
    {
        ViewData["returnUrl"] = returnUrl;
        var cart = GetCart();
        return View(cart);
    }

    public async Task<IActionResult> Add(int id, string? returnUrl = null)
    {
        var dishResp = await _productService.GetProductByIdAsync(id);
        if (dishResp.IsSuccess && dishResp.Data != null)
        {
            var d = dishResp.Data;
            var cart = GetCart();
            cart.Add(d.Id, d.Name, d.Price, d.Image, 1);
            SaveCart(cart);
        }
        return Redirect(returnUrl ?? Url.Action("Index", "Product")!);
    }

    public IActionResult RemoveOne(int id, string? returnUrl = null)
    {
        var cart = GetCart();
        cart.RemoveOne(id);
        SaveCart(cart);
        return Redirect(returnUrl ?? Url.Action("Index")!);
    }

    public IActionResult RemoveItem(int id, string? returnUrl = null)
    {
        var cart = GetCart();
        cart.RemoveItem(id);
        SaveCart(cart);
        return Redirect(returnUrl ?? Url.Action("Index")!);
    }

    public IActionResult Clear(string? returnUrl = null)
    {
        var cart = GetCart();
        cart.Clear();
        SaveCart(cart);
        return Redirect(returnUrl ?? Url.Action("Index")!);
    }

    private Cart GetCart() =>
        HttpContext.Session.GetJson<Cart>(CartKey) ?? new Cart();

    private void SaveCart(Cart cart) =>
        HttpContext.Session.SetJson(CartKey, cart);
}
