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
        public string DescriptionListBuilder(ICollection<GuildMemberViewModel> members)
        {
            string list = "";
            int ctr = 1;

            if (members.Count == 0)
            {
                return "\n\nNothing to display.";
            }

            foreach (var member in members)
            {
                list += $"\n#{ctr.ToString().ToBold()} - {member.FullUsername()}";
                ctr++;
            }

            return $"\n{list}";
        }
    }

    public interface IEmbedContentBuilder
    {
        string DescriptionListBuilder(ICollection<GuildMemberViewModel> members);
    }
}
