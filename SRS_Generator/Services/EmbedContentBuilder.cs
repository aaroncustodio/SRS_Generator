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
                    list += $"\n#{ctr.ToString().ToBold()} - ({member.Guild.Name.ToBold()}) {member.FullUsername()}";
                }
                else
                {
                    list += $"\n#{ctr.ToString().ToBold()} - {member.FullUsername()}";
                }

                ctr++;
            }

            return $"\n{list}";
        }
    }

    public interface IEmbedContentBuilder
    {
        string BuildGuildList(ICollection<GuildViewModel> guilds);
        string BuildMemberList(ICollection<GuildMemberViewModel> members);
    }
}
