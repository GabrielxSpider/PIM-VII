using BackendPIM.Data;
using BackendPIM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BackendPIM.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ParticipacoesTreinamentosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ParticipacoesTreinamentosController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("{conteudoId:int}")]
    public async Task<IActionResult> Concluir(int conteudoId)
    {
        var usuarioIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdString, out var usuarioId))
        {
            return Unauthorized(new
            {
                mensagem = "Usuário não identificado."
            });
        }

        var conteudo = await _context.ConteudosDiversidade
            .FirstOrDefaultAsync(c => c.Id == conteudoId);

        if (conteudo == null)
        {
            return NotFound(new
            {
                mensagem = "Conteúdo não encontrado."
            });
        }

        var participacaoExistente = await _context.ParticipacoesTreinamentos
            .AnyAsync(p =>
                p.ConteudoDiversidadeId == conteudoId &&
                p.UsuarioId == usuarioId);

        if (participacaoExistente)
        {
            return Conflict(new
            {
                mensagem = "Você já concluiu este treinamento."
            });
        }

        var participacao = new ParticipacaoTreinamento
        {
            ConteudoDiversidadeId = conteudoId,
            UsuarioId = usuarioId,
            DataConclusao = DateTime.Now
        };

        _context.ParticipacoesTreinamentos.Add(participacao);

        await _context.SaveChangesAsync();

        return Created("", new
        {
            participacao.Id,
            participacao.UsuarioId,
            participacao.ConteudoDiversidadeId,
            participacao.DataConclusao
        });
    }

    [HttpGet("minhas")]
    public async Task<IActionResult> MinhasParticipacoes()
    {
        var usuarioIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdString, out var usuarioId))
        {
            return Unauthorized(new
            {
                mensagem = "Usuário não identificado."
            });
        }

        var participacoes = await _context.ParticipacoesTreinamentos
            .AsNoTracking()
            .Where(p => p.UsuarioId == usuarioId)
            .Select(p => new
            {
                p.Id,
                p.DataConclusao,
                Conteudo = new
                {
                    p.ConteudoDiversidadeId,
                    Titulo = p.ConteudoDiversidade!.Titulo,
                    Tipo = p.ConteudoDiversidade.Tipo.ToString()
                }
            })
            .OrderByDescending(p => p.DataConclusao)
            .ToListAsync();

        return Ok(participacoes);
    }
}