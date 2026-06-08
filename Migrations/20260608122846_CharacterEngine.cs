using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dnd_assistant.Migrations
{
    /// <inheritdoc />
    public partial class CharacterEngine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SpeciesID = table.Column<Guid>(type: "uuid", nullable: false),
                    BackgroundID = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassID = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    ExperiencePoints = table.Column<int>(type: "integer", nullable: false),
                    Alignment = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Biography = table.Column<string>(type: "text", nullable: false),
                    Age = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Height = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Weight = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PhysicalAppearance = table.Column<string>(type: "text", nullable: false),
                    Strength = table.Column<int>(type: "integer", nullable: false),
                    Dexterity = table.Column<int>(type: "integer", nullable: false),
                    Constitution = table.Column<int>(type: "integer", nullable: false),
                    Intelligence = table.Column<int>(type: "integer", nullable: false),
                    Wisdom = table.Column<int>(type: "integer", nullable: false),
                    Charisma = table.Column<int>(type: "integer", nullable: false),
                    CurrentHitPoints = table.Column<int>(type: "integer", nullable: false),
                    MaxHitPoints = table.Column<int>(type: "integer", nullable: false),
                    TemporaryHitPoints = table.Column<int>(type: "integer", nullable: false),
                    DeathSaveSuccesses = table.Column<int>(type: "integer", nullable: false),
                    DeathSaveFailures = table.Column<int>(type: "integer", nullable: false),
                    CurrentSpeedOverride = table.Column<int>(type: "integer", nullable: false),
                    ActiveConditions = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    CustomSkillProficiencies = table.Column<int[]>(type: "integer[]", nullable: false),
                    ExpendedSpellSlots = table.Column<int[]>(type: "integer[]", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Characters_Backgrounds_BackgroundID",
                        column: x => x.BackgroundID,
                        principalTable: "Backgrounds",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_Classes_ClassID",
                        column: x => x.ClassID,
                        principalTable: "Classes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_Species_SpeciesID",
                        column: x => x.SpeciesID,
                        principalTable: "Species",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterItems",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    CharacterID = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemID = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    IsEquipped = table.Column<bool>(type: "boolean", nullable: false),
                    IsAttuned = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CharacterItems_Characters_CharacterID",
                        column: x => x.CharacterID,
                        principalTable: "Characters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterItems_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterSpells",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    CharacterID = table.Column<Guid>(type: "uuid", nullable: false),
                    SpellID = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPrepared = table.Column<bool>(type: "boolean", nullable: false),
                    IsAlwaysPrepared = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterSpells", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CharacterSpells_Characters_CharacterID",
                        column: x => x.CharacterID,
                        principalTable: "Characters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterSpells_Spells_SpellID",
                        column: x => x.SpellID,
                        principalTable: "Spells",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterItems_CharacterID",
                table: "CharacterItems",
                column: "CharacterID");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterItems_ItemID",
                table: "CharacterItems",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_BackgroundID",
                table: "Characters",
                column: "BackgroundID");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_ClassID",
                table: "Characters",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_SpeciesID",
                table: "Characters",
                column: "SpeciesID");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_UserID",
                table: "Characters",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSpells_CharacterID",
                table: "CharacterSpells",
                column: "CharacterID");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSpells_SpellID",
                table: "CharacterSpells",
                column: "SpellID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterItems");

            migrationBuilder.DropTable(
                name: "CharacterSpells");

            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
