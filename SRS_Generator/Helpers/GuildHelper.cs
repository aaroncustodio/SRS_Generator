using SRS_Generator.Models;
using SRS_Generator.ViewModels;
using System.Linq;

namespace SRS_Generator.Helpers
{
    public static class GuildHelper
    {
        private static int _guildCapacity = 20;

        #region MapFromEntity
        public static GuildViewModel MapFromEntity(this Guild source, bool includeMembers = true)
        {
            if (source == null)
            {
                return null;
            }

            var result = new GuildViewModel
            {
                Name = source.Name,
                IsActive = source.IsActive,
                IsFarmingGuild = source.IsFarmingGuild,
            };

            if (includeMembers)
            {
                var members = source.Members;

                result.Members = source.Members.Select(x => x.MapFromEntity()).ToList();
                result.MemberCount = members.Count();
                result.OpenSpots = _guildCapacity - members.Count();
            }

            return result;
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
                Guild = source.Guild.MapFromEntity(false),
                Username = source.Username,
                DisplayName = source.DisplayName
            };
        }

        public static SwitchRequestViewModel MapFromEntity(this SwitchRequest source)
        {
            if (source == null)
            {
                return null;
            }

            return new SwitchRequestViewModel
            {
                RequestedBy = source.RequestedBy.MapFromEntity(),
                SourceGuild = source.SourceGuild?.MapFromEntity(false),
                TargetGuild = source.TargetGuild.MapFromEntity(false),
                Status = source.Status.GetDescription()
                //IsApproved = source.IsApproved
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
                DisplayName = source.DisplayName,
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
                SourceGuild = source.SourceGuild.MapToEntity(),
                TargetGuild = source.TargetGuild.MapToEntity(),
                RequestedBy = source.RequestedBy.MapToEntity(),
            };
        }
        #endregion
    }
}
