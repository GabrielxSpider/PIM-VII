using BackendPIM.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendPIM.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfissionaisController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProfissionaisController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetTodos()
    {
        var profissionais = await _context.Profissionais
            .AsNoTracking()
            .Select(p => new
            {
                p.Id,
                p.Nome,
                p.Telefone,
                p.Especialidade,
                p.Disponivel
            })
            .ToListAsync();

        return Ok(profissionais);
    }
}