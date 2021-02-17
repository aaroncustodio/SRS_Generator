using System.Collections.Generic;

namespace SRS_Generator.Models
{
    public class GuildMember
    {
        public string DiscordId { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public Guild Guild { get; set; }
        public ICollection<SwitchRequest> SwitchRequests { get; set; }
        public bool IsGuildMaster { get; set; }
    }
}
