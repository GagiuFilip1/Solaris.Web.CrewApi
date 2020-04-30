using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Solaris.Web.CrewApi.Presentation.Migrations
{
    public partial class AddMainModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExplorersTeams",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "varchar(256)", nullable: true),
                    DepartedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorersTeams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CrewMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "varchar(256)", nullable: true),
                    Type = table.Column<byte>(nullable: false),
                    ExplorersTeamId = table.Column<Guid>(nullable: false),
                    Gender = table.Column<byte>(nullable: true),
                    Age = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    ProductNumber = table.Column<string>(type: "varchar(256)", nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    CurrentPlanetId = table.Column<Guid>(nullable: true),
                    CurrentStatus = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewMembers_ExplorersTeams_ExplorersTeamId",
                        column: x => x.ExplorersTeamId,
                        principalTable: "ExplorersTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shuttles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ShipNumber = table.Column<string>(type: "varchar(16)", nullable: true),
                    Name = table.Column<string>(type: "varchar(256)", nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ExploreresTeamId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shuttles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shuttles_ExplorersTeams_ExploreresTeamId",
                        column: x => x.ExploreresTeamId,
                        principalTable: "ExplorersTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrewMembers_ExplorersTeamId",
                table: "CrewMembers",
                column: "ExplorersTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewMembers_Name",
                table: "CrewMembers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExplorersTeams_Name",
                table: "ExplorersTeams",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shuttles_ExploreresTeamId",
                table: "Shuttles",
                column: "ExploreresTeamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shuttles_Name",
                table: "Shuttles",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrewMembers");

            migrationBuilder.DropTable(
                name: "Shuttles");

            migrationBuilder.DropTable(
                name: "ExplorersTeams");
        }
    }
}
