using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRS_Generator.ViewModels
{
    public class SwitchRequestViewModel
    {
        public GuildMemberViewModel RequestedBy { get; set; }
        public GuildViewModel SourceGuild { get; set; }
        public GuildViewModel TargetGuild { get; set; }
        public bool IsApproved { get; set; }
        public string ApprovedBy { get; set; }
    }
}
