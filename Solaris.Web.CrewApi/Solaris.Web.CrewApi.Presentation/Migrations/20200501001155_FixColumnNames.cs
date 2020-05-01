using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Solaris.Web.CrewApi.Presentation.Migrations
{
    public partial class FixColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shuttles_ExplorersTeams_ExploreresTeamId",
                table: "Shuttles");

            migrationBuilder.DropIndex(
                name: "IX_Shuttles_ExploreresTeamId",
                table: "Shuttles");

            migrationBuilder.DropColumn(
                name: "ExploreresTeamId",
                table: "Shuttles");

            migrationBuilder.AddColumn<Guid>(
                name: "ExplorersTeamId",
                table: "Shuttles",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Shuttles_ExplorersTeamId",
                table: "Shuttles",
                column: "ExplorersTeamId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shuttles_ExplorersTeams_ExplorersTeamId",
                table: "Shuttles",
                column: "ExplorersTeamId",
                principalTable: "ExplorersTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shuttles_ExplorersTeams_ExplorersTeamId",
                table: "Shuttles");

            migrationBuilder.DropIndex(
                name: "IX_Shuttles_ExplorersTeamId",
                table: "Shuttles");

            migrationBuilder.DropColumn(
                name: "ExplorersTeamId",
                table: "Shuttles");

            migrationBuilder.AddColumn<Guid>(
                name: "ExploreresTeamId",
                table: "Shuttles",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Shuttles_ExploreresTeamId",
                table: "Shuttles",
                column: "ExploreresTeamId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shuttles_ExplorersTeams_ExploreresTeamId",
                table: "Shuttles",
                column: "ExploreresTeamId",
                principalTable: "ExplorersTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
