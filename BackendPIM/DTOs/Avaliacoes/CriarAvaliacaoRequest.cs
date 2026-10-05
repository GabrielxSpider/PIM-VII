using System.ComponentModel.DataAnnotations;

namespace BackendPIM.DTOs.Avaliacoes;

public class CriarAvaliacaoRequest
{
    [Range(1, 5)]
    public int Nota { get; set; }

    [StringLength(500)]
    public string Comentario { get; set; } = string.Empty;
}