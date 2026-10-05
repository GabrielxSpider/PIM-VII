using System.ComponentModel.DataAnnotations;
using BackendPIM.Models;

namespace BackendPIM.DTOs.ConteudosDiversidade;

public class CriarConteudoDiversidadeRequest
{
    [Required]
    [StringLength(150)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    public TipoConteudo Tipo { get; set; }

    [Required]
    public string Conteudo { get; set; } = string.Empty;
}