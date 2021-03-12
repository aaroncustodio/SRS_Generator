using Microsoft.EntityFrameworkCore.Migrations;

namespace SRS_Generator.Migrations
{
    public partial class renamenicknamecolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nickname",
                table: "GuildMembers",
                newName: "DisplayName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DisplayName",
                table: "GuildMembers",
                newName: "Nickname");
        }
    }
}
