using System.ComponentModel.DataAnnotations;

namespace BackendPIM.Models
{
    public class ConteudoDiversidade
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public TipoConteudo Tipo { get; set; }

        [Required]
        public string Conteudo { get; set; } = string.Empty;
        public DateTime DataPublicacao { get; set; } = DateTime.Now;
    }
}
