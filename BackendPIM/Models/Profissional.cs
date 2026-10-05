using System.ComponentModel.DataAnnotations;

namespace BackendPIM.Models
{
    public class Profissional
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Especialidade { get; set; } = string.Empty;

        public bool Disponivel { get; set; } = true;

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
