using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharpEstate.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModeloImobiliariaCompleto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Imoveis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoNegocio = table.Column<int>(type: "int", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quartos = table.Column<int>(type: "int", nullable: false),
                    CasasBanho = table.Column<int>(type: "int", nullable: false),
                    AreaUtil = table.Column<double>(type: "float", nullable: false),
                    AreaBrutaPrivativa = table.Column<double>(type: "float", nullable: true),
                    Estacionamento = table.Column<int>(type: "int", nullable: false),
                    Certificado = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    AnoConstrucao = table.Column<int>(type: "int", nullable: true),
                    NumeroFrentes = table.Column<int>(type: "int", nullable: true),
                    NumeroPisos = table.Column<int>(type: "int", nullable: true),
                    Vistas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrientacaoSolar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Distrito = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Concelho = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Freguesia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zona = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnderecoCompleto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DataAngariacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConsultorId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imoveis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InteressesCompra",
                columns: table => new
                {
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    PrecoMaximo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tipologias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Concelhos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Urgencia = table.Column<int>(type: "int", nullable: false),
                    DataRegisto = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteressesCompra", x => x.ClienteId);
                    table.ForeignKey(
                        name: "FK_InteressesCompra_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Caracteristicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grupo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImovelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caracteristicas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Caracteristicas_Imoveis_ImovelId",
                        column: x => x.ImovelId,
                        principalTable: "Imoveis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DadosImagem = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCapa = table.Column<bool>(type: "bit", nullable: false),
                    ImovelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fotos_Imoveis_ImovelId",
                        column: x => x.ImovelId,
                        principalTable: "Imoveis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImoveisProprietarios",
                columns: table => new
                {
                    ImovelId = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImoveisProprietarios", x => new { x.ImovelId, x.ClienteId });
                    table.ForeignKey(
                        name: "FK_ImoveisProprietarios_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImoveisProprietarios_Imoveis_ImovelId",
                        column: x => x.ImovelId,
                        principalTable: "Imoveis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Caracteristicas_ImovelId",
                table: "Caracteristicas",
                column: "ImovelId");

            migrationBuilder.CreateIndex(
                name: "IX_Fotos_ImovelId",
                table: "Fotos",
                column: "ImovelId");

            migrationBuilder.CreateIndex(
                name: "IX_ImoveisProprietarios_ClienteId",
                table: "ImoveisProprietarios",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Caracteristicas");

            migrationBuilder.DropTable(
                name: "Fotos");

            migrationBuilder.DropTable(
                name: "ImoveisProprietarios");

            migrationBuilder.DropTable(
                name: "InteressesCompra");

            migrationBuilder.DropTable(
                name: "Imoveis");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
