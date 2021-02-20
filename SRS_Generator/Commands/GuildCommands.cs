using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SRS_Generator.Services;
using SRS_Generator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRS_Generator.Commands
{
    public class GuildCommands : BaseCommandModule
    {
        private readonly IGuildService _guildService;

        public GuildCommands(
            IGuildService guildService)
        {
            _guildService = guildService;
        }

        [Command("add-guild")]
        [Description("")]
        //[RequireRoles(RoleCheckMode.Any, "ADMIN")]
        public async Task CreateGuild(CommandContext ctx, string name, bool isActive, bool isFarming)
        {
            try
            {
                var guild = new GuildViewModel();
                guild.Name = name;
                guild.IsActive = isActive;
                guild.IsFarmingGuild = isFarming;

                await _guildService.CreateGuild(guild);

                await ctx.Channel.SendMessageAsync("Guild added").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string message = $"Create guild failed. Error message: {ex.Message}";
                await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
            }
        }

        [Command("list-guilds")]
        [Description("")]
        //[RequireRoles(RoleCheckMode.Any, "ADMIN")]
        public async Task ListGuilds(CommandContext ctx)
        {

        }

        [Command("guild-info")]
        [Description("")]
        //[RequireRoles(RoleCheckMode.Any, "ADMIN")]
        public async Task ViewGuildInfo(CommandContext ctx, string guildName)
        {
            try
            {
                var guild = await _guildService.GetGuild(guildName);

                var embed = new DiscordEmbedBuilder();
                embed.Title = "Guild Information";
                embed.AddField("Name", guild.Name, true);
                embed.AddField("Status", guild.Status(), true);
                embed.AddField("Member count", guild.MemberCount.ToString());
                embed.AddField("Open spots", guild.OpenSpots.ToString());
                
                embed.Build();

                await ctx.Channel.SendMessageAsync(embed).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await ctx.Channel.SendMessageAsync(ex.Message).ConfigureAwait(false);
            }
        }
    }
}
