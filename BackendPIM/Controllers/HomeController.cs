using Microsoft.AspNetCore.Mvc;

namespace BackendPIM.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}