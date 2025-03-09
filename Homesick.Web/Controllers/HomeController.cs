using Microsoft.AspNetCore.Mvc;

namespace Homesick.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
