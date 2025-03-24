using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendSistemaInventario.Migrations
{
    /// <inheritdoc />
    public partial class prueba2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "idEquipoSeguridad",
                table: "detalleAsignaciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "numSerieEquipoSeg",
                table: "detalleAsignaciones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_detalleAsignaciones_idEquipoSeguridad",
                table: "detalleAsignaciones",
                column: "idEquipoSeguridad");

            migrationBuilder.AddForeignKey(
                name: "FK_detalleAsignaciones_equiposSeguridad_idEquipoSeguridad",
                table: "detalleAsignaciones",
                column: "idEquipoSeguridad",
                principalTable: "equiposSeguridad",
                principalColumn: "idEquipoSeguridad");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_detalleAsignaciones_equiposSeguridad_idEquipoSeguridad",
                table: "detalleAsignaciones");

            migrationBuilder.DropIndex(
                name: "IX_detalleAsignaciones_idEquipoSeguridad",
                table: "detalleAsignaciones");

            migrationBuilder.DropColumn(
                name: "idEquipoSeguridad",
                table: "detalleAsignaciones");

            migrationBuilder.DropColumn(
                name: "numSerieEquipoSeg",
                table: "detalleAsignaciones");
        }
    }
}
