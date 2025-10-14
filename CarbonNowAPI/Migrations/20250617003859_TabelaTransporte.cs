using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarbonNowAPI.Migrations
{
    /// <inheritdoc />
    public partial class TabelaTransporte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "SEQ_ID_TRANSPORTE");

            migrationBuilder.CreateTable(
                name: "Transporte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false, defaultValueSql: "SEQ_ID_TRANSPORTE.NEXTVAL"),
                    ValorPesoKg = table.Column<decimal>(type: "NUMBER(18,2)", nullable: false),
                    ValorDistanciaKm = table.Column<decimal>(type: "NUMBER(18,2)", nullable: false),
                    MetodoTransporte = table.Column<string>(type: "NVARCHAR2(5)", maxLength: 5, nullable: false),
                    DataEstimacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "TRUNC(SYSDATE)"),
                    CarbonoKg = table.Column<decimal>(type: "NUMBER(18,2)", nullable: false),
                    UsuarioId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transporte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transporte_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transporte_UsuarioId",
                table: "Transporte",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transporte");

            migrationBuilder.DropSequence(
                name: "SEQ_ID_TRANSPORTE");
        }
    }
}
