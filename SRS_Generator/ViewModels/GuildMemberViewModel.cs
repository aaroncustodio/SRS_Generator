using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRS_Generator.ViewModels
{
    public class GuildMemberViewModel
    {
        public string DiscordId { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public GuildViewModel Guild { get; set; }
        public bool IsGuildMaster { get; set; }
        public string FullUsername()
        {
            return $"{Username}#{Discriminator}";
        }
        //public Guild Guild { get; set; }
        //public bool IsGuildMaster { get; set; }
    }
}
