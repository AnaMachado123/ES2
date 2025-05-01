using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendTesteESII.Migrations
{
    /// <inheritdoc />
    public partial class AtualizaModelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "password",
                table: "utilizador",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "utilizador",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "utilizador",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "utilizador",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "horas_dia",
                table: "utilizador",
                newName: "HorasDia");

            migrationBuilder.AlterColumn<string>(
                name: "tipo",
                table: "utilizador",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "utilizador",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "utilizador",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "utilizador",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "utilizador",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "utilizador",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "utilizador",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "utilizador",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "HorasDia",
                table: "utilizador",
                newName: "horas_dia");

            migrationBuilder.AlterColumn<int>(
                name: "tipo",
                table: "utilizador",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "utilizador",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "utilizador",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "utilizador",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
