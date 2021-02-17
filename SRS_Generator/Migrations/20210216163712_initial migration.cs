using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SRS_Generator.Migrations
{
    public partial class initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuildMembers",
                columns: table => new
                {
                    DiscordId = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    GuildId = table.Column<Guid>(nullable: true),
                    IsGuildMaster = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildMembers", x => x.DiscordId);
                    table.ForeignKey(
                        name: "FK_GuildMembers_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildMembers_GuildId",
                table: "GuildMembers",
                column: "GuildId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildMembers");

            migrationBuilder.DropTable(
                name: "Guilds");
        }
    }
}
