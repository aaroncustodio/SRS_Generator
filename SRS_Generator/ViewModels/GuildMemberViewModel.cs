namespace SRS_Generator.ViewModels
{
    public class GuildMemberViewModel
    {
        public string DiscordId { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public string DisplayName { get; set; }
        public GuildViewModel Guild { get; set; }
        public bool IsGuildMaster { get; set; }
        public string FullUsername()
        {
            return $"{Username}#{Discriminator}";
        }
    }
}
