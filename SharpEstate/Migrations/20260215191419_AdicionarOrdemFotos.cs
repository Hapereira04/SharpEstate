using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharpEstate.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarOrdemFotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCapa",
                table: "Fotos");

            migrationBuilder.AddColumn<int>(
                name: "Ordem",
                table: "Fotos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ordem",
                table: "Fotos");

            migrationBuilder.AddColumn<bool>(
                name: "IsCapa",
                table: "Fotos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
