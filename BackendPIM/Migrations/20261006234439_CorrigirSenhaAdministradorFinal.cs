using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendPIM.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirSenhaAdministradorFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "SenhaHash",
                value: "AQAAAAIAAYagAAAAEDtwbNr98RTUpfrjz1whQxyKFH7RF4WDx4PjZPYzjY5+62MQ9AHNuR8WabZw/zygzw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "SenhaHash",
                value: "AQAAAAIAAYagAAAAEED0kY+0rJGk8SCL2GjwNAP88maisi2J3d9BN1EfYCo/X4iUvPjCxbkjFewSUM1uYA==");
        }
    }
}
