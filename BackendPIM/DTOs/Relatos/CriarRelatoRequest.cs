using System.ComponentModel.DataAnnotations;

namespace BackendPIM.DTOs.Relatos;

public class CriarRelatoRequest
{
    [Required]
    [StringLength(2000)]
    public string DescricaoFatos { get; set; } = string.Empty;

    public bool Anonimo { get; set; } = false;
}