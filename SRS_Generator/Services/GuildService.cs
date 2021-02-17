using SRS_Generator.Data;
using SRS_Generator.Helpers;
using SRS_Generator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRS_Generator.Services
{
    public class GuildService : IGuildService
    {
        private readonly GuildContext _context;

        public GuildService(
            GuildContext context)
        {
            _context = context;
        }

        public async Task CreateGuild(GuildViewModel guild)
        {
            var newGuild = guild.MapToEntity();

            _context.Guilds.Add(newGuild);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }

    public interface IGuildService
    {
        Task CreateGuild(GuildViewModel guild);
    }
}
