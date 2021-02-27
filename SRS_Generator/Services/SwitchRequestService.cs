using Microsoft.EntityFrameworkCore;
using SRS_Generator.Data;
using SRS_Generator.Helpers;
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

        public async Task<SwitchRequestViewModel> CreateSwitchRequest(SwitchRequestViewModel switchRequest)
        {
            var newSwitchRequest = switchRequest.MapToEntity();

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
                .Where(x => !x.IsApproved)
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
        Task<SwitchRequestViewModel> CreateSwitchRequest(SwitchRequestViewModel switchRequest);
    }
}
