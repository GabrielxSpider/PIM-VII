using System.ComponentModel.DataAnnotations;
using BackendPIM.Models;

namespace BackendPIM.DTOs.Relatos;

public class AtualizarStatusRelatoRequest
{
    [Required]
    public StatusRelato Status { get; set; }
}