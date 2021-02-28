using Microsoft.EntityFrameworkCore.Migrations;

namespace SRS_Generator.Migrations
{
    public partial class addswitchrequeststatusenum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "SwitchRequests",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "SwitchRequests");
        }
    }
}
