using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WirtualnaUczelnia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBuildings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "Locations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_BuildingId",
                table: "Locations",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Buildings_BuildingId",
                table: "Locations",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Buildings_BuildingId",
                table: "Locations");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Locations_BuildingId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Locations");
        }
    }
}
