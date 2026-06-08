using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dnd_assistant.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Rarity = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    Weight = table.Column<decimal>(type: "numeric", nullable: false),
                    CostValue = table.Column<int>(type: "integer", nullable: false),
                    CostCurrency = table.Column<string>(type: "text", nullable: false),
                    RequiresAttunement = table.Column<bool>(type: "boolean", nullable: false),
                    AttunementPrerequisites = table.Column<string>(type: "text", nullable: true),
                    StrengthRequirement = table.Column<int>(type: "integer", nullable: true),
                    StealthDisadvantage = table.Column<bool>(type: "boolean", nullable: false),
                    AcValue = table.Column<int>(type: "integer", nullable: true),
                    AcDexBonusType = table.Column<string>(type: "text", nullable: true),
                    DamageDiceQuantity = table.Column<int>(type: "integer", nullable: true),
                    DamageDiceSides = table.Column<int>(type: "integer", nullable: true),
                    DamageType = table.Column<string>(type: "text", nullable: true),
                    Properties = table.Column<int>(type: "integer", nullable: false),
                    ContainerCapacityWeight = table.Column<decimal>(type: "numeric", nullable: true),
                    IsConsumable = table.Column<bool>(type: "boolean", nullable: false),
                    HasCharges = table.Column<bool>(type: "boolean", nullable: false),
                    MaxCharges = table.Column<int>(type: "integer", nullable: true),
                    ChargeResetCondition = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spells",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    School = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    HigherLevelDescription = table.Column<string>(type: "text", nullable: true),
                    CastingTimeValue = table.Column<int>(type: "integer", nullable: false),
                    CastingTimeType = table.Column<string>(type: "text", nullable: false),
                    CastingTimeCondition = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    RangeUnits = table.Column<string>(type: "text", nullable: false),
                    RangeValue = table.Column<int>(type: "integer", nullable: true),
                    AoeType = table.Column<string>(type: "text", nullable: true),
                    AoeValue = table.Column<int>(type: "integer", nullable: true),
                    Components = table.Column<int>(type: "integer", nullable: false),
                    MaterialComponents = table.Column<string>(type: "text", nullable: true),
                    MaterialCostGp = table.Column<int>(type: "integer", nullable: false),
                    MaterialConsumed = table.Column<bool>(type: "boolean", nullable: false),
                    DurationUnits = table.Column<string>(type: "text", nullable: false),
                    DurationValue = table.Column<int>(type: "integer", nullable: true),
                    RequiresConcentration = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spells", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    TokenHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByIp = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Revoked = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RevokedByIp = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ReplacedByTokenHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_Name",
                table: "Items",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_TokenHash",
                table: "RefreshTokens",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserID",
                table: "RefreshTokens",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Spells_Name",
                table: "Spells",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Spells");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
