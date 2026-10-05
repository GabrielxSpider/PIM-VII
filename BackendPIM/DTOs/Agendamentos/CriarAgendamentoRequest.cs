using System.ComponentModel.DataAnnotations;

namespace BackendPIM.DTOs.Agendamentos;

public class CriarAgendamentoRequest
{
    [Required]
    public DateTime DataHora { get; set; }

    [Required]
    public int ProfissionalId { get; set; }

    [Required]
    public int ServicoId { get; set; }
}