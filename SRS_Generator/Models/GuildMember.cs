namespace SRS_Generator.Models
{
    public class GuildMember
    {
        public string DiscordId { get; set; }
        public string Username { get; set; }
        public Guild Guild { get; set; }
        public bool IsGuildMaster { get; set; }
    }
}
