namespace BackendPIM.Models
{
    public class ParticipacaoTreinamento
    {
        public int Id { get; set; }
        public DateTime DataConclusao { get; set; } = DateTime.Now;

        // --- RELACIONAMENTOS ---

        // Qual foi o conteúdo/treinamento assistido?
        public int ConteudoDiversidadeId { get; set; }
        public ConteudoDiversidade? ConteudoDiversidade { get; set; }

        // Quem foi o usuário que participou?
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
