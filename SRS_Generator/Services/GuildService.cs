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

        public async Task<List<GuildViewModel>> GetAllGuilds()
        {
            var guilds = await _context.Guilds
                .Include(x => x.Members)
                .ToListAsync()
                .ConfigureAwait(false);

            if (guilds.Count == 0)
            {
                throw new Exception("No guilds to display.");
            }

            var guildList = guilds.Select(x => x.MapFromEntity()).ToList();

            return guildList;
        }

        public async Task<string> GetGuildInfo(string name)
        {
            var guild = await GetGuild(name);

            string result = "";
            return result;
        }

        public async Task AddMembersToGuild(string guildName, List<string> userIds)
        {
            var guild = await _context.Guilds
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Name.ToLower() == guildName.ToLower())
                .ConfigureAwait(false);

            if (guild == null)
            {
                throw new Exception("Guild does not exist.");
            }

            var userList = await _context.GuildMembers
                .Where(x => userIds.Any(id => id == x.DiscordId))
                .ToListAsync()
                .ConfigureAwait(false);

            foreach (var user in userList)
            {
                var contains = guild.Members.Any(x => x.DiscordId == user.DiscordId);

                if (!contains)
                {
                    guild.Members.Add(user);
                }
            }

            _context.Guilds.Update(guild);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return;
        }
    }

    public interface IGuildService
    {
        Task CreateGuild(GuildViewModel guild);
        Task<List<GuildViewModel>> GetAllGuilds();
        Task<GuildViewModel> GetGuild(string name);
        Task<string> GetGuildInfo(string name);
        Task AddMembersToGuild(string guildName, List<string> userIds);
    }
}
