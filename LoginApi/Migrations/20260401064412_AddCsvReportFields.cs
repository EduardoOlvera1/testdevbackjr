using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoginApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCsvReportFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApellidoMaterno",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApellidoPaterno",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombres",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    IDArea = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.IDArea);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_IDArea",
                table: "Users",
                column: "IDArea");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Areas_IDArea",
                table: "Users",
                column: "IDArea",
                principalTable: "Areas",
                principalColumn: "IDArea",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Areas_IDArea",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Users_IDArea",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ApellidoMaterno",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ApellidoPaterno",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Nombres",
                table: "Users");
        }
    }
}
