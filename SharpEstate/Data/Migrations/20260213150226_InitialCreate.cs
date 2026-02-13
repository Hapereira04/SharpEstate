using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharpEstate.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Caracteristica",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caracteristica", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImovelCaracteristica",
                columns: table => new
                {
                    ImovelId = table.Column<int>(type: "int", nullable: false),
                    CaracteristicaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImovelCaracteristica", x => new { x.ImovelId, x.CaracteristicaId });
                    table.ForeignKey(
                        name: "FK_ImovelCaracteristica_Caracteristica_CaracteristicaId",
                        column: x => x.CaracteristicaId,
                        principalTable: "Caracteristica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ImovelCaracteristica_Imoveis_ImovelId",
                        column: x => x.ImovelId,
                        principalTable: "Imoveis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImovelCaracteristica_CaracteristicaId",
                table: "ImovelCaracteristica",
                column: "CaracteristicaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImovelCaracteristica");

            migrationBuilder.DropTable(
                name: "Caracteristica");
        }
    }
}
