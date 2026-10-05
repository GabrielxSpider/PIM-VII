using System.ComponentModel.DataAnnotations;

namespace BackendPIM.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }

        [Range(1, 5)]
        public int Nota { get; set; }

        [StringLength(500)]
        public string Comentario { get; set; } = string.Empty;
        public DateTime Data { get; set; } = DateTime.Now;

        // Relacionamentos
        public int AgendamentoId { get; set; }
        public Agendamento? Agendamento { get; set; }
    }
}
