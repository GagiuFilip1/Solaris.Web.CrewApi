using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Solaris.Web.CrewApi.Presentation.Migrations
{
    public partial class AddMainModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "ExplorersTeams",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>("varchar(256)", nullable: true),
                    DepartedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorersTeams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "CrewMembers",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>("varchar(256)", nullable: true),
                    Type = table.Column<byte>(nullable: false),
                    ExplorersTeamId = table.Column<Guid>(nullable: false),
                    Gender = table.Column<byte>(nullable: true),
                    Age = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    ProductNumber = table.Column<string>("varchar(256)", nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    CurrentPlanetId = table.Column<Guid>(nullable: true),
                    CurrentStatus = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewMembers", x => x.Id);
                    table.ForeignKey(
                        "FK_CrewMembers_ExplorersTeams_ExplorersTeamId",
                        x => x.ExplorersTeamId,
                        "ExplorersTeams",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Shuttles",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ShipNumber = table.Column<string>("varchar(16)", nullable: true),
                    Name = table.Column<string>("varchar(256)", nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ExploreresTeamId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shuttles", x => x.Id);
                    table.ForeignKey(
                        "FK_Shuttles_ExplorersTeams_ExploreresTeamId",
                        x => x.ExploreresTeamId,
                        "ExplorersTeams",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_CrewMembers_ExplorersTeamId",
                "CrewMembers",
                "ExplorersTeamId");

            migrationBuilder.CreateIndex(
                "IX_CrewMembers_Name",
                "CrewMembers",
                "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ExplorersTeams_Name",
                "ExplorersTeams",
                "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Shuttles_ExploreresTeamId",
                "Shuttles",
                "ExploreresTeamId",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Shuttles_Name",
                "Shuttles",
                "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "CrewMembers");

            migrationBuilder.DropTable(
                "Shuttles");

            migrationBuilder.DropTable(
                "ExplorersTeams");
        }
    }
}
