using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoBanco.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialOracle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Numero = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    TipoProduto = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TipoProdutoDiscriminator = table.Column<string>(type: "NVARCHAR2(13)", maxLength: 13, nullable: false),
                    ValorMaximo = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    TaxaJuros = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    TaxaMdr = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    EmpresaConveniada = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                    AgenciaId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TipoCliente = table.Column<string>(type: "NVARCHAR2(8)", maxLength: 8, nullable: false),
                    Cpf = table.Column<string>(type: "NVARCHAR2(11)", maxLength: 11, nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Cnpj = table.Column<string>(type: "NVARCHAR2(14)", maxLength: 14, nullable: true),
                    RazaoSocial = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clientes_Agencias_AgenciaId",
                        column: x => x.AgenciaId,
                        principalTable: "Agencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contratacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ClienteId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ProdutoId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Status = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ValorSolicitado = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    ScoreCredito = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    FaturamentoMensal = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    TaxaAplicada = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    MotivoRecusa = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ProcessadoEm = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contratacoes_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contratacoes_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_AgenciaId",
                table: "Clientes",
                column: "AgenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Cnpj",
                table: "Clientes",
                column: "Cnpj",
                unique: true,
                filter: "\"Cnpj\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Cpf",
                table: "Clientes",
                column: "Cpf",
                unique: true,
                filter: "\"Cpf\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Contratacoes_ClienteId",
                table: "Contratacoes",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Contratacoes_ProdutoId",
                table: "Contratacoes",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contratacoes");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Agencias");
        }
    }
}
