using BackendPIM.Filters;
using BackendPIM.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendPIM.Controllers;

[AdminAuthorizationFilter]
public class RelatosDiscriminacaoMvcController : Controller
{
    private readonly ApiService _apiService;

    public RelatosDiscriminacaoMvcController(ApiService apiService)
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

        var relatos = await _apiService
            .GetRelatosAdminAsync(token);

        return View(relatos);
    }

    [HttpPost]
public async Task<IActionResult> AtualizarStatus(int id, int status)
{
    var token = HttpContext.Session.GetString("Token");

    if (string.IsNullOrEmpty(token))
    {
        return RedirectToAction("Index", "Login");
    }

    var sucesso = await _apiService.AtualizarStatusRelatoAsync(
        id,
        status,
        token);

    if (!sucesso)
    {
        TempData["Erro"] = "Não foi possível atualizar o status do relato.";
    }

    return RedirectToAction(nameof(Index));
}
}