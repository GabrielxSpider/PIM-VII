using BackendPIM.Filters;
using BackendPIM.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendPIM.Controllers;

[ClienteAuthorizationFilter]
public class ClienteController : Controller
{
    private readonly ApiService _apiService;

    public ClienteController(ApiService apiService)
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

    var agendamentos =
        await _apiService.GetAgendamentosClienteAsync(token);

    return View(agendamentos);
}

    public async Task<IActionResult> NovoAgendamento()
    {
        var token = HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Index", "Login");
        }

        ViewBag.Servicos = await _apiService.GetServicosAsync();
        ViewBag.Profissionais = await _apiService.GetProfissionaisAsync(token);

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> NovoAgendamento(
        int ServicoId,
        int ProfissionalId,
        DateTime Data,
        TimeSpan Hora)
    {
        var token = HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Index", "Login");
        }

        var dataHora = Data.Date.Add(Hora);

        var sucesso = await _apiService.CreateAgendamentoAsync(
            ProfissionalId,
            ServicoId,
            dataHora,
            token
        );

        if (!sucesso)
        {
            TempData["Erro"] = "Não foi possível realizar o agendamento.";
            return RedirectToAction("NovoAgendamento");
        }

        TempData["Sucesso"] = "Agendamento realizado com sucesso.";
        return RedirectToAction("Agendamentos");
    }

    [HttpPost]
public async Task<IActionResult> CancelarAgendamento(int id)
{
    var token = HttpContext.Session.GetString("Token");

    if (string.IsNullOrEmpty(token))
    {
        return RedirectToAction("Index", "Login");
    }

    var sucesso = await _apiService.CancelarAgendamentoAsync(
        id,
        token
    );

    if (!sucesso)
    {
        TempData["Erro"] =
            "Não foi possível cancelar o agendamento.";
    }
    else
    {
        TempData["Sucesso"] =
            "Agendamento cancelado com sucesso.";
    }

    return RedirectToAction("Agendamentos");
}

    public async Task<IActionResult> Agendamentos()
    {
        var token = HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Index", "Login");
        }

        var agendamentos = await _apiService.GetAgendamentosClienteAsync(token);
        return View(agendamentos);
    }

    public async Task<IActionResult> Relatos()
    {
        var token = HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Index", "Login");
        }

        var relatos = await _apiService.GetRelatosClienteAsync(token);
        return View(relatos);
    }

    [HttpGet]
    public IActionResult NovoRelato()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> NovoRelato(
        string DescricaoFatos,
        bool Anonimo)
    {
        var token = HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Index", "Login");
        }

        if (string.IsNullOrWhiteSpace(DescricaoFatos))
        {
            ViewBag.Erro = "Descreva os fatos antes de enviar o relato.";
            return View();
        }

        var sucesso = await _apiService.CreateRelatoAsync(
            DescricaoFatos,
            Anonimo,
            token
        );

        if (!sucesso)
        {
            ViewBag.Erro = "Não foi possível enviar o relato.";
            return View();
        }

        TempData["Sucesso"] = "Relato enviado com sucesso.";
        return RedirectToAction("Relatos");
    }
}