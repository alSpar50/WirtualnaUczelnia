using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WirtualnaUczelnia.Migrations
{
    /// <inheritdoc />
    public partial class AddBuildingImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "Buildings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "Buildings");
        }
    }
}
