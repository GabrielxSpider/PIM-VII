namespace BackendPIM.DTOs.Auth;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Perfil { get; set; } = string.Empty;
}