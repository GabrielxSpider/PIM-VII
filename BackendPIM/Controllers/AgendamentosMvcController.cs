using BackendPIM.Filters;
using BackendPIM.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendPIM.Controllers;

[AdminAuthorizationFilter]
public class AgendamentosMvcController : Controller
{
    private readonly ApiService _apiService;

    public AgendamentosMvcController(ApiService apiService)
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

        var agendamentos = await _apiService
            .GetAgendamentosAdminAsync(token);

        return View(agendamentos);
    }

    [HttpGet]
    public async Task<IActionResult> Detalhes(int id)
    {
        var token = HttpContext.Session.GetString("Token");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Index", "Login");
        }

        var agendamentos = await _apiService
            .GetAgendamentosAdminAsync(token);

        var agendamento = agendamentos
            .FirstOrDefault(a => a.Id == id);

        if (agendamento == null)
        {
            return NotFound();
        }

        return View(agendamento);
    }
}