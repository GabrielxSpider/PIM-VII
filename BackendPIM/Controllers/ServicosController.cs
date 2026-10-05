using BackendPIM.Data;
using BackendPIM.DTOs.Servicos;
using BackendPIM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendPIM.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ServicosController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetTodos()
    {
        var servicos = await _context.Servicos
            .AsNoTracking()
            .Select(s => new
            {
                s.Id,
                s.Titulo,
                s.Descricao,
                s.PrecoBase,
                s.DataCriacao
            })
            .ToListAsync();

        return Ok(servicos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPorId(int id)
    {
        var servico = await _context.Servicos
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new
            {
                s.Id,
                s.Titulo,
                s.Descricao,
                s.PrecoBase,
                s.DataCriacao
            })
            .FirstOrDefaultAsync();

        if (servico == null)
        {
            return NotFound(new
            {
                mensagem = "Serviço não encontrado."
            });
        }

        return Ok(servico);
    }
    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<IActionResult> Criar(CriarServicoRequest request)
    {
        var servico = new Servico
        {
            Titulo = request.Titulo,
            Descricao = request.Descricao,
            PrecoBase = request.PrecoBase,
            DataCriacao = DateTime.Now
        };

        _context.Servicos.Add(servico);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetPorId),
            new { id = servico.Id },
            servico
        );
    }
    [Authorize(Roles = "Administrador")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(
        int id,
        CriarServicoRequest request)
    {
        var servico = await _context.Servicos
            .FirstOrDefaultAsync(s => s.Id == id);

        if (servico == null)
        {
            return NotFound(new
            {
                mensagem = "Serviço não encontrado."
            });
        }

        servico.Titulo = request.Titulo;
        servico.Descricao = request.Descricao;
        servico.PrecoBase = request.PrecoBase;

        await _context.SaveChangesAsync();

        return Ok(servico);
    }
    [Authorize(Roles = "Administrador")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var servico = await _context.Servicos
            .FirstOrDefaultAsync(s => s.Id == id);

        if (servico == null)
        {
            return NotFound(new
            {
                mensagem = "Serviço não encontrado."
            });
        }

        _context.Servicos.Remove(servico);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
