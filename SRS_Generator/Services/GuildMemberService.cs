using DSharpPlus.Entities;
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
    public class GuildMemberService : IGuildMemberService
    {
        private readonly GuildContext _context;

        public GuildMemberService(
            GuildContext context)
        {
            _context = context;
        }

        public async Task CreateMember(DiscordUser user)
        {
            if (user == null)
            {
                throw new Exception("Please mention the user to be added.");
            }

            var memberId = user.Id.ToString();
            var member = await _context.GuildMembers.FirstOrDefaultAsync(x => x.DiscordId == memberId).ConfigureAwait(false);
            //var query = _context.GuildMembers.AsQueryable();
            //var memberList = query.Where().ToListAsync();
            if (member != null)
            {
                throw new Exception("Member already exists!");
            }

            var newMember = new GuildMemberViewModel
            {
                DiscordId = user.Id.ToString(),
                Username = user.Username,
                Discriminator = user.Discriminator
            };

            var memberEntity = newMember.MapToEntity();

            _context.GuildMembers.Add(memberEntity);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return;
        }

        public async Task DeleteMember(DiscordUser user)
        {
            if (user == null)
            {
                throw new Exception("Please mention the user to be deleted.");
            }

            var memberId = user.Id.ToString();
            var member = await _context.GuildMembers.FirstOrDefaultAsync(x => x.DiscordId == memberId).ConfigureAwait(false);
            //var query = _context.GuildMembers.AsQueryable();
            //var memberList = query.Where().ToListAsync();
            if (member == null)
            {
                throw new Exception("Member does not exist!");
            }

            _context.GuildMembers.Remove(member);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return;
        }

        public async Task<List<GuildMemberViewModel>> GetAllUsers()
        {
            var guildMembers = await _context.GuildMembers.ToListAsync().ConfigureAwait(false);
            var guildMemberList = guildMembers.Select(x => x.MapFromEntity()).ToList();

            return guildMemberList;
        }

        public async Task AddAllUsers(List<DiscordMember> users)
        {
            var userIds = users.Select(x => x.Id.ToString()).ToList();
            var existingUsers = await _context.GuildMembers
                .Where(x => userIds.Any(id => id == x.DiscordId)).ToListAsync().ConfigureAwait(false);

            var newUserList = users.Where(x => existingUsers.All(existing => existing.DiscordId != x.Id.ToString())).ToList();

            foreach (var newUser in newUserList)
            {
                var newMember = new GuildMemberViewModel
                {
                    DiscordId = newUser.Id.ToString(),
                    Username = newUser.Username,
                    Discriminator = newUser.Discriminator
                };

                var memberEntity = newMember.MapToEntity();

                _context.GuildMembers.Add(memberEntity);
            }

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return;
        }
    }

    public interface IGuildMemberService
    {
        Task CreateMember(DiscordUser user);
        Task DeleteMember(DiscordUser user);
        Task<List<GuildMemberViewModel>> GetAllUsers();
        Task AddAllUsers(List<DiscordMember> users);
    }
}
