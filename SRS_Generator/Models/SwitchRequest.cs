using SRS_Generator.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRS_Generator.Models
{
    public class SwitchRequest : BaseEntity
    {
        public GuildMember RequestedBy { get; set; }
        public Guild SourceGuildId { get; set; }
        public Guild TargetGuildId { get; set; }
        public bool IsApproved { get; set; }
        public string ApprovedBy { get; set; }
    }
}
