using System.Security.Claims;
using BackendPIM.Data;
using BackendPIM.DTOs.Avaliacoes;
using BackendPIM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendPIM.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AvaliacoesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AvaliacoesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("{agendamentoId:int}")]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> Criar(
        int agendamentoId,
        CriarAvaliacaoRequest request)
    {
        var usuarioIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdString, out var usuarioId))
        {
            return Unauthorized(new
            {
                mensagem = "Usuário não identificado."
            });
        }

        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

        if (cliente == null)
        {
            return NotFound(new
            {
                mensagem = "Cliente não encontrado."
            });
        }

        var agendamento = await _context.Agendamentos
            .FirstOrDefaultAsync(a =>
                a.Id == agendamentoId &&
                a.ClienteId == cliente.Id);

        if (agendamento == null)
        {
            return NotFound(new
            {
                mensagem = "Agendamento não encontrado."
            });
        }

        if (agendamento.Status != StatusAgendamento.Concluido)
        {
            return BadRequest(new
            {
                mensagem = "Somente agendamentos concluídos podem ser avaliados."
            });
        }

        var avaliacaoExistente = await _context.Avaliacoes
            .AnyAsync(a => a.AgendamentoId == agendamentoId);

        if (avaliacaoExistente)
        {
            return Conflict(new
            {
                mensagem = "Este agendamento já foi avaliado."
            });
        }

        var avaliacao = new Avaliacao
        {
            Nota = request.Nota,
            Comentario = request.Comentario,
            Data = DateTime.Now,
            AgendamentoId = agendamento.Id
        };

        _context.Avaliacoes.Add(avaliacao);

        await _context.SaveChangesAsync();

        return Created("", new
        {
            avaliacao.Id,
            avaliacao.Nota,
            avaliacao.Comentario,
            avaliacao.Data,
            avaliacao.AgendamentoId
        });
    }
}