using BackendPIM.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendPIM.Controllers;

public class ServicosMvcController : Controller
{
    private readonly ApiService _apiService;

    public ServicosMvcController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var servicos = await _apiService.GetServicosAsync();

        return View(servicos);
    }

    public IActionResult Criar()
    {
        return View();
    }

    [HttpPost]
public async Task<IActionResult> Criar(
    string Titulo,
    string Descricao,
    decimal PrecoBase)
{
    var token = HttpContext.Session.GetString("Token");

    if (string.IsNullOrEmpty(token))
    {
        return RedirectToAction("Index", "Login");
    }

    var servico = await _apiService.CreateServicoAsync(
        Titulo,
        Descricao,
        PrecoBase,
        token
    );

    if (servico == null)
    {
        ViewBag.Erro = "Não foi possível cadastrar o serviço.";
        return View();
    }

    return RedirectToAction(nameof(Index));
}
}