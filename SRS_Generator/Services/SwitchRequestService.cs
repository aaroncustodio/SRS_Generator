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
    }

    public interface ISwitchRequestService
    {
        Task<List<SwitchRequestViewModel>> GetAllSwitchRequests();
        Task<SwitchRequestViewModel> CreateSwitchRequest(Dictionary<string, string> sourceAndTarget, DiscordUser requestor);
    }
}
