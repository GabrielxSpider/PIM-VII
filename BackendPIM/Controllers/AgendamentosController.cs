using System.Security.Claims;
using BackendPIM.Data;
using BackendPIM.DTOs.Agendamentos;
using BackendPIM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendPIM.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AgendamentosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AgendamentosController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> Criar(CriarAgendamentoRequest request)
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

        var profissional = await _context.Profissionais
            .FirstOrDefaultAsync(p => p.Id == request.ProfissionalId);

        if (profissional == null)
        {
            return NotFound(new
            {
                mensagem = "Profissional não encontrado."
            });
        }

        if (!profissional.Disponivel)
        {
            return BadRequest(new
            {
                mensagem = "O profissional não está disponível."
            });
        }

        var servico = await _context.Servicos
            .FirstOrDefaultAsync(s => s.Id == request.ServicoId);

        if (servico == null)
        {
            return NotFound(new
            {
                mensagem = "Serviço não encontrado."
            });
        }

        var agendamento = new Agendamento
        {
            DataHora = request.DataHora,
            Status = StatusAgendamento.Pendente,
            AceitoPeloProfissional = false,
            ClienteId = cliente.Id,
            ProfissionalId = profissional.Id,
            ServicoId = servico.Id
        };

        _context.Agendamentos.Add(agendamento);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetPorId),
            new { id = agendamento.Id },
            agendamento
        );
    }
    [HttpGet]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> GetMeusAgendamentos()
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
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

        if (cliente == null)
        {
            return NotFound(new
            {
                mensagem = "Cliente não encontrado."
            });
        }

        var agendamentos = await _context.Agendamentos
            .AsNoTracking()
            .Where(a => a.ClienteId == cliente.Id)
            .Select(a => new
            {
                a.Id,
                a.DataHora,
                Status = a.Status.ToString(),
                a.AceitoPeloProfissional,

                Profissional = new
                {
                    a.ProfissionalId,
                    Nome = a.Profissional!.Nome,
                    Especialidade = a.Profissional.Especialidade
                },

                Servico = new
                {
                    a.ServicoId,
                    Titulo = a.Servico!.Titulo,
                    PrecoBase = a.Servico.PrecoBase
                }
            })
            .OrderBy(a => a.DataHora)
            .ToListAsync();

        return Ok(agendamentos);
    }

        [HttpGet("admin")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GetTodosAdmin()
    {
        var agendamentos = await _context.Agendamentos
            .AsNoTracking()
            .Select(a => new
            {
                a.Id,
                a.DataHora,
                Status = a.Status.ToString(),
                a.AceitoPeloProfissional,

                Cliente = new
                {
                    a.ClienteId,
                    Nome = a.Cliente!.Nome
                },

                Profissional = new
                {
                    a.ProfissionalId,
                    Nome = a.Profissional!.Nome,
                    Especialidade = a.Profissional.Especialidade
                },

                Servico = new
                {
                    a.ServicoId,
                    Titulo = a.Servico!.Titulo,
                    PrecoBase = a.Servico.PrecoBase
                }
            })
            .OrderBy(a => a.DataHora)
            .ToListAsync();

        return Ok(agendamentos);
    }

    [HttpGet("{id:int}")]

    public async Task<IActionResult> GetPorId(int id)
    {
        var usuarioIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdString, out var usuarioId))
        {
            return Unauthorized(new
            {
                mensagem = "Usuário não identificado."
            });
        }

        var agendamento = await _context.Agendamentos
            .Include(a => a.Cliente)
            .Include(a => a.Profissional)
            .Include(a => a.Servico)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (agendamento == null)
        {
            return NotFound(new
            {
                mensagem = "Agendamento não encontrado."
            });
        }

        var usuario = await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == usuarioId);

        if (usuario == null)
        {
            return Unauthorized(new
            {
                mensagem = "Usuário não encontrado."
            });
        }

        if (usuario.Perfil == PerfilUsuario.Cliente &&
            agendamento.Cliente?.UsuarioId != usuarioId)
        {
            return Forbid();
        }

        if (usuario.Perfil == PerfilUsuario.Profissional &&
            agendamento.Profissional?.UsuarioId != usuarioId)
        {
            return Forbid();
        }

        return Ok(new
        {
            agendamento.Id,
            agendamento.DataHora,
            Status = agendamento.Status.ToString(),
            agendamento.AceitoPeloProfissional,

            Cliente = new
            {
                agendamento.ClienteId,
                Nome = agendamento.Cliente?.Nome
            },

            Profissional = new
            {
                agendamento.ProfissionalId,
                Nome = agendamento.Profissional?.Nome,
                Especialidade = agendamento.Profissional?.Especialidade
            },

            Servico = new
            {
                agendamento.ServicoId,
                Titulo = agendamento.Servico?.Titulo,
                PrecoBase = agendamento.Servico?.PrecoBase
            }
        });
    }
    [HttpGet("profissional")]
    [Authorize(Roles = "Profissional")]
    public async Task<IActionResult> GetMeusAgendamentosProfissional()
    {
        var usuarioIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdString, out var usuarioId))
        {
            return Unauthorized(new
            {
                mensagem = "Usuário não identificado."
            });
        }

        var profissional = await _context.Profissionais
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);

        if (profissional == null)
        {
            return NotFound(new
            {
                mensagem = "Profissional não encontrado."
            });
        }

        var agendamentos = await _context.Agendamentos
            .AsNoTracking()
            .Where(a => a.ProfissionalId == profissional.Id)
            .Select(a => new
            {
                a.Id,
                a.DataHora,
                Status = a.Status.ToString(),
                a.AceitoPeloProfissional,

                Cliente = new
                {
                    a.ClienteId,
                    Nome = a.Cliente!.Nome,
                    Telefone = a.Cliente.Telefone
                },

                Servico = new
                {
                    a.ServicoId,
                    Titulo = a.Servico!.Titulo,
                    PrecoBase = a.Servico.PrecoBase
                }
            })
            .OrderBy(a => a.DataHora)
            .ToListAsync();

        return Ok(agendamentos);
    }
    [HttpPut("{id:int}/aceitar")]
    [Authorize(Roles = "Profissional")]
    public async Task<IActionResult> Aceitar(int id)
    {
        var usuarioIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdString, out var usuarioId))
        {
            return Unauthorized(new
            {
                mensagem = "Usuário não identificado."
            });
        }

        var profissional = await _context.Profissionais
            .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);

        if (profissional == null)
        {
            return NotFound(new
            {
                mensagem = "Profissional não encontrado."
            });
        }

        var agendamento = await _context.Agendamentos
            .FirstOrDefaultAsync(a =>
                a.Id == id &&
                a.ProfissionalId == profissional.Id);

        if (agendamento == null)
        {
            return NotFound(new
            {
                mensagem = "Agendamento não encontrado."
            });
        }

        if (agendamento.Status != StatusAgendamento.Pendente)
        {
            return BadRequest(new
            {
                mensagem = "Este agendamento não está pendente."
            });
        }

        agendamento.Status = StatusAgendamento.Confirmado;
        agendamento.AceitoPeloProfissional = true;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            mensagem = "Agendamento aceito com sucesso.",
            agendamento.Id,
            Status = agendamento.Status.ToString(),
            agendamento.AceitoPeloProfissional
        });
    }
    [HttpPut("{id:int}/concluir")]
    [Authorize(Roles = "Profissional")]
    public async Task<IActionResult> Concluir(int id)
    {
        var usuarioIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdString, out var usuarioId))
        {
            return Unauthorized(new
            {
                mensagem = "Usuário não identificado."
            });
        }

        var profissional = await _context.Profissionais
            .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);

        if (profissional == null)
        {
            return NotFound(new
            {
                mensagem = "Profissional não encontrado."
            });
        }

        var agendamento = await _context.Agendamentos
            .FirstOrDefaultAsync(a =>
                a.Id == id &&
                a.ProfissionalId == profissional.Id);

        if (agendamento == null)
        {
            return NotFound(new
            {
                mensagem = "Agendamento não encontrado."
            });
        }

        if (agendamento.Status != StatusAgendamento.Confirmado)
        {
            return BadRequest(new
            {
                mensagem = "Somente agendamentos confirmados podem ser concluídos."
            });
        }

        agendamento.Status = StatusAgendamento.Concluido;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            mensagem = "Agendamento concluído com sucesso.",
            agendamento.Id,
            Status = agendamento.Status.ToString(),
            agendamento.AceitoPeloProfissional
        });
    }
}