using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SRS_Generator.Services;
using SRS_Generator.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SRS_Generator.Commands
{
    public class GuildCommands : BaseCommandModule
    {
        private readonly IGuildService _guildService;
        private readonly IEmbedContentBuilder _embedContentBuilder;

        private const string _AddGuildMembers = "add-guild-members";
        private const string _AddGuildMembersAlias = "agm" ;
        private const string _CreateGuild = "create-guild";
        private const string _CreateGuildAlias = "cg";
        private const string _ListGuilds = "list-guilds";
        private const string _ListGuildsAlias = "lg";
        private const string _RemoveGuildMembers = "remove-guild-memebrs";
        private const string _RemoveGuildMembersAlias = "rgm";
        private const string _ViewGuildInfo = "view-guild-info";
        private const string _ViewGuildInfoAlias = "vgi";

        public GuildCommands(
            IGuildService guildService,
            IEmbedContentBuilder embedContentBuilder)
        {
            _guildService = guildService;
            _embedContentBuilder = embedContentBuilder;
        }

        [Command(_CreateGuild)]
        [Aliases(new string[] { _CreateGuildAlias })]
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

                await ctx.Channel.SendMessageAsync("Guild created.").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string message = $"Create guild failed. Error message: {ex.Message}";
                await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
            }
        }

        [Command(_ListGuilds)]
        [Aliases(new string[] { _ListGuildsAlias })]
        [Description("")]
        //[RequireRoles(RoleCheckMode.Any, "ADMIN")]
        public async Task ListGuilds(CommandContext ctx)
        {
            try
            {
                var guildList = await _guildService.GetAllGuilds().ConfigureAwait(false);
                var guildListString = _embedContentBuilder.BuildGuildList(guildList);

                var embed = new DiscordEmbedBuilder();
                embed.Title = "Guilds";
                embed.Description = guildListString;

                embed.Build();

                await ctx.Channel.SendMessageAsync(embed).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await ctx.Channel.SendMessageAsync(ex.Message).ConfigureAwait(false);
            }
        }

        [Command(_ViewGuildInfo)]
        [Aliases(new string[] { _ViewGuildInfoAlias })]
        [Description("")]
        //[RequireRoles(RoleCheckMode.Any, "ADMIN")]
        public async Task ViewGuildInfo(CommandContext ctx, string guildName)
        {
            try
            {
                var guild = await _guildService.GetGuild(guildName);
                var memberList = _embedContentBuilder.BuildMemberList(guild.Members);

                var embed = new DiscordEmbedBuilder();
                embed.Title = "Guild Information";
                embed.AddField("Name", guild.Name, true);
                embed.AddField("Status", guild.Status(), true);
                embed.AddField("Member count", guild.MemberCount.ToString(), true);
                embed.AddField("Open spots", guild.OpenSpots.ToString(), true);
                embed.AddField("Members", memberList);

                embed.Build();

                await ctx.Channel.SendMessageAsync(embed).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await ctx.Channel.SendMessageAsync(ex.Message).ConfigureAwait(false);
            }
        }

        [Command(_AddGuildMembers)]
        [Aliases(new string[] { _AddGuildMembersAlias })]
        [Description("")]
        //[RequireRoles(RoleCheckMode.Any, "ADMIN")]
        public async Task AddGuildMembers(CommandContext ctx, string guildName, [RemainingText] string users)
        {
            try
            {
                //var guild = await _guildService.GetGuild(guildName).ConfigureAwait(false);
                var mentionedUsers = ctx.Message.MentionedUsers;
                var userIds = mentionedUsers.Select(x => x.Id.ToString()).ToList();

                var addedUsersList = await _guildService.AddMembersToGuild(guildName, userIds);

                //TODO: should only mention the members that were actually added
                var addedUsers = string.Join(", ", addedUsersList);

                string response = $"Added {addedUsers} to {guildName.ToUpper()}";
                await ctx.Channel.SendMessageAsync(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await ctx.Channel.SendMessageAsync(ex.Message).ConfigureAwait(false);
            }
        }

        [Command(_RemoveGuildMembers)]
        [Aliases(new string[] { _RemoveGuildMembersAlias })]
        [Description("")]
        //[RequireRoles(RoleCheckMode.Any, "ADMIN")]
        public async Task RemoveGuildMembers(CommandContext ctx, string guildName, [RemainingText] string users)
        {
            try
            {
                //var guild = await _guildService.GetGuild(guildName).ConfigureAwait(false);
                var mentionedUsers = ctx.Message.MentionedUsers;
                var userIds = mentionedUsers.Select(x => x.Id.ToString()).ToList();

                var removedUsersList = await _guildService.RemoveMembersFromGuild(guildName, userIds);

                //TODO: should only mention the members that were actually added
                var removedUsers = string.Join(", ", removedUsersList);

                string response = $"Removed {removedUsers} from {guildName.ToUpper()}";
                await ctx.Channel.SendMessageAsync(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await ctx.Channel.SendMessageAsync(ex.Message).ConfigureAwait(false);
            }
        }
    }
}
