namespace BackendPIM.Models
{
    public class Servico
    {
        public int Id { get; set; }

        // Ex: "Troca de fiação elétrica", "Reparo de vazamento"
        public string Titulo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        // Preço sugerido ou base para o serviço de manutenção
        public decimal PrecoBase { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
