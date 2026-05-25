using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dnd_assistant.Migrations
{
    /// <inheritdoc />
    public partial class MVP2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Worlds",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Pitch = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "jsonb", nullable: true),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worlds", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Worlds_Users_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatorID = table.Column<Guid>(type: "uuid", nullable: false),
                    WorldID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    System = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Pitch = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "jsonb", nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    SessionNumber = table.Column<int>(type: "integer", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Campaigns_Users_CreatorID",
                        column: x => x.CreatorID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Campaigns_Worlds_WorldID",
                        column: x => x.WorldID,
                        principalTable: "Worlds",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    WorldID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatorID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Pitch = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "jsonb", nullable: true),
                    Time = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsSecret = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Events_Worlds_WorldID",
                        column: x => x.WorldID,
                        principalTable: "Worlds",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    WorldID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatorID = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentLocationID = table.Column<Guid>(type: "uuid", nullable: true),
                    X = table.Column<double>(type: "double precision", nullable: true),
                    Y = table.Column<double>(type: "double precision", nullable: true),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Pitch = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "jsonb", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Locations_Locations_ParentLocationID",
                        column: x => x.ParentLocationID,
                        principalTable: "Locations",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Locations_Worlds_WorldID",
                        column: x => x.WorldID,
                        principalTable: "Worlds",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorldAccess",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    WorldID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorldAccess", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorldAccess_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorldAccess_Worlds_WorldID",
                        column: x => x.WorldID,
                        principalTable: "Worlds",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignAccess",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    CampaignID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignAccess", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CampaignAccess_Campaigns_CampaignID",
                        column: x => x.CampaignID,
                        principalTable: "Campaigns",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignAccess_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatorID = table.Column<Guid>(type: "uuid", nullable: false),
                    CampaignID = table.Column<Guid>(type: "uuid", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Pitch = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "jsonb", nullable: true),
                    IsSecret = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Sessions_Campaigns_CampaignID",
                        column: x => x.CampaignID,
                        principalTable: "Campaigns",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Organisations",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    WorldID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatorID = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationID = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Pitch = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "jsonb", nullable: true),
                    IsSecret = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Organisations_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Organisations_Worlds_WorldID",
                        column: x => x.WorldID,
                        principalTable: "Worlds",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NPCs",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    WorldID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatorID = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationID = table.Column<Guid>(type: "uuid", nullable: true),
                    OrganisationID = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SpeciesID = table.Column<Guid>(type: "uuid", nullable: false),
                    Pitch = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "jsonb", nullable: true),
                    IsSecret = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPCs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NPCs_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_NPCs_Organisations_OrganisationID",
                        column: x => x.OrganisationID,
                        principalTable: "Organisations",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_NPCs_Species_SpeciesID",
                        column: x => x.SpeciesID,
                        principalTable: "Species",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NPCs_Worlds_WorldID",
                        column: x => x.WorldID,
                        principalTable: "Worlds",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignAccess_CampaignID_UserID",
                table: "CampaignAccess",
                columns: new[] { "CampaignID", "UserID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CampaignAccess_UserID",
                table: "CampaignAccess",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_CreatorID",
                table: "Campaigns",
                column: "CreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_WorldID",
                table: "Campaigns",
                column: "WorldID");

            migrationBuilder.CreateIndex(
                name: "IX_Events_WorldID",
                table: "Events",
                column: "WorldID");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ParentLocationID",
                table: "Locations",
                column: "ParentLocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_WorldID",
                table: "Locations",
                column: "WorldID");

            migrationBuilder.CreateIndex(
                name: "IX_NPCs_LocationID",
                table: "NPCs",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_NPCs_OrganisationID",
                table: "NPCs",
                column: "OrganisationID");

            migrationBuilder.CreateIndex(
                name: "IX_NPCs_SpeciesID",
                table: "NPCs",
                column: "SpeciesID");

            migrationBuilder.CreateIndex(
                name: "IX_NPCs_WorldID",
                table: "NPCs",
                column: "WorldID");

            migrationBuilder.CreateIndex(
                name: "IX_Organisations_LocationID",
                table: "Organisations",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Organisations_WorldID",
                table: "Organisations",
                column: "WorldID");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_CampaignID_Order",
                table: "Sessions",
                columns: new[] { "CampaignID", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorldAccess_UserID",
                table: "WorldAccess",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_WorldAccess_WorldID_UserID",
                table: "WorldAccess",
                columns: new[] { "WorldID", "UserID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Worlds_OwnerID",
                table: "Worlds",
                column: "OwnerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignAccess");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "NPCs");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "WorldAccess");

            migrationBuilder.DropTable(
                name: "Organisations");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Worlds");
        }
    }
}
