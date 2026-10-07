using BackendPIM.Filters;
using BackendPIM.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendPIM.Controllers;

[AdminAuthorizationFilter]
public class HomeController : Controller
{
    private readonly ApiService _apiService;

    public HomeController(ApiService apiService)
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

    var servicos = await _apiService.GetServicosAsync();
    var profissionais = await _apiService.GetProfissionaisAsync(token);


    ViewBag.TotalServicos = servicos.Count;
    ViewBag.TotalProfissionais = profissionais.Count;

    return View();
}
}