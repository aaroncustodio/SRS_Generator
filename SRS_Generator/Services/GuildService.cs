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

        public async Task<GuildViewModel> GetGuild(string name)
        {
            var query = _context.Guilds.AsQueryable();
            query = query
                .Include(g => g.Members)
                .Where(g => g.Name.ToLower() == name.ToLower());

            var guildList = await query.ToListAsync().ConfigureAwait(false);

            if (guildList.Count == 0)
            {
                throw new Exception("Guild does not exist.");
            }

            var guild = guildList.FirstOrDefault().MapFromEntity();

            return guild;
        }

        public async Task<string> GetGuildInfo(string name)
        {
            var guild = await GetGuild(name);

            string result = "";
            return result;
        }
    }

    public interface IGuildService
    {
        Task CreateGuild(GuildViewModel guild);
        Task<GuildViewModel> GetGuild(string name);
        Task<string> GetGuildInfo(string name);
    }
}
