using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using SRS_Generator.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SRS_Generator.Infrastructure
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public Bot(
            IServiceProvider services,
            string token,
            List<string> prefixes
            )
        {
            var config = new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
                //Intents = DiscordIntents.GuildMembers
            };

            Client = new DiscordClient(config);
            Client.Ready += OnClientReady;

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = prefixes,
                EnableMentionPrefix = true,
                DmHelp = true,
                Services = services
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<TestCommand>();
            Commands.RegisterCommands<GuildMemberCommands>();
            Commands.RegisterCommands<GuildCommands>();

            Client.ConnectAsync();
        }

        private Task OnClientReady(DiscordClient d, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
