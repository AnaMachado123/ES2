using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendTesteESII.Migrations
{
    /// <inheritdoc />
    public partial class AddTarefasToProjeto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tarefa_projeto_id",
                table: "tarefa",
                column: "projeto_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tarefa_projeto_projeto_id",
                table: "tarefa",
                column: "projeto_id",
                principalTable: "projeto",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tarefa_projeto_projeto_id",
                table: "tarefa");

            migrationBuilder.DropIndex(
                name: "IX_tarefa_projeto_id",
                table: "tarefa");
        }
    }
}
