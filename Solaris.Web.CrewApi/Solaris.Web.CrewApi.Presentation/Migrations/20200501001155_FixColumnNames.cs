using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Solaris.Web.CrewApi.Presentation.Migrations
{
    public partial class FixColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Shuttles_ExplorersTeams_ExploreresTeamId",
                "Shuttles");

            migrationBuilder.DropIndex(
                "IX_Shuttles_ExploreresTeamId",
                "Shuttles");

            migrationBuilder.DropColumn(
                "ExploreresTeamId",
                "Shuttles");

            migrationBuilder.AddColumn<Guid>(
                "ExplorersTeamId",
                "Shuttles",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                "IX_Shuttles_ExplorersTeamId",
                "Shuttles",
                "ExplorersTeamId",
                unique: true);

            migrationBuilder.AddForeignKey(
                "FK_Shuttles_ExplorersTeams_ExplorersTeamId",
                "Shuttles",
                "ExplorersTeamId",
                "ExplorersTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Shuttles_ExplorersTeams_ExplorersTeamId",
                "Shuttles");

            migrationBuilder.DropIndex(
                "IX_Shuttles_ExplorersTeamId",
                "Shuttles");

            migrationBuilder.DropColumn(
                "ExplorersTeamId",
                "Shuttles");

            migrationBuilder.AddColumn<Guid>(
                "ExploreresTeamId",
                "Shuttles",
                "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                "IX_Shuttles_ExploreresTeamId",
                "Shuttles",
                "ExploreresTeamId",
                unique: true);

            migrationBuilder.AddForeignKey(
                "FK_Shuttles_ExplorersTeams_ExploreresTeamId",
                "Shuttles",
                "ExploreresTeamId",
                "ExplorersTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
