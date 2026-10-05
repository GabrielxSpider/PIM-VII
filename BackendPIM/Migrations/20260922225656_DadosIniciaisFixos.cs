using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendPIM.Migrations
{
    /// <inheritdoc />
    public partial class DadosIniciaisFixos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ConteudosDiversidade",
                columns: new[] { "Id", "Conteudo", "DataPublicacao", "Descricao", "Tipo", "Titulo" },
                values: new object[] { 1, "Este módulo ensina práticas de combate à discriminação no ambiente de prestação de serviços.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Treinamento corporativo sobre igualdade racial e aplicação da Lei 10.639/2003.", "Treinamento Corporativo", "Cultura Afro-Brasileira no Atendimento" });

            migrationBuilder.InsertData(
                table: "Servicos",
                columns: new[] { "Id", "DataCriacao", "Descricao", "PrecoBase", "Titulo" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Troca e instalação segura de chuveiros residenciais.", 120.00m, "Instalação de Chuveiro Elétrico" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Conserto de encanamentos e vazamentos hidráulicos.", 90.00m, "Reparo de Vazamento em Torneira" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Ativo", "DataCriacao", "Email", "Perfil", "SenhaHash" },
                values: new object[] { 1, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@conectalar.com", 3, "admin123" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConteudosDiversidade",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Servicos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Servicos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
