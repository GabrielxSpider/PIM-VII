using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BackendPIM.Services;
public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ServicoDto>> GetServicosAsync()
    {
        var servicos = await _httpClient
            .GetFromJsonAsync<List<ServicoDto>>("api/Servicos");

        return servicos ?? new List<ServicoDto>();
    }

    public async Task<ServicoDto?> CreateServicoAsync(
    string titulo,
    string descricao,
    decimal precoBase,
    string token)
{
    _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);

    var response = await _httpClient.PostAsJsonAsync(
        "api/Servicos",
        new
        {
            Titulo = titulo,
            Descricao = descricao,
            PrecoBase = precoBase
        });

    if (!response.IsSuccessStatusCode)
    {
        return null;
    }

    return await response.Content.ReadFromJsonAsync<ServicoDto>();
}

public async Task<ServicoDto?> UpdateServicoAsync(
    int id,
    string titulo,
    string descricao,
    decimal precoBase,
    string token)
{
    _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);

    var response = await _httpClient.PutAsJsonAsync(
        $"api/Servicos/{id}",
        new
        {
            Titulo = titulo,
            Descricao = descricao,
            PrecoBase = precoBase
        });

    if (!response.IsSuccessStatusCode)
    {
        return null;
    }

    return await response.Content
        .ReadFromJsonAsync<ServicoDto>();
}

public async Task<bool> DeleteServicoAsync(
    int id,
    string token)
{
    _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);

    var response = await _httpClient.DeleteAsync(
        $"api/Servicos/{id}");

    return response.IsSuccessStatusCode;
}

    public async Task<LoginResponseDto?> LoginAsync(
    string email,
    string senha)
{
    var response = await _httpClient.PostAsJsonAsync(
        "api/Auth/login",
        new
        {
            Email = email,
            Senha = senha
        });

    if (!response.IsSuccessStatusCode)
    {
        return null;
    }

    return await response.Content
        .ReadFromJsonAsync<LoginResponseDto>();
}
}

public class ServicoDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal PrecoBase { get; set; }
    public DateTime DataCriacao { get; set; }
}

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Perfil { get; set; } = string.Empty;
}