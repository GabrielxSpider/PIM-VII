using Microsoft.AspNetCore.Mvc;

namespace BackendPIM.Controllers;

public class LogoutController : Controller
{
    [HttpPost]
    public IActionResult Index()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Index", "Login");
    }
}