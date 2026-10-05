using System.ComponentModel.DataAnnotations;

namespace BackendPIM.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)] // Senhas criptografadas precisam de mais espaço
        public string SenhaHash { get; set; } = string.Empty;

        [Required]
        public PerfilUsuario Perfil { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public bool Ativo { get; set; } = true;
    }
}
