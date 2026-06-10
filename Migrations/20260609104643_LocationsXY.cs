using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dnd_assistant.Migrations
{
    /// <inheritdoc />
    public partial class LocationsXY : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MapImageUrl",
                table: "Worlds",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "X",
                table: "Locations",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Y",
                table: "Locations",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapImageUrl",
                table: "Worlds");

            migrationBuilder.DropColumn(
                name: "X",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "Locations");
        }
    }
}
