using BackendPIM.Data;
using BackendPIM.DTOs.ConteudosDiversidade;
using BackendPIM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendPIM.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConteudosDiversidadeController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ConteudosDiversidadeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetTodos()
    {
        var conteudos = await _context.ConteudosDiversidade
            .AsNoTracking()
            .Select(c => new
            {
                c.Id,
                c.Titulo,
                c.Descricao,
                Tipo = c.Tipo.ToString(),
                c.Conteudo,
                c.DataPublicacao
            })
            .OrderByDescending(c => c.DataPublicacao)
            .ToListAsync();

        return Ok(conteudos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPorId(int id)
    {
        var conteudo = await _context.ConteudosDiversidade
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new
            {
                c.Id,
                c.Titulo,
                c.Descricao,
                Tipo = c.Tipo.ToString(),
                c.Conteudo,
                c.DataPublicacao
            })
            .FirstOrDefaultAsync();

        if (conteudo == null)
        {
            return NotFound(new
            {
                mensagem = "Conteúdo não encontrado."
            });
        }

        return Ok(conteudo);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<IActionResult> Criar(CriarConteudoDiversidadeRequest request)
    {
        var conteudo = new ConteudoDiversidade
        {
            Titulo = request.Titulo,
            Descricao = request.Descricao,
            Tipo = request.Tipo,
            Conteudo = request.Conteudo,
            DataPublicacao = DateTime.Now
        };

        _context.ConteudosDiversidade.Add(conteudo);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetPorId),
            new { id = conteudo.Id },
            conteudo
        );
    }

    [Authorize(Roles = "Administrador")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(
        int id,
        CriarConteudoDiversidadeRequest request)
    {
        var conteudo = await _context.ConteudosDiversidade
            .FirstOrDefaultAsync(c => c.Id == id);

        if (conteudo == null)
        {
            return NotFound(new
            {
                mensagem = "Conteúdo não encontrado."
            });
        }

        conteudo.Titulo = request.Titulo;
        conteudo.Descricao = request.Descricao;
        conteudo.Tipo = request.Tipo;
        conteudo.Conteudo = request.Conteudo;

        await _context.SaveChangesAsync();

        return Ok(conteudo);
    }

    [Authorize(Roles = "Administrador")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var conteudo = await _context.ConteudosDiversidade
            .FirstOrDefaultAsync(c => c.Id == id);

        if (conteudo == null)
        {
            return NotFound(new
            {
                mensagem = "Conteúdo não encontrado."
            });
        }

        _context.ConteudosDiversidade.Remove(conteudo);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}