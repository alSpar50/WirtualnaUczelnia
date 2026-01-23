using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WirtualnaUczelnia.Migrations
{
    /// <inheritdoc />
    public partial class AddCostAndLocationType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cost",
                table: "Transitions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Floor",
                table: "Locations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Locations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Transitions");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Locations");
        }
    }
}
