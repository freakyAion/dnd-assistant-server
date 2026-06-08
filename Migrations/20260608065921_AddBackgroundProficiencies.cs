using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dnd_assistant.Migrations
{
    /// <inheritdoc />
    public partial class AddBackgroundProficiencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int[]>(
                name: "SkillProficiencies",
                table: "Backgrounds",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SkillProficiencies",
                table: "Backgrounds");
        }
    }
}
