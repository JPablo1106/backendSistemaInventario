using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendSistemaInventario.Migrations
{
    /// <inheritdoc />
    public partial class prueba150425 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "celulares",
                columns: table => new
                {
                    idCelular = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    marca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    modelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    compania = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    numSerie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imei = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    numCelular = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_celulares", x => x.idCelular);
                    table.ForeignKey(
                        name: "FK_celulares_usuarios_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "usuarios",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "radios",
                columns: table => new
                {
                    idRadio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    marca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    modelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    issi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    numSerie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tieneAntena = table.Column<bool>(type: "bit", nullable: false),
                    tieneClip = table.Column<bool>(type: "bit", nullable: false),
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radios", x => x.idRadio);
                    table.ForeignKey(
                        name: "FK_radios_usuarios_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "usuarios",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tabletas",
                columns: table => new
                {
                    idTableta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    marca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    modelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    numSerie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accesorios = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tabletas", x => x.idTableta);
                    table.ForeignKey(
                        name: "FK_tabletas_usuarios_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "usuarios",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_celulares_idUsuario",
                table: "celulares",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_radios_idUsuario",
                table: "radios",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_tabletas_idUsuario",
                table: "tabletas",
                column: "idUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "celulares");

            migrationBuilder.DropTable(
                name: "radios");

            migrationBuilder.DropTable(
                name: "tabletas");
        }
    }
}
