using SRS_Generator.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SRS_Generator.Models
{
    public class SwitchRequest : BaseEntity
    {
        public GuildMember RequestedBy { get; set; }
        public Guild SourceGuild { get; set; }
        public Guild TargetGuild { get; set; }
        public SwitchRequestStatus Status { get; set; } = SwitchRequestStatus.Active;
        public string ApprovedBy { get; set; }
    }

    public enum SwitchRequestStatus
    {
        [Description("Active")]
        Active,
        [Description("Cancelled")]
        Cancelled,
        [Description("Approved")]
        Approved,
        [Description("Rejected")]
        Rejected
    }
}
