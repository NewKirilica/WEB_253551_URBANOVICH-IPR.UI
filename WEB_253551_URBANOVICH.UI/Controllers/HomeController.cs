using Microsoft.AspNetCore.Mvc;

namespace WEB_253551_URBANOVICH.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
