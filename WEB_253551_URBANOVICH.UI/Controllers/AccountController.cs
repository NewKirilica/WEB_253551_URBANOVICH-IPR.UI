using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_253551_URBANOVICH.UI.Models;
using WEB_253551_URBANOVICH.UI.Services.Authentication;

namespace WEB_253551_URBANOVICH.UI.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    [AllowAnonymous]
    public async Task Login()
    {
        await HttpContext.ChallengeAsync(
            OpenIdConnectDefaults.AuthenticationScheme,
            new AuthenticationProperties { RedirectUri = Url.Action("Index", "Home") });
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View(new RegisterUserViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Register(
        RegisterUserViewModel user,
        [FromServices] IAuthService authService)
    {
        if (!ModelState.IsValid)
            return View(user);

        if (user is null)
            return BadRequest();

        var result = await authService.RegisterUserAsync(user.Email, user.Password, user.Avatar);

        if (result.Result)
            return Redirect(Url.Action("Index", "Home") ?? "/");

        ModelState.AddModelError(string.Empty, result.ErrorMessage);
        return View(user);
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme,
            new AuthenticationProperties { RedirectUri = Url.Action("Index", "Home") });
    }
}
