using Microsoft.EntityFrameworkCore.Migrations;

namespace SRS_Generator.Migrations
{
    public partial class addguildmembernickname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                table: "GuildMembers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nickname",
                table: "GuildMembers");
        }
    }
}
