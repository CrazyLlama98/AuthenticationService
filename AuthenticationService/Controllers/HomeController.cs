using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return File("~/index.html", "text/html");
        }
    }
}