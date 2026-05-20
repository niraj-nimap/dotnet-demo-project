using Microsoft.AspNetCore.Mvc;

namespace dotnet_demo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Message = "Hello from Jenkins CI/CD Pipeline!";
            return View();
        }
    }
}