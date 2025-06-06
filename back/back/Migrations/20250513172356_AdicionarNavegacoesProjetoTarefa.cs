using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendTesteESII.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarNavegacoesProjetoTarefa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateIndex(
                //name: "IX_tarefa_utilizador_id",
                //table: "tarefa",
                //column: "utilizador_id");

            migrationBuilder.CreateIndex(
                name: "IX_projeto_cliente_id",
                table: "projeto",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_projeto_utilizador_id",
                table: "projeto",
                column: "utilizador_id");

            migrationBuilder.AddForeignKey(
                name: "FK_projeto_cliente_cliente_id",
                table: "projeto",
                column: "cliente_id",
                principalTable: "cliente",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_projeto_utilizador_utilizador_id",
                table: "projeto",
                column: "utilizador_id",
                principalTable: "utilizador",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
               // name: "FK_tarefa_utilizador_utilizador_id",
                //table: "tarefa",
                //column: "utilizador_id",
                //principalTable: "utilizador",
                //principalColumn: "Id",
                //onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projeto_cliente_cliente_id",
                table: "projeto");

            migrationBuilder.DropForeignKey(
                name: "FK_projeto_utilizador_utilizador_id",
                table: "projeto");

            migrationBuilder.DropForeignKey(
                name: "FK_tarefa_utilizador_utilizador_id",
                table: "tarefa");

            migrationBuilder.DropIndex(
                name: "IX_tarefa_utilizador_id",
                table: "tarefa");

            migrationBuilder.DropIndex(
                name: "IX_projeto_cliente_id",
                table: "projeto");

            migrationBuilder.DropIndex(
                name: "IX_projeto_utilizador_id",
                table: "projeto");
        }
    }
}
