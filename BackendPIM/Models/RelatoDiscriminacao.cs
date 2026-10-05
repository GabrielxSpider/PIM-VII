using System.ComponentModel.DataAnnotations;

namespace BackendPIM.Models
{
    public class RelatoDiscriminacao
    {
        public int Id { get; set; }

        // MUDANÇA DE SEGURANÇA: Garante que ninguém envie um relato em branco
        [Required(ErrorMessage = "A descrição dos fatos é obrigatória.")]
        [StringLength(2000)]
        public string DescricaoFatos { get; set; } = string.Empty;

        public DateTime DataEnvio { get; set; } = DateTime.Now;

        [Required]
        public StatusRelato Status { get; set; } = StatusRelato.Recebido;

        // Pode ser nulo se o usuário quiser fazer uma denúncia anônima
        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
