using BackendPIM.Filters;
using BackendPIM.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendPIM.Controllers;

[AdminAuthorizationFilter]
public class ProfissionaisMvcController : Controller
{
    private readonly ApiService _apiService;

    public ProfissionaisMvcController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var token = HttpContext.Session.GetString("Token");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Index", "Login");
        }

        var profissionais = await _apiService
            .GetProfissionaisAsync(token);

        return View(profissionais);
    }
}