using BackendPIM.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendPIM.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Tabelas do Banco de Dados
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Profissional> Profissionais { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<ConteudoDiversidade> ConteudosDiversidade { get; set; }
        public DbSet<ParticipacaoTreinamento> ParticipacoesTreinamentos { get; set; }
        public DbSet<RelatoDiscriminacao> RelatosDiscriminacao { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações de Índices Únicos
            modelBuilder.Entity<Cliente>().HasIndex(c => c.UsuarioId).IsUnique();
            modelBuilder.Entity<Profissional>().HasIndex(p => p.UsuarioId).IsUnique();
            modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();

            // Proteção contra Cascade Delete nos Agendamentos
            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Cliente)
                .WithMany()
                .HasForeignKey(a => a.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Profissional)
                .WithMany()
                .HasForeignKey(a => a.ProfissionalId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---- DATA SEEDING TOTALMENTE ESTÁTICO ----
            var dataFixa = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Usuário Administrador com um Hash de senha REAL e FIXO (Equivalente a 'admin123')
      modelBuilder.Entity<Usuario>().HasData(
    new Usuario
    {
        Id = 1,
        Email = "admin@conectalar.com",
        SenhaHash = "AQAAAAIAAYagAAAAEDtwbNr98RTUpfrjz1whQxyKFH7RF4WDx4PjZPYzjY5+62MQ9AHNuR8WabZw/zygzw==",
        Perfil = PerfilUsuario.Administrador,
        DataCriacao = dataFixa,
        Ativo = true
    }
);
            // Serviços Iniciais
            modelBuilder.Entity<Servico>().HasData(
                new Servico { Id = 1, Titulo = "Instalação de Chuveiro Elétrico", Descricao = "Troca e instalação segura de chuveiros residenciais.", PrecoBase = 120.00m, DataCriacao = dataFixa },
                new Servico { Id = 2, Titulo = "Reparo de Vazamento em Torneira", Descricao = "Conserto de encanamentos e vazamentos hidráulicos.", PrecoBase = 90.00m, DataCriacao = dataFixa }
            );

            // Conteúdo de Responsabilidade Social
            modelBuilder.Entity<ConteudoDiversidade>().HasData(
                new ConteudoDiversidade
                {
                    Id = 1,
                    Titulo = "Cultura Afro-Brasileira no Atendimento",
                    Descricao = "Treinamento corporativo sobre igualdade racial e aplicação da Lei 10.639/2003.",
                    Tipo = TipoConteudo.TreinamentoCorporativo,
                    Conteudo = "Este módulo ensina práticas de combate à discriminação no ambiente de prestação de serviços.",
                    DataPublicacao = dataFixa
                }
            );
        }
    }
}
