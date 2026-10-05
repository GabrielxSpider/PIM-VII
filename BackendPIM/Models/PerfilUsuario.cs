namespace BackendPIM.Models
{
    public enum PerfilUsuario
    {
        Cliente = 1,
        Profissional = 2,
        Administrador = 3
    }

    public enum StatusAgendamento
    {
        Pendente = 1,
        Confirmado = 2,
        EmExecucao = 3,
        Concluido = 4,
        Cancelado = 5
    }

    public enum StatusRelato
    {
        Recebido = 1,
        EmAnalise = 2,
        Resolvido = 3
    }

    public enum TipoConteudo
    {
        Artigo = 1,
        Video = 2,
        TreinamentoCorporativo = 3
    }
}
