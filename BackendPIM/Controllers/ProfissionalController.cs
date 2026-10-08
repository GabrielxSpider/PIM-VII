using BackendPIM.Filters;
using BackendPIM.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendPIM.Controllers;

[ProfissionalAuthorizationFilter]
public class ProfissionalController : Controller
{
    private readonly ApiService _apiService;

    public ProfissionalController(ApiService apiService)
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

        var agendamentos = await _apiService.GetAgendamentosProfissionalAsync(token);
        return View(agendamentos);
    }

    public async Task<IActionResult> Agendamentos()
    {
        var token = HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Index", "Login");
        }

        var agendamentos = await _apiService.GetAgendamentosProfissionalAsync(token);
        return View(agendamentos);
    }

    [HttpPost]
    public async Task<IActionResult> Aceitar(int id)
    {
        var token = HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Index", "Login");
        }

        var sucesso = await _apiService.AceitarAgendamentoAsync(id, token);

        if (!sucesso)
        {
            TempData["Erro"] = "Não foi possível aceitar o agendamento.";
        }
        else
        {
            TempData["Sucesso"] = "Agendamento aceito com sucesso.";
        }

        return RedirectToAction("Agendamentos");
    }

    [HttpPost]
    public async Task<IActionResult> Recusar(int id)
    {
        var token = HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Index", "Login");
        }

        var sucesso = await _apiService.RecusarAgendamentoAsync(id, token);

        if (!sucesso)
        {
            TempData["Erro"] = "Não foi possível recusar o agendamento.";
        }
        else
        {
            TempData["Sucesso"] = "Agendamento recusado com sucesso.";
        }

        return RedirectToAction("Agendamentos");
    }

    [HttpPost]
    public async Task<IActionResult> Concluir(int id)
    {
        var token = HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Index", "Login");
        }

        var sucesso = await _apiService.ConcluirAgendamentoAsync(id, token);

        if (!sucesso)
        {
            TempData["Erro"] = "Não foi possível concluir o agendamento.";
        }
        else
        {
            TempData["Sucesso"] = "Agendamento concluído com sucesso.";
        }

        return RedirectToAction("Agendamentos");
    }
}