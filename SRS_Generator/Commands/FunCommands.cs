using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SRS_Generator.Services;
using SRS_Generator.ViewModels;
using System.Threading.Tasks;

namespace SRS_Generator.Commands
{
    public class FunCommands : BaseCommandModule
    {
        [Command("ping")]
        [Description("Returns pong")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong").ConfigureAwait(false);
        }

        [Command("add")]
        [Description("Returns sum of two integers")]
        [RequireRoles(RoleCheckMode.Any, "ADMIN")]
        public async Task Ping(CommandContext ctx,
            [Description("1st number")] int x,
            [Description("2nd number")] int y)
        {
            string message = (x + y).ToString();
            await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
        }

        [Command("user-info")]
        [Description("Returns user information")]
        public async Task GetUserInfo(CommandContext ctx)
        {
            var member = ctx.Member;
            var username = member.Username;
            string message = $@"{username}
                {member.Discriminator}

                {member.DisplayName}";
            await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
        }
    }
}
