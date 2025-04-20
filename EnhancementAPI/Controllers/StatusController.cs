using Microsoft.AspNetCore.Mvc;

namespace EnhancementAPI.Controllers
{
    public class StatusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
