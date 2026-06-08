using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dnd_assistant.Migrations
{
    /// <inheritdoc />
    public partial class RulesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassLevelProgressionFeatures_Features_FeaturesID",
                table: "ClassLevelProgressionFeatures");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.CreateTable(
                name: "BackgroundFeatures",
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
                    table.PrimaryKey("PK_BackgroundFeatures", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Backgrounds",
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
                    table.PrimaryKey("PK_Backgrounds", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ClassFeatures",
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
                    table.PrimaryKey("PK_ClassFeatures", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Size = table.Column<string>(type: "text", nullable: false),
                    BaseSpeed = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SpeciesTraits",
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
                    table.PrimaryKey("PK_SpeciesTraits", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BackgroundToFeatures",
                columns: table => new
                {
                    BackgroundID = table.Column<Guid>(type: "uuid", nullable: false),
                    FeaturesID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackgroundToFeatures", x => new { x.BackgroundID, x.FeaturesID });
                    table.ForeignKey(
                        name: "FK_BackgroundToFeatures_BackgroundFeatures_FeaturesID",
                        column: x => x.FeaturesID,
                        principalTable: "BackgroundFeatures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BackgroundToFeatures_Backgrounds_BackgroundID",
                        column: x => x.BackgroundID,
                        principalTable: "Backgrounds",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpeciesToTraits",
                columns: table => new
                {
                    SpeciesID = table.Column<Guid>(type: "uuid", nullable: false),
                    TraitsID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciesToTraits", x => new { x.SpeciesID, x.TraitsID });
                    table.ForeignKey(
                        name: "FK_SpeciesToTraits_SpeciesTraits_TraitsID",
                        column: x => x.TraitsID,
                        principalTable: "SpeciesTraits",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpeciesToTraits_Species_SpeciesID",
                        column: x => x.SpeciesID,
                        principalTable: "Species",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BackgroundFeatures_Name",
                table: "BackgroundFeatures",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Backgrounds_Name",
                table: "Backgrounds",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BackgroundToFeatures_FeaturesID",
                table: "BackgroundToFeatures",
                column: "FeaturesID");

            migrationBuilder.CreateIndex(
                name: "IX_ClassFeatures_Name",
                table: "ClassFeatures",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rules_Slug",
                table: "Rules",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Species_Name",
                table: "Species",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesToTraits_TraitsID",
                table: "SpeciesToTraits",
                column: "TraitsID");

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesTraits_Name",
                table: "SpeciesTraits",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassLevelProgressionFeatures_ClassFeatures_FeaturesID",
                table: "ClassLevelProgressionFeatures",
                column: "FeaturesID",
                principalTable: "ClassFeatures",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassLevelProgressionFeatures_ClassFeatures_FeaturesID",
                table: "ClassLevelProgressionFeatures");

            migrationBuilder.DropTable(
                name: "BackgroundToFeatures");

            migrationBuilder.DropTable(
                name: "ClassFeatures");

            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "SpeciesToTraits");

            migrationBuilder.DropTable(
                name: "BackgroundFeatures");

            migrationBuilder.DropTable(
                name: "Backgrounds");

            migrationBuilder.DropTable(
                name: "SpeciesTraits");

            migrationBuilder.DropTable(
                name: "Species");

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Features_Name",
                table: "Features",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassLevelProgressionFeatures_Features_FeaturesID",
                table: "ClassLevelProgressionFeatures",
                column: "FeaturesID",
                principalTable: "Features",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
