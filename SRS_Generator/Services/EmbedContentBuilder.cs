using SRS_Generator.Helpers;
using SRS_Generator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRS_Generator.Services
{
    public class EmbedContentBuilder : IEmbedContentBuilder
    {
        public string BuildGuildList(ICollection<GuildViewModel> guilds)
        {
            string list = "";
            int ctr = 1;

            if (guilds.Count == 0)
            {
                return "\n\nNothing to display.";
            }

            foreach (var guild in guilds)
            {
                list += $"\n{ctr.ToString()}. {guild.Name.ToBold()} - {guild.MemberCount}/20 ({guild.OpenSpots} open spots)";
                ctr++;
            }

            return $"\n{list}";
        }

        public string BuildMemberList(ICollection<GuildMemberViewModel> members)
        {
            string list = "";
            int ctr = 1;

            if (members.Count == 0)
            {
                return "\n\nNothing to display.";
            }

            var membersWithGuild = members
                .Where(x => x.Guild != null)
                .ToList();
            var membersWithoutGuild = members
                .Where(x => x.Guild == null)
                .ToList();

            if (membersWithGuild.Count > 0)
            {
                membersWithGuild = membersWithGuild
                    .OrderBy(x => x.Guild.Name)
                    .ThenBy(x => x.Username)
                    .ToList();
            }
            if (membersWithoutGuild.Count > 0)
            {
                membersWithoutGuild = membersWithoutGuild
                    .OrderBy(x => x.Username)
                    .ToList();
            }

            var memberList = new List<GuildMemberViewModel>();
            memberList.AddRange(membersWithGuild);
            memberList.AddRange(membersWithoutGuild);

            foreach (var member in memberList)
            {
                if (member.Guild != null)
                {
                    //list += $"\n#{ctr.ToString().ToBold()} - ({member.Guild.Name.ToBold()}) {member.FullUsername()}";
                    list += $"\n#{ctr.ToString().ToBold()} - ({member.Guild.Name.ToBold()}) {member.DisplayName}";
                }
                else
                {
                    //list += $"\n#{ctr.ToString().ToBold()} - {member.FullUsername()}";
                    list += $"\n#{ctr.ToString().ToBold()} - {member.DisplayName}";
                }

                ctr++;
            }

            return $"\n{list}";
        }

        public string BuildSwitchRequestList(ICollection<SwitchRequestViewModel> switchRequests)
        {
            string list = "\n";
            int ctr = 1;

            if (switchRequests.Count == 0)
            {
                return "\n\nNothing to display.";
            }

            foreach (var switchRequest in switchRequests)
            {
                var sourceGuild = switchRequest.SourceGuild != null ? switchRequest.SourceGuild.Name : "None";
                var requestedBy = switchRequest.RequestedBy;

                list += /*$"\n#{ctr.ToString().ToBold()}:" +*/
                    //$"\n{requestedBy.DisplayName} ({requestedBy.FullUsername()})" +
                    $"\n{requestedBy.DisplayName}" +
                    $"\n{sourceGuild} → {switchRequest.TargetGuild.Name}" +
                    //$"\n**From**: {sourceGuild}" +
                    //$"\n**To**: {switchRequest.TargetGuild.Name}" +
                    //$"\n**Status**: {switchRequest.Status}" +
                    $"\n";
                ctr++;
            }

            return $"\n{list}";
        }

        public string BuildSwitchRequestSummary(ICollection<GuildViewModel> guilds, ICollection<SwitchRequestViewModel> switchRequests)
        {
            string summary = "\n";

            foreach (var guild in guilds)
            {
                var requestsByTarget = switchRequests.Where(x => x.TargetGuild.Name == guild.Name).ToList();
                var incomingUserList = requestsByTarget.Select(x => x.RequestedBy.DisplayName).ToList();
                var incomingUsers = string.Join(", ", incomingUserList);

                var requestsBySource = switchRequests.Where(x => x.SourceGuild?.Name == guild.Name).ToList();
                var outgoingUserList = requestsBySource.Select(x => x.RequestedBy.DisplayName).ToList();
                var outgoingUsers = string.Join(", ", outgoingUserList);

                var openSpotCount = guild.OpenSpots - incomingUserList.Count + outgoingUserList.Count;
                var openSpots = "";
                if (openSpotCount < 0)
                {
                    openSpots = $"\nFull ({Math.Abs(openSpotCount)} open spot(s) needed)";
                }
                else if (openSpotCount > 0)
                {
                    openSpots = $"\n{openSpotCount} open spot(s)";
                }
                else
                {
                    openSpots = $"\nFull";
                }
                
                summary +=
                    $"\n**{guild.Name} Summary**" +
                    $"\nIn: {incomingUsers}" +
                    $"\nOut: {outgoingUsers}" +
                    openSpots +
                    $"\n";
            }

            return summary;
        }
    }

    public interface IEmbedContentBuilder
    {
        string BuildGuildList(ICollection<GuildViewModel> guilds);
        string BuildMemberList(ICollection<GuildMemberViewModel> members);
        string BuildSwitchRequestList(ICollection<SwitchRequestViewModel> switchRequests);
        string BuildSwitchRequestSummary(ICollection<GuildViewModel> guilds, ICollection<SwitchRequestViewModel> switchRequests);
    }
}
