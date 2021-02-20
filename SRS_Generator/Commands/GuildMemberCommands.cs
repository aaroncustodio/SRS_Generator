using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
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

        public GuildMemberCommands(
            IGuildMemberService guildMemberService)
        {
            _guildMemberService = guildMemberService;
        }

        [Command("create-member")]
        [Description("")]
        //[RequireRoles(RoleCheckMode.Any, "ADMIN")]
        public async Task CreateGuildMember(CommandContext ctx, string mention)
        {
            try
            {
                var mentionedUser = ctx.Message.MentionedUsers.FirstOrDefault();

                await _guildMemberService.CreateMember(mentionedUser);

                var responseMessage = $"Member added: {mentionedUser.Username}";
                await ctx.Channel.SendMessageAsync(responseMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await ctx.Channel.SendMessageAsync(ex.Message).ConfigureAwait(false);
            }
        }
    }
}
