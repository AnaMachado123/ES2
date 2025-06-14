﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendTesteESII.Migrations
{
    /// <inheritdoc />
    public partial class PermitirProjetoOpcional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tarefa_projeto_projeto_id",
                table: "tarefa");

            migrationBuilder.AlterColumn<int>(
                name: "projeto_id",
                table: "tarefa",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_tarefa_projeto_projeto_id",
                table: "tarefa",
                column: "projeto_id",
                principalTable: "projeto",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tarefa_projeto_projeto_id",
                table: "tarefa");

            migrationBuilder.AlterColumn<int>(
                name: "projeto_id",
                table: "tarefa",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tarefa_projeto_projeto_id",
                table: "tarefa",
                column: "projeto_id",
                principalTable: "projeto",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
