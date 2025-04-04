using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendSistemaInventario.Migrations
{
    /// <inheritdoc />
    public partial class prueba4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "passwordResetTokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expira = table.Column<DateTime>(type: "datetime2", nullable: false),
                    esValido = table.Column<bool>(type: "bit", nullable: false),
                    administradorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passwordResetTokens", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "passwordResetTokens");
        }
    }
}
