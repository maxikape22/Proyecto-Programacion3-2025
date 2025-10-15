using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto2024.BD.Migrations
{
    /// <inheritdoc />
    public partial class autenticcion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Simbolos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreSimbolo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Moneda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SimboloRelacionadoId = table.Column<int>(type: "int", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Simbolos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Simbolos_Simbolos_SimboloRelacionadoId",
                        column: x => x.SimboloRelacionadoId,
                        principalTable: "Simbolos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Simbolos_SimboloRelacionadoId",
                table: "Simbolos",
                column: "SimboloRelacionadoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Simbolos");
        }
    }
}
