using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SRS_Generator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRS_Generator.Commands
{
    public class GuildMemberCommands : BaseCommandModule
    {
        private readonly IGuildMemberService _guildMemberService;
        private readonly IEmbedContentBuilder _embedContentBuilder;

        private const string _AddAllMembers = "add-all-members";
        private const string _AddAllMembersAlias = "aam";
        private const string _CreateGuildMember = "create-member";
        private const string _CreateGuildMemberAlias = "cm";
        private const string _DeleteGuildMember = "delete-member";
        private const string _DeleteGuildMemberAlias = "dm";
        private const string _ViewAllMembers = "list-members";
        private const string _ViewAllMembersAlias = "lm";

        public GuildMemberCommands(
            IGuildMemberService guildMemberService,
            IEmbedContentBuilder embedContentBuilder)
        {
            _guildMemberService = guildMemberService;
            _embedContentBuilder = embedContentBuilder;
        }

        [Command(_CreateGuildMember)]
        [Aliases(new string[] { _CreateGuildMemberAlias })]
        [Description("")]
        //[RequireRoles(RoleCheckMode.Any, "ADMIN")]
        public async Task CreateGuildMember(CommandContext ctx, string mention)
        {
            try
            {
                var mentionedUser = ctx.Message.MentionedUsers.FirstOrDefault();
                var user = await ctx.Guild.GetMemberAsync(mentionedUser.Id).ConfigureAwait(false);

                await _guildMemberService.CreateMember(user).ConfigureAwait(false);

                var responseMessage = $"Member created: {user.Username}";
                await ctx.Channel.SendMessageAsync(responseMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await ctx.Channel.SendMessageAsync(ex.Message).ConfigureAwait(false);
            }
        }

        [Command(_DeleteGuildMember)]
        [Aliases(new string[] { _DeleteGuildMemberAlias })]
        [Description("")]
        //[RequireRoles(RoleCheckMode.Any, "ADMIN")]
        public async Task DeleteGuildMember(CommandContext ctx, string mention)
        {
            try
            {
                var mentionedUser = ctx.Message.MentionedUsers.FirstOrDefault();
                var user = await ctx.Guild.GetMemberAsync(mentionedUser.Id).ConfigureAwait(false);

                await _guildMemberService.DeleteMember(user).ConfigureAwait(false);

                var responseMessage = $"Member deleted: {user.Username}";
                await ctx.Channel.SendMessageAsync(responseMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await ctx.Channel.SendMessageAsync(ex.Message).ConfigureAwait(false);
            }
        }

        [Command(_AddAllMembers)]
        [Aliases(new string[] { _AddAllMembersAlias })]
        [Description("")]
        //[RequireRoles(RoleCheckMode.Any, "ADMIN")]
        public async Task AddAllMembers(CommandContext ctx)
        {
            try
            {
                var users = await ctx.Guild.GetAllMembersAsync().ConfigureAwait(false);
                var userList = users.ToList();

                await _guildMemberService.AddAllUsers(userList).ConfigureAwait(false);

                var responseMessage = $"Members added successfully";
                await ctx.Channel.SendMessageAsync(responseMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await ctx.Channel.SendMessageAsync(ex.Message).ConfigureAwait(false);
            }
        }

        [Command(_ViewAllMembers)]
        [Aliases(new string[] { _ViewAllMembersAlias })]
        [Description("")]
        //[RequireRoles(RoleCheckMode.Any, "ADMIN")]
        public async Task ViewAllMembers(CommandContext ctx)
        {
            try
            {
                var members = await _guildMemberService.GetAllUsers().ConfigureAwait(false);
                var memberListString = _embedContentBuilder.BuildMemberList(members);

                var embed = new DiscordEmbedBuilder();
                embed.Title = "Members";
                embed.Description = memberListString;
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
