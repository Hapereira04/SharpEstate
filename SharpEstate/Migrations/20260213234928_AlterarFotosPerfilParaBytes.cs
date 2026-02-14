using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharpEstate.Migrations
{
    /// <inheritdoc />
    public partial class AlterarFotosPerfilParaBytes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FotoUrl",
                table: "Consultores",
                newName: "ContentTypeFoto");

            migrationBuilder.AddColumn<byte[]>(
                name: "FotoPerfil",
                table: "Consultores",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentTypeFoto",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FotoPerfil",
                table: "Clientes",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoPerfil",
                table: "Consultores");

            migrationBuilder.DropColumn(
                name: "ContentTypeFoto",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "FotoPerfil",
                table: "Clientes");

            migrationBuilder.RenameColumn(
                name: "ContentTypeFoto",
                table: "Consultores",
                newName: "FotoUrl");
        }
    }
}
