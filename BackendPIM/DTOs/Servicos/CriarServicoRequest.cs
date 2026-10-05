using System.ComponentModel.DataAnnotations;

namespace BackendPIM.DTOs.Servicos;

public class CriarServicoRequest
{
    [Required]
    [StringLength(150)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    public string Descricao { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal PrecoBase { get; set; }
}