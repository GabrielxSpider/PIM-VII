using BackendPIM.Filters;
using BackendPIM.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendPIM.Controllers;

[AdminAuthorizationFilter]
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

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var servicos = await _apiService.GetServicosAsync();

        var servico = servicos.FirstOrDefault(s => s.Id == id);

        if (servico == null)
        {
            return NotFound();
        }

        return View(servico);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(
        int Id,
        string Titulo,
        string Descricao,
        decimal PrecoBase)
    {
        var token = HttpContext.Session.GetString("Token");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Index", "Login");
        }

        var servico = await _apiService.UpdateServicoAsync(
            Id,
            Titulo,
            Descricao,
            PrecoBase,
            token
        );

        if (servico == null)
        {
            ViewBag.Erro = "Não foi possível atualizar o serviço.";

            return View(new ServicoDto
            {
                Id = Id,
                Titulo = Titulo,
                Descricao = Descricao,
                PrecoBase = PrecoBase
            });
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
public async Task<IActionResult> Excluir(int id)
{
    var token = HttpContext.Session.GetString("Token");

    if (string.IsNullOrEmpty(token))
    {
        return RedirectToAction("Index", "Login");
    }

    var sucesso = await _apiService.DeleteServicoAsync(
        id,
        token
    );

    if (!sucesso)
    {
        TempData["Erro"] = "Não foi possível excluir o serviço.";
    }

    return RedirectToAction(nameof(Index));
}
}