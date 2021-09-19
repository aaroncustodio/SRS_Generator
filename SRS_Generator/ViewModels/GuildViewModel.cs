using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRS_Generator.ViewModels
{
    public class GuildViewModel
    {
        public string Name { get; set; }
        public ICollection<GuildMemberViewModel> Members { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsFarmingGuild { get; set; } = false;
        public int MemberCount { get; set; }
        public int OpenSpots { get; set; }

        public string Status()
        {
            string status = IsActive ? "Active" : "Inactive";
            return status;
        }
    }
}
