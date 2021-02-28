﻿using DSharpPlus.CommandsNext;
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
    public class SwitchRequestCommands : BaseCommandModule
    {
        private readonly ISwitchRequestService _switchRequestService;
        private readonly IGuildService _guildService;
        private readonly IGuildMemberService _guildMemberService;
        private readonly IEmbedContentBuilder _embedContentBuilder;

        private const string _CreateSwitchRequest = "create-switch-request";
        private const string _CreateSwitchRequestAlias = "csr";
        private const string _ListSwitchRequests = "list-switch-requests";
        private const string _ListSwitchRequestsAlias = "lsr";

        public SwitchRequestCommands(
            ISwitchRequestService switchRequestService,
            IGuildService guildService,
            IGuildMemberService guildMemberService,
            IEmbedContentBuilder embedContentBuilder)
        {
            _switchRequestService = switchRequestService;
            _guildService = guildService;
            _guildMemberService = guildMemberService;
            _embedContentBuilder = embedContentBuilder;
        }

        [Command(_CreateSwitchRequest)]
        [Aliases(new string[] { _CreateSwitchRequestAlias })]
        [Description("")]
        //[RequireRoles(RoleCheckMode.Any, "ADMIN")]
        public async Task CreateSwitchRequest(CommandContext ctx, string source, string target, [RemainingText] string mentionedUser = null)
        {
            try
            {
                var guildNames = new List<string> { target };
                if (source != "--") guildNames.Add(source);
                
                DiscordUser requestor = null;
                var mentions = ctx.Message.MentionedUsers;
                if (mentions.Count > 0)
                {
                    requestor = mentions.FirstOrDefault();
                }
                else
                {
                    requestor = ctx.Message.Author;
                }

                await _guildMemberService.CheckIfUsersExist(new List<DiscordUser> { requestor });
                await _guildService.CheckIfGuildsExist(guildNames);

                var sourceAndTarget = new Dictionary<string, string>
                {
                    { "source", source },
                    { "target", target }
                };

                var switchRequest = await _switchRequestService.CreateSwitchRequest(sourceAndTarget, requestor);

                await ctx.Channel.SendMessageAsync("Switch request created.").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await ctx.Channel.SendMessageAsync(ex.Message).ConfigureAwait(false);
            }
        }

        [Command(_ListSwitchRequests)]
        [Aliases(new string[] { _ListSwitchRequestsAlias })]
        [Description("")]
        //[RequireRoles(RoleCheckMode.Any, "ADMIN")]
        public async Task ListSwitchRequests(CommandContext ctx)
        {
            try
            {
                var switchRequestList = await _switchRequestService.GetAllSwitchRequests().ConfigureAwait(false);
                var switchRequestString = _embedContentBuilder.BuildSwitchRequestList(switchRequestList);

                var embed = new DiscordEmbedBuilder();
                embed.Title = "Switch Requests";
                embed.Description = switchRequestString;

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
