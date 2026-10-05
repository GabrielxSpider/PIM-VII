using System.Security.Claims;
using BackendPIM.Data;
using BackendPIM.DTOs.Relatos;
using BackendPIM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendPIM.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RelatosDiscriminacaoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RelatosDiscriminacaoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Criar(CriarRelatoRequest request)
    {
        int? usuarioId = null;

        if (!request.Anonimo)
        {
            var usuarioIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(usuarioIdString, out var id))
            {
                return Unauthorized(new
                {
                    mensagem = "Usuário não identificado."
                });
            }

            usuarioId = id;
        }

        var relato = new RelatoDiscriminacao
        {
            DescricaoFatos = request.DescricaoFatos,
            DataEnvio = DateTime.Now,
            Status = StatusRelato.Recebido,
            UsuarioId = usuarioId
        };

        _context.RelatosDiscriminacao.Add(relato);

        await _context.SaveChangesAsync();

        return Created("", new
        {
            relato.Id,
            relato.DescricaoFatos,
            relato.DataEnvio,
            Status = relato.Status.ToString(),
            relato.UsuarioId
        });
    }

    [HttpGet("meus")]
    public async Task<IActionResult> MeusRelatos()
    {
        var usuarioIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdString, out var usuarioId))
        {
            return Unauthorized(new
            {
                mensagem = "Usuário não identificado."
            });
        }

        var relatos = await _context.RelatosDiscriminacao
            .AsNoTracking()
            .Where(r => r.UsuarioId == usuarioId)
            .Select(r => new
            {
                r.Id,
                r.DescricaoFatos,
                r.DataEnvio,
                Status = r.Status.ToString()
            })
            .OrderByDescending(r => r.DataEnvio)
            .ToListAsync();

        return Ok(relatos);
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Todos()
    {
        var relatos = await _context.RelatosDiscriminacao
            .AsNoTracking()
            .Select(r => new
            {
                r.Id,
                r.DescricaoFatos,
                r.DataEnvio,
                Status = r.Status.ToString(),
                r.UsuarioId
            })
            .OrderByDescending(r => r.DataEnvio)
            .ToListAsync();

        return Ok(relatos);
    }

    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AtualizarStatus(
    int id,
    AtualizarStatusRelatoRequest request)
    {
        if (!Enum.IsDefined(typeof(StatusRelato), request.Status))
        {
            return BadRequest(new
            {
                mensagem = "Status inválido."
            });
        }

        var relato = await _context.RelatosDiscriminacao
            .FirstOrDefaultAsync(r => r.Id == id);

        if (relato == null)
        {
            return NotFound(new
            {
                mensagem = "Relato não encontrado."
            });
        }

        relato.Status = request.Status;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            mensagem = "Status do relato atualizado com sucesso.",
            relato.Id,
            Status = relato.Status.ToString()
        });
    }
}