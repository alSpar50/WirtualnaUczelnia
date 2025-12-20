using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WirtualnaUczelnia.Migrations
{
    /// <inheritdoc />
    public partial class AddIsHiddenFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Transitions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Buildings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Transitions");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Buildings");
        }
    }
}
