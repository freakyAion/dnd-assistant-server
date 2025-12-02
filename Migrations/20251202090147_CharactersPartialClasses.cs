using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dnd_assistant.Migrations
{
    /// <inheritdoc />
    public partial class CharactersPartialClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassArmourProficiencies",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ShieldProficiency = table.Column<bool>(type: "boolean", nullable: false),
                    LightProficiency = table.Column<bool>(type: "boolean", nullable: false),
                    MediumProficiency = table.Column<bool>(type: "boolean", nullable: false),
                    HeavyProficiency = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassArmourProficiencies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    HitDie = table.Column<int>(type: "integer", nullable: false),
                    SkillProficienciesAmount = table.Column<int>(type: "integer", nullable: false),
                    SubClassUnlockingLevel = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    StartingClassID = table.Column<Guid>(type: "uuid", nullable: false),
                    Ideals = table.Column<string>(type: "text", nullable: true),
                    Bonds = table.Column<string>(type: "text", nullable: true),
                    Flaws = table.Column<string>(type: "text", nullable: true),
                    Sex = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    Complexion = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    HairColour = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    EyeColour = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    alignment = table.Column<int>(type: "integer", nullable: true),
                    Religion = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    Height = table.Column<double>(type: "double precision", nullable: true),
                    Weight = table.Column<double>(type: "double precision", nullable: true),
                    Age = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    Looks = table.Column<string>(type: "text", nullable: true),
                    Temper = table.Column<string>(type: "text", nullable: true),
                    Backstory = table.Column<string>(type: "text", nullable: true),
                    Statuseffects = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Characters_Classes_StartingClassID",
                        column: x => x.StartingClassID,
                        principalTable: "Classes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_Classes_UserID",
                        column: x => x.UserID,
                        principalTable: "Classes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassFeatures",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UnlockingLevel = table.Column<int>(type: "integer", nullable: false),
                    ClassID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassFeatures", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClassFeatures_Classes_ClassID",
                        column: x => x.ClassID,
                        principalTable: "Classes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_StartingClassID",
                table: "Characters",
                column: "StartingClassID");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_UserID",
                table: "Characters",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ClassFeatures_ClassID",
                table: "ClassFeatures",
                column: "ClassID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "ClassArmourProficiencies");

            migrationBuilder.DropTable(
                name: "ClassFeatures");

            migrationBuilder.DropTable(
                name: "Classes");
        }
    }
}
