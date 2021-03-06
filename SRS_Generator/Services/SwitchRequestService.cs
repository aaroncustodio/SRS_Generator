using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using SRS_Generator.Data;
using SRS_Generator.Helpers;
using SRS_Generator.Models;
using SRS_Generator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRS_Generator.Services
{
    public class SwitchRequestService : ISwitchRequestService
    {
        private readonly GuildContext _context;

        public SwitchRequestService(
            GuildContext context)
        {
            _context = context;
        }

        public async Task<SwitchRequestViewModel> CreateSwitchRequest(Dictionary<string, string> sourceAndTarget, DiscordUser requestor)
        {
            var requestedBy = await _context.GuildMembers
                .FirstOrDefaultAsync(x => x.DiscordId == requestor.Id.ToString())
                .ConfigureAwait(false);
            var sourceGuild = await _context.Guilds
                .FirstOrDefaultAsync(x => x.Name.ToLower() == sourceAndTarget["source"].ToLower())
                .ConfigureAwait(false);
            var targetGuild = await _context.Guilds
                .FirstOrDefaultAsync(x => x.Name.ToLower() == sourceAndTarget["target"].ToLower())
                .ConfigureAwait(false);

            var newSwitchRequest = new SwitchRequest()
            {
                RequestedBy = requestedBy,
                SourceGuild = sourceGuild,
                TargetGuild = targetGuild
            };

            _context.SwitchRequests.Add(newSwitchRequest);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return newSwitchRequest.MapFromEntity();
        }

        public async Task<SwitchRequestViewModel> UpdateSwitchRequest(Dictionary<string, string> sourceAndTarget, string requestorId)
        {
            var switchRequests = await _context.SwitchRequests
                .Include(x => x.RequestedBy)
                .ToListAsync()
                .ConfigureAwait(false);

            var switchRequest = switchRequests.FirstOrDefault(x => x.RequestedBy.DiscordId == requestorId);

            var sourceGuild = await _context.Guilds
                .FirstOrDefaultAsync(x => x.Name.ToLower() == sourceAndTarget["source"].ToLower())
                .ConfigureAwait(false);
            var targetGuild = await _context.Guilds
                .FirstOrDefaultAsync(x => x.Name.ToLower() == sourceAndTarget["target"].ToLower())
                .ConfigureAwait(false);

            switchRequest.SourceGuild = sourceGuild;
            switchRequest.TargetGuild = targetGuild;

            _context.SwitchRequests.Update(switchRequest);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return switchRequest.MapFromEntity();
        }

        public async Task<List<SwitchRequestViewModel>> GetAllSwitchRequests()
        {
            var switchRequests = await _context.SwitchRequests
                .Include(x => x.RequestedBy)
                .Include(x => x.SourceGuild)
                .Include(x => x.TargetGuild)
                .Where(x => x.Status == SwitchRequestStatus.Active)
                .ToListAsync()
                .ConfigureAwait(false);

            if (switchRequests.Count == 0)
            {
                throw new Exception("No switch requests to display.");
            }

            var switchRequestList = switchRequests.Select(x => x.MapFromEntity()).ToList();

            return switchRequestList;
        }

        public async Task<bool> CheckIfSwitchRequestExists(DiscordUser discordUser)
        {
            var switchRequests = await _context.SwitchRequests
                .Include(x => x.RequestedBy)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            var switchRequestExists = switchRequests.Any(x => x.RequestedBy.DiscordId == discordUser.Id.ToString());

            return switchRequestExists;
        }
    }

    public interface ISwitchRequestService
    {
        Task<bool> CheckIfSwitchRequestExists(DiscordUser discordUser);
        Task<SwitchRequestViewModel> CreateSwitchRequest(Dictionary<string, string> sourceAndTarget, DiscordUser requestor);
        Task<List<SwitchRequestViewModel>> GetAllSwitchRequests();
        Task<SwitchRequestViewModel> UpdateSwitchRequest(Dictionary<string, string> sourceAndTarget, string requestorId);
    }
}
