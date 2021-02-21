using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace SRS_Generator.Commands
{
    public class TestCommand : BaseCommandModule
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

        [Command("pun")]
        //[Description("Returns user information")]
        public async Task SayPun(CommandContext ctx)
        {
            var message = "Dragon deez nuts. -Aerin";
            await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
        }

        [Command("gm")]
        public async Task GetMessages(CommandContext ctx, int limit)
        {
            var messages = await ctx.Channel.GetMessagesAsync(limit).ConfigureAwait(false);

            return;
        }
    }
}
