using System.ComponentModel.DataAnnotations;
using BackendPIM.Models;

namespace BackendPIM.DTOs.Auth;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Senha { get; set; } = string.Empty;

    [Required]
    public PerfilUsuario Perfil { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Telefone { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Especialidade { get; set; }
}