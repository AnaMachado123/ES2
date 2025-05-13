using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendTesteESII.Migrations
{
    /// <inheritdoc />
    public partial class AddUtilizadorNavigationToTarefa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tarefa_utilizador_id",
                table: "tarefa",
                column: "utilizador_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tarefa_utilizador_utilizador_id",
                table: "tarefa",
                column: "utilizador_id",
                principalTable: "utilizador",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tarefa_utilizador_utilizador_id",
                table: "tarefa");

            migrationBuilder.DropIndex(
                name: "IX_tarefa_utilizador_id",
                table: "tarefa");
        }
    }
}
