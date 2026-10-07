using BackendPIM.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendPIM.Controllers;

public class LoginController : Controller
{
    private readonly ApiService _apiService;

    public LoginController(ApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(
        string Email,
        string Senha)
    {
        var resultado = await _apiService.LoginAsync(
            Email,
            Senha
        );

       if (resultado == null)
{
    ViewBag.Erro = "E-mail ou senha inválidos.";
    return View();
}

HttpContext.Session.SetString("Token", resultado.Token);
HttpContext.Session.SetString("Email", resultado.Email);
HttpContext.Session.SetString("Perfil", resultado.Perfil);

return RedirectToAction("Index", "Home");
    }
}