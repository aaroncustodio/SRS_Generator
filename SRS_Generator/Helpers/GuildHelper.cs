using SRS_Generator.Models;
using SRS_Generator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRS_Generator.Helpers
{
    public static class GuildHelper
    {
        private static int _guildCapacity = 20;

        #region MapFromEntity
        public static GuildViewModel MapFromEntity(this Guild source)
        {
            if (source == null)
            {
                return null;
            }

            var members = source.Members;

            return new GuildViewModel
            {
                Name = source.Name,
                Members = source.Members.Select(x => x.MapFromEntity()).ToList(),
                MemberCount = members.Count(),
                OpenSpots = _guildCapacity - members.Count()
            };
        }

        public static GuildMemberViewModel MapFromEntity(this GuildMember source)
        {
            if (source == null)
            {
                return null;
            }

            return new GuildMemberViewModel
            {
                DiscordId = source.DiscordId,
                Discriminator = source.Discriminator,
                Username = source.Username
            };
        }
        #endregion

        #region MapToEntity
        public static Guild MapToEntity(this GuildViewModel source)
        {
            if (source == null)
            {
                return null;
            }

            return new Guild
            {
                //Id = new Guid(),
                //DateCreated = DateTime.UtcNow,
                //DateUpdated = DateTime.UtcNow
                Name = source.Name,
                IsActive = source.IsActive,
                IsFarmingGuild = source.IsFarmingGuild,
                //Members = source.Members.Select(x => x.MapToEntity()).ToList(),
            };
        }

        public static GuildMember MapToEntity(this GuildMemberViewModel source)
        {
            if (source == null)
            {
                return null;
            }

            return new GuildMember
            {
                DiscordId = source.DiscordId,
                Discriminator = source.Discriminator,
                Username = source.Username,
                //Guild = source.Guild?.MapToEntity(),
                IsGuildMaster = source.IsGuildMaster
            };
        }

        public static SwitchRequest MapToEntity(this SwitchRequestViewModel source)
        {
            if (source == null)
            {
                return null;
            }

            return new SwitchRequest
            {
                //Id = new Guid(),
                //DateCreated = DateTime.UtcNow,
                //DateUpdated = DateTime.UtcNow,
                IsApproved = source.IsApproved
            };
        }
        #endregion
    }
}
