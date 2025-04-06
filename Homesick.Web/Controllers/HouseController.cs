using Microsoft.AspNetCore.Mvc;

namespace Homesick.Web.Controllers
{
    public class HouseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
