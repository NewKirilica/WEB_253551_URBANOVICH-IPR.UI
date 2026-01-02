using Microsoft.AspNetCore.Mvc;
using WEB_253551_URBANOVICH.UI.Extensions;
using WEB_253551_URBANOVICH.UI.Models;

namespace WEB_253551_URBANOVICH.UI.Components;

public class CartViewComponent : ViewComponent
{
    private const string CartKey = "CART";

    public IViewComponentResult Invoke()
    {
        var cart = HttpContext.Session.GetJson<Cart>(CartKey) ?? new Cart();

        var model = new CartViewModel
        {
            TotalPrice = cart.TotalPrice,
            ItemsCount = cart.ItemsCount
        };

        return View(model);
    }
}
