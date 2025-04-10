using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BackendTesteESII.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaUtilizadorProjeto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "relatorio_projeto",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    projeto_id = table.Column<int>(type: "integer", nullable: false),
                    cliente_id = table.Column<int>(type: "integer", nullable: false),
                    total_horas = table.Column<int>(type: "integer", nullable: false),
                    total_preco = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_relatorio_projeto", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "utilizador_projeto",
                columns: table => new
                {
                    utilizador_id = table.Column<int>(type: "integer", nullable: false),
                    projeto_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_utilizador_projeto", x => new { x.utilizador_id, x.projeto_id });
                    table.ForeignKey(
                        name: "FK_utilizador_projeto_projeto_projeto_id",
                        column: x => x.projeto_id,
                        principalTable: "projeto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_utilizador_projeto_utilizador_utilizador_id",
                        column: x => x.utilizador_id,
                        principalTable: "utilizador",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_utilizador_projeto_projeto_id",
                table: "utilizador_projeto",
                column: "projeto_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "relatorio_projeto");

            migrationBuilder.DropTable(
                name: "utilizador_projeto");
        }
    }
}
