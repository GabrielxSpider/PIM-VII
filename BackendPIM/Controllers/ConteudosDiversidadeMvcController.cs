using BackendPIM.Filters; 
using BackendPIM.Services; 
using Microsoft.AspNetCore.Mvc; 
 
namespace BackendPIM.Controllers; 
 
[AdminAuthorizationFilter] 
public class ConteudosDiversidadeMvcController : Controller 
{ 
    private readonly ApiService _apiService; 
 
    public ConteudosDiversidadeMvcController(ApiService apiService) 
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
 
        var conteudos = await _apiService 
            .GetConteudosAsync(token); 
 
        return View(conteudos); 
    } 
 
    [HttpGet] 
    public IActionResult Criar() 
    { 
        return View(); 
    } 

    [HttpGet]
public async Task<IActionResult> Editar(int id)
{
    var token = HttpContext.Session.GetString("Token");

    if (string.IsNullOrEmpty(token))
    {
        return RedirectToAction("Index", "Login");
    }

    var conteudos = await _apiService.GetConteudosAsync(token);

    var conteudo = conteudos.FirstOrDefault(c => c.Id == id);

    if (conteudo == null)
    {
        return NotFound();
    }

    return View(conteudo);
}

[HttpPost]
public async Task<IActionResult> Editar(
    int Id,
    string Titulo,
    string Descricao,
    int Tipo,
    string Conteudo)
{
    var token = HttpContext.Session.GetString("Token");

    if (string.IsNullOrEmpty(token))
    {
        return RedirectToAction("Index", "Login");
    }

    var conteudo = await _apiService.UpdateConteudoAsync(
        Id,
        Titulo,
        Descricao,
        Tipo,
        Conteudo,
        token);

    if (conteudo == null)
    {
        ViewBag.Erro = "Não foi possível atualizar o conteúdo.";

        return View(new BackendPIM.Services.ConteudoAdminDto
        {
            Id = Id,
            Titulo = Titulo,
            Descricao = Descricao,
            Tipo = Tipo switch
            {
                1 => "Artigo",
                2 => "Video",
                3 => "TreinamentoCorporativo",
                _ => ""
            },
            Conteudo = Conteudo
        });
    }

    return RedirectToAction(nameof(Index));
}
 
   [HttpPost] 
public async Task<IActionResult> Criar( 
    string Titulo, 
    string Descricao, 
    int Tipo, 
    string Conteudo) 
{ 
    var token = HttpContext.Session.GetString("Token"); 
 
    if (string.IsNullOrEmpty(token)) 
    { 
        return RedirectToAction("Index", "Login"); 
    } 
 
    var conteudo = await _apiService.CreateConteudoAsync( 
        Titulo, 
        Descricao, 
        Tipo, 
        Conteudo, 
        token); 
 
    if (conteudo == null) 
    { 
        ViewBag.Erro = "Não foi possível cadastrar o conteúdo."; 
        return View(); 
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

    var sucesso = await _apiService.DeleteConteudoAsync(id, token);

    if (!sucesso)
    {
        TempData["Erro"] = "Não foi possível excluir o conteúdo.";
    }

    return RedirectToAction(nameof(Index));
}

}