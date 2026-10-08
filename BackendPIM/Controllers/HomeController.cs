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
    var agendamentos = await _apiService.GetAgendamentosAdminAsync(token);
    var relatos = await _apiService.GetRelatosAdminAsync(token);
    var conteudos = await _apiService.GetConteudosAsync(token);

    ViewBag.TotalServicos = servicos.Count;
    ViewBag.TotalProfissionais = profissionais.Count;
    ViewBag.TotalAgendamentos = agendamentos.Count;
    ViewBag.TotalRelatos = relatos.Count;
    ViewBag.TotalConteudos = conteudos.Count;

    return View();
}
}