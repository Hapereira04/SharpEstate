using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharpEstate.Migrations
{
    /// <inheritdoc />
    public partial class RefatoracaoCaracteristicas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "CaracteristicasCatalogo");

            migrationBuilder.AddColumn<double>(
                name: "AreaBruta",
                table: "Imoveis",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataRegisto",
                table: "Imoveis",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Piso",
                table: "Imoveis",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GrupoCaracteristicaId",
                table: "CaracteristicasCatalogo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GruposCaracteristicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GruposCaracteristicas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaracteristicasCatalogo_GrupoCaracteristicaId",
                table: "CaracteristicasCatalogo",
                column: "GrupoCaracteristicaId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaracteristicasCatalogo_GruposCaracteristicas_GrupoCaracteristicaId",
                table: "CaracteristicasCatalogo",
                column: "GrupoCaracteristicaId",
                principalTable: "GruposCaracteristicas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaracteristicasCatalogo_GruposCaracteristicas_GrupoCaracteristicaId",
                table: "CaracteristicasCatalogo");

            migrationBuilder.DropTable(
                name: "GruposCaracteristicas");

            migrationBuilder.DropIndex(
                name: "IX_CaracteristicasCatalogo_GrupoCaracteristicaId",
                table: "CaracteristicasCatalogo");

            migrationBuilder.DropColumn(
                name: "AreaBruta",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "DataRegisto",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "Piso",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "GrupoCaracteristicaId",
                table: "CaracteristicasCatalogo");

            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "CaracteristicasCatalogo",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
