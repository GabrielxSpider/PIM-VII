using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackendPIM.Data;
using BackendPIM.DTOs.Auth;
using BackendPIM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BackendPIM.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<Usuario> _passwordHasher;

    public AuthController(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<Usuario>();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
        {
            return Conflict(new
            {
                mensagem = "E-mail já cadastrado."
            });
        }

        if (request.Perfil == PerfilUsuario.Administrador)
        {
            return BadRequest(new
            {
                mensagem = "Não é permitido criar administrador pelo cadastro público."
            });
        }

        var usuario = new Usuario
        {
            Email = request.Email,
            Perfil = request.Perfil,
            DataCriacao = DateTime.Now,
            Ativo = true
        };

        usuario.SenhaHash = _passwordHasher.HashPassword(
            usuario,
            request.Senha
        );

        _context.Usuarios.Add(usuario);

        await _context.SaveChangesAsync();

        if (request.Perfil == PerfilUsuario.Cliente)
        {
            _context.Clientes.Add(new Cliente
            {
                Nome = request.Nome,
                Telefone = request.Telefone,
                UsuarioId = usuario.Id
            });
        }
        else if (request.Perfil == PerfilUsuario.Profissional)
        {
            if (string.IsNullOrWhiteSpace(request.Especialidade))
            {
                return BadRequest(new
                {
                    mensagem = "Especialidade é obrigatória para profissionais."
                });
            }

            _context.Profissionais.Add(new Profissional
            {
                Nome = request.Nome,
                Telefone = request.Telefone,
                Especialidade = request.Especialidade,
                Disponivel = true,
                UsuarioId = usuario.Id
            });
        }

        await _context.SaveChangesAsync();

        return Created("", new
        {
            usuario.Id,
            usuario.Email,
            Perfil = usuario.Perfil.ToString()
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (usuario == null || !usuario.Ativo)
        {
            return Unauthorized(new
            {
                mensagem = "E-mail ou senha inválidos."
            });
        }

        var resultado = _passwordHasher.VerifyHashedPassword(
            usuario,
            usuario.SenhaHash,
            request.Senha
        );

        if (resultado == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new
            {
                mensagem = "E-mail ou senha inválidos."
            });
        }

        var token = GerarToken(usuario);

        return Ok(new LoginResponse
        {
            Token = token,
            UsuarioId = usuario.Id,
            Email = usuario.Email,
            Perfil = usuario.Perfil.ToString()
        });
    }

    private string GerarToken(Usuario usuario)
    {
        var key = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT Key não configurada.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Email, usuario.Email),
            new(ClaimTypes.Role, usuario.Perfil.ToString())
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256
        );

        var expiration = DateTime.UtcNow.AddMinutes(
            _configuration.GetValue<int>("Jwt:ExpirationMinutes", 120)
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}