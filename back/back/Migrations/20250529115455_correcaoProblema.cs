using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendTesteESII.Migrations
{
    /// <inheritdoc />
    public partial class correcaoProblema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projeto_cliente_cliente_id",
                table: "projeto");

            migrationBuilder.DropForeignKey(
                name: "FK_projeto_utilizador_utilizador_id",
                table: "projeto");

            migrationBuilder.DropIndex(
                name: "IX_projeto_cliente_id",
                table: "projeto");

            migrationBuilder.DropIndex(
                name: "IX_projeto_utilizador_id",
                table: "projeto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
