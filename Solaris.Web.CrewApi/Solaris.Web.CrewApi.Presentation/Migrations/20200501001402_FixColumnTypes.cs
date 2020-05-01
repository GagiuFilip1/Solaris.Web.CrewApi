using Microsoft.EntityFrameworkCore.Migrations;

namespace Solaris.Web.CrewApi.Presentation.Migrations
{
    public partial class FixColumnTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                "Email",
                "CrewMembers",
                "varchar(256)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET latin1",
                oldNullable: true);

            migrationBuilder.AlterColumn<sbyte>(
                "Age",
                "CrewMembers",
                "tinyint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                "Email",
                "CrewMembers",
                "longtext CHARACTER SET latin1",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                "Age",
                "CrewMembers",
                "int",
                nullable: true,
                oldClrType: typeof(sbyte),
                oldType: "tinyint",
                oldNullable: true);
        }
    }
}
