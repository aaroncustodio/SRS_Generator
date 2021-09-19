using DSharpPlus;
using DSharpPlus.Entities;

namespace SRS_Generator.Helpers
{
    public static class EmojiHelper
    {
        public static DiscordEmoji WhiteCheckMark(BaseDiscordClient client) => DiscordEmoji.FromName(client, ":white_check_mark:");
        public static DiscordEmoji X(BaseDiscordClient client) => DiscordEmoji.FromName(client, ":x:");
    }
}
