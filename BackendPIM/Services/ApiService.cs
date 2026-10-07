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

public async Task<List<ProfissionalDto>> GetProfissionaisAsync(string token)
{
    _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);

    var profissionais = await _httpClient
        .GetFromJsonAsync<List<ProfissionalDto>>("api/Profissionais");

    return profissionais ?? new List<ProfissionalDto>();
}

public async Task<List<RelatoAdminDto>> GetRelatosAdminAsync(string token)
{
    _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);

    var relatos = await _httpClient
        .GetFromJsonAsync<List<RelatoAdminDto>>(
            "api/RelatosDiscriminacao");

    return relatos ?? new List<RelatoAdminDto>();
}

public async Task<bool> AtualizarStatusRelatoAsync(
    int id,
    int status,
    string token)
{
    _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);

    var response = await _httpClient.PutAsJsonAsync(
        $"api/RelatosDiscriminacao/{id}/status",
        new
        {
            Status = status
        });

    return response.IsSuccessStatusCode;
}

public async Task<List<AgendamentoAdminDto>> GetAgendamentosAdminAsync(string token)
{
    _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);

    var agendamentos = await _httpClient
        .GetFromJsonAsync<List<AgendamentoAdminDto>>(
            "api/Agendamentos/admin");

    return agendamentos ?? new List<AgendamentoAdminDto>();
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

public class ProfissionalDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Especialidade { get; set; } = string.Empty;
    public bool Disponivel { get; set; }
}

public class AgendamentoAdminDto
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool AceitoPeloProfissional { get; set; }

    public ClienteAgendamentoDto Cliente { get; set; } = new();
    public ProfissionalAgendamentoDto Profissional { get; set; } = new();
    public ServicoAgendamentoDto Servico { get; set; } = new();
}

public class ClienteAgendamentoDto
{
    public int ClienteId { get; set; }
    public string Nome { get; set; } = string.Empty;
}

public class ProfissionalAgendamentoDto
{
    public int ProfissionalId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Especialidade { get; set; } = string.Empty;
}

public class ServicoAgendamentoDto
{
    public int ServicoId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public decimal PrecoBase { get; set; }
}

public class RelatoAdminDto
{
    public int Id { get; set; }

    public string DescricaoFatos { get; set; } = string.Empty;

    public DateTime DataEnvio { get; set; }

    public string Status { get; set; } = string.Empty;

    public int? UsuarioId { get; set; }
}