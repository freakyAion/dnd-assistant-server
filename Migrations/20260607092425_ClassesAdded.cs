using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dnd_assistant.Migrations
{
    /// <inheritdoc />
    public partial class ClassesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Spells",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Items",
                newName: "ID");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Spells",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Spells",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Items",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Items",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    HitDieSides = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    SavingThrows = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ClassLevelProgressions",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    ProficiencyBonus = table.Column<int>(type: "integer", nullable: false),
                    SpellSlots = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassLevelProgressions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClassLevelProgressions_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassSpells",
                columns: table => new
                {
                    ClassesID = table.Column<Guid>(type: "uuid", nullable: false),
                    SpellsID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSpells", x => new { x.ClassesID, x.SpellsID });
                    table.ForeignKey(
                        name: "FK_ClassSpells_Classes_ClassesID",
                        column: x => x.ClassesID,
                        principalTable: "Classes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSpells_Spells_SpellsID",
                        column: x => x.SpellsID,
                        principalTable: "Spells",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassLevelProgressionFeatures",
                columns: table => new
                {
                    FeaturesID = table.Column<Guid>(type: "uuid", nullable: false),
                    LevelProgressionsID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassLevelProgressionFeatures", x => new { x.FeaturesID, x.LevelProgressionsID });
                    table.ForeignKey(
                        name: "FK_ClassLevelProgressionFeatures_ClassLevelProgressions_LevelP~",
                        column: x => x.LevelProgressionsID,
                        principalTable: "ClassLevelProgressions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassLevelProgressionFeatures_Features_FeaturesID",
                        column: x => x.FeaturesID,
                        principalTable: "Features",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_Name",
                table: "Classes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassLevelProgressionFeatures_LevelProgressionsID",
                table: "ClassLevelProgressionFeatures",
                column: "LevelProgressionsID");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLevelProgressions_ClassId_Level",
                table: "ClassLevelProgressions",
                columns: new[] { "ClassId", "Level" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassSpells_SpellsID",
                table: "ClassSpells",
                column: "SpellsID");

            migrationBuilder.CreateIndex(
                name: "IX_Features_Name",
                table: "Features",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassLevelProgressionFeatures");

            migrationBuilder.DropTable(
                name: "ClassSpells");

            migrationBuilder.DropTable(
                name: "ClassLevelProgressions");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Spells");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "Spells");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Spells",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Items",
                newName: "Id");
        }
    }
}
