using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarbonNowAPI.Migrations
{
    /// <inheritdoc />
    public partial class TabelaUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "SEQ_ID_USUARIO");

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false, defaultValueSql: "SEQ_ID_USUARIO.NEXTVAL"),
                    Nome = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(128)", maxLength: 128, nullable: false),
                    Senha = table.Column<string>(type: "NVARCHAR2(60)", maxLength: 60, nullable: false),
                    Regra = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email",
                table: "Usuario",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropSequence(
                name: "SEQ_ID_USUARIO");
        }
    }
}
