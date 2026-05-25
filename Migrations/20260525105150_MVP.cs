using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dnd_assistant.Migrations
{
    /// <inheritdoc />
    public partial class MVP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Backgrounds",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Pitch = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "jsonb", nullable: true),
                    Proficiencies = table.Column<string>(type: "jsonb", nullable: true),
                    Equipment = table.Column<string>(type: "jsonb", nullable: true),
                    Source = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backgrounds", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Pitch = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "jsonb", nullable: true),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Rarity = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Cost = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Weight = table.Column<double>(type: "double precision", nullable: true),
                    Properties = table.Column<string>(type: "jsonb", nullable: true),
                    Source = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Pitch = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "jsonb", nullable: true),
                    Bonuses = table.Column<string>(type: "jsonb", nullable: true),
                    Source = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Spells",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Pitch = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "jsonb", nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    School = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CastingTime = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Range = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Components = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Duration = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Source = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spells", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SubSpecies",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    SpeciesID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Pitch = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "jsonb", nullable: true),
                    Bonuses = table.Column<string>(type: "jsonb", nullable: true),
                    Source = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubSpecies", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SubSpecies_Species_SpeciesID",
                        column: x => x.SpeciesID,
                        principalTable: "Species",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubSpecies_SpeciesID",
                table: "SubSpecies",
                column: "SpeciesID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Backgrounds");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Spells");

            migrationBuilder.DropTable(
                name: "SubSpecies");

            migrationBuilder.DropTable(
                name: "Species");
        }
    }
}
