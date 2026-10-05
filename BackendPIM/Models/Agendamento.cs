using System.ComponentModel.DataAnnotations;

namespace BackendPIM.Models
{
    public class Agendamento
    {
        public int Id { get; set; }

        [Required]
        public DateTime DataHora { get; set; }

        [Required]
        public StatusAgendamento Status { get; set; } = StatusAgendamento.Pendente;

        
        public bool AceitoPeloProfissional { get; set; } = false;

        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public int ProfissionalId { get; set; }
        public Profissional? Profissional { get; set; }

        public int ServicoId { get; set; }
        public Servico? Servico { get; set; }
    }
}
