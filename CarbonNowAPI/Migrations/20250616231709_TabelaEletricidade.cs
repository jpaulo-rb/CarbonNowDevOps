using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarbonNowAPI.Migrations
{
    /// <inheritdoc />
    public partial class TabelaEletricidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "SEQ_ID_ELETRICIDADE");

            migrationBuilder.CreateTable(
                name: "Eletricidade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false, defaultValueSql: "SEQ_ID_ELETRICIDADE.NEXTVAL"),
                    UnidadeEletricidade = table.Column<string>(type: "NVARCHAR2(3)", maxLength: 3, nullable: false, defaultValueSql: "'KWH'"),
                    ValorEletricidade = table.Column<decimal>(type: "NUMBER(18,2)", nullable: false),
                    DataEstimacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "TRUNC(SYSDATE)"),
                    CarbonoKg = table.Column<decimal>(type: "NUMBER(18,2)", nullable: false),
                    UsuarioId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eletricidade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eletricidade_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Eletricidade_UsuarioId",
                table: "Eletricidade",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Eletricidade");

            migrationBuilder.DropSequence(
                name: "SEQ_ID_ELETRICIDADE");
        }
    }
}
