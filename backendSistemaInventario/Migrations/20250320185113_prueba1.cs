using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendSistemaInventario.Migrations
{
    /// <inheritdoc />
    public partial class prueba1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "administrador",
                columns: table => new
                {
                    idAdministrador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreAdmin = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    usuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contraseña = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administrador", x => x.idAdministrador);
                });

            migrationBuilder.CreateTable(
                name: "componentes",
                columns: table => new
                {
                    idComponente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipoComponente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    marcaComponente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    modeloMonitor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idiomaTeclado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modeloTelefono = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_componentes", x => x.idComponente);
                });

            migrationBuilder.CreateTable(
                name: "discoDuro",
                columns: table => new
                {
                    idDiscoDuro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    marca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    modelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    capacidad = table.Column<int>(type: "int", nullable: false),
                    c = table.Column<int>(type: "int", nullable: false),
                    d = table.Column<int>(type: "int", nullable: false),
                    e = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discoDuro", x => x.idDiscoDuro);
                });

            migrationBuilder.CreateTable(
                name: "dispositivosExt",
                columns: table => new
                {
                    idDispExt = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    marca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dispositivosExt", x => x.idDispExt);
                });

            migrationBuilder.CreateTable(
                name: "equiposSeguridad",
                columns: table => new
                {
                    idEquipoSeguridad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    marca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    modelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    capacidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tipo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equiposSeguridad", x => x.idEquipoSeguridad);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreUsuario = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    area = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    departamento = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.idUsuario);
                });

            migrationBuilder.CreateTable(
                name: "refreshToken",
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
                    table.PrimaryKey("PK_refreshToken", x => x.id);
                    table.ForeignKey(
                        name: "FK_refreshToken_administrador_administradorId",
                        column: x => x.administradorId,
                        principalTable: "administrador",
                        principalColumn: "idAdministrador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "equipos",
                columns: table => new
                {
                    idEquipo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    marca = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    modelo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    tipoEquipo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    velocidadProcesador = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    tipoProcesador = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    memoriaRam = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    tipoMemoriaRam = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    idDiscoDuro = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipos", x => x.idEquipo);
                    table.ForeignKey(
                        name: "FK_equipos_discoDuro_idDiscoDuro",
                        column: x => x.idDiscoDuro,
                        principalTable: "discoDuro",
                        principalColumn: "idDiscoDuro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asignaciones",
                columns: table => new
                {
                    idAsignacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asignaciones", x => x.idAsignacion);
                    table.ForeignKey(
                        name: "FK_asignaciones_usuarios_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "usuarios",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detalleAsignaciones",
                columns: table => new
                {
                    idDetalleAsignacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idAsignacion = table.Column<int>(type: "int", nullable: false),
                    idEquipo = table.Column<int>(type: "int", nullable: true),
                    numSerieEquipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ipAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ipCpuRed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idComponente = table.Column<int>(type: "int", nullable: true),
                    numSerieComponente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idDispExt = table.Column<int>(type: "int", nullable: true),
                    numSerieDispExt = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalleAsignaciones", x => x.idDetalleAsignacion);
                    table.ForeignKey(
                        name: "FK_detalleAsignaciones_asignaciones_idAsignacion",
                        column: x => x.idAsignacion,
                        principalTable: "asignaciones",
                        principalColumn: "idAsignacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_detalleAsignaciones_componentes_idComponente",
                        column: x => x.idComponente,
                        principalTable: "componentes",
                        principalColumn: "idComponente");
                    table.ForeignKey(
                        name: "FK_detalleAsignaciones_dispositivosExt_idDispExt",
                        column: x => x.idDispExt,
                        principalTable: "dispositivosExt",
                        principalColumn: "idDispExt");
                    table.ForeignKey(
                        name: "FK_detalleAsignaciones_equipos_idEquipo",
                        column: x => x.idEquipo,
                        principalTable: "equipos",
                        principalColumn: "idEquipo");
                });

            migrationBuilder.CreateIndex(
                name: "IX_asignaciones_idUsuario",
                table: "asignaciones",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_detalleAsignaciones_idAsignacion",
                table: "detalleAsignaciones",
                column: "idAsignacion");

            migrationBuilder.CreateIndex(
                name: "IX_detalleAsignaciones_idComponente",
                table: "detalleAsignaciones",
                column: "idComponente");

            migrationBuilder.CreateIndex(
                name: "IX_detalleAsignaciones_idDispExt",
                table: "detalleAsignaciones",
                column: "idDispExt");

            migrationBuilder.CreateIndex(
                name: "IX_detalleAsignaciones_idEquipo",
                table: "detalleAsignaciones",
                column: "idEquipo");

            migrationBuilder.CreateIndex(
                name: "IX_equipos_idDiscoDuro",
                table: "equipos",
                column: "idDiscoDuro");

            migrationBuilder.CreateIndex(
                name: "IX_refreshToken_administradorId",
                table: "refreshToken",
                column: "administradorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "detalleAsignaciones");

            migrationBuilder.DropTable(
                name: "equiposSeguridad");

            migrationBuilder.DropTable(
                name: "refreshToken");

            migrationBuilder.DropTable(
                name: "asignaciones");

            migrationBuilder.DropTable(
                name: "componentes");

            migrationBuilder.DropTable(
                name: "dispositivosExt");

            migrationBuilder.DropTable(
                name: "equipos");

            migrationBuilder.DropTable(
                name: "administrador");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "discoDuro");
        }
    }
}
