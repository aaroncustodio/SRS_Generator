using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SRS_Generator.Migrations
{
    public partial class addswitchreq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFarmingGuild",
                table: "Guilds",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "GuildMembers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SwitchRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    RequestedByDiscordId = table.Column<string>(nullable: true),
                    SourceGuildIdId = table.Column<Guid>(nullable: true),
                    TargetGuildIdId = table.Column<Guid>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    ApprovedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwitchRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SwitchRequests_GuildMembers_RequestedByDiscordId",
                        column: x => x.RequestedByDiscordId,
                        principalTable: "GuildMembers",
                        principalColumn: "DiscordId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SwitchRequests_Guilds_SourceGuildIdId",
                        column: x => x.SourceGuildIdId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SwitchRequests_Guilds_TargetGuildIdId",
                        column: x => x.TargetGuildIdId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SwitchRequests_RequestedByDiscordId",
                table: "SwitchRequests",
                column: "RequestedByDiscordId");

            migrationBuilder.CreateIndex(
                name: "IX_SwitchRequests_SourceGuildIdId",
                table: "SwitchRequests",
                column: "SourceGuildIdId");

            migrationBuilder.CreateIndex(
                name: "IX_SwitchRequests_TargetGuildIdId",
                table: "SwitchRequests",
                column: "TargetGuildIdId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SwitchRequests");

            migrationBuilder.DropColumn(
                name: "IsFarmingGuild",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "GuildMembers");
        }
    }
}
