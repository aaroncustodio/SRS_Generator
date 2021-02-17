using SRS_Generator.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRS_Generator.Models
{
    public class Guild : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<GuildMember> Members { get; set; }
        public bool IsActive { get; set; }
    }
}
