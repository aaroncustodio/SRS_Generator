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
            var member = await _context.GuildMembers
				.FirstOrDefaultAsync(x => x.DiscordId == memberId)
				.ConfigureAwait(false);

            if (member != null)
            {
                throw new Exception("Member already exists.");
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
            var member = await _context.GuildMembers
				.FirstOrDefaultAsync(x => x.DiscordId == memberId)
				.ConfigureAwait(false);

            if (member == null)
            {
                throw new Exception("Member does not exist.");
            }

            _context.GuildMembers.Remove(member);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return;
        }

        public async Task<GuildMemberViewModel> GetUser(DiscordUser discordUser)
        {
            await CheckIfUsersExist(new List<DiscordUser> { discordUser });

            var user = await _context.GuildMembers
                .Include(x => x.Guild)
                .FirstOrDefaultAsync(x => x.DiscordId == discordUser.Id.ToString())
                .ConfigureAwait(false);

            var result = user.MapFromEntity();

            return result;
        }

        public async Task<List<GuildMemberViewModel>> GetUsers(List<DiscordUser> discordUsers)
        {
            await CheckIfUsersExist(discordUsers);

            var users = await _context.GuildMembers
                .Include(x => x.Guild)
                .Where(x => discordUsers.Any(y => y.Id.ToString() == x.DiscordId))
                .ToListAsync()
                .ConfigureAwait(false);

            var userList = users.Select(x => x.MapFromEntity()).ToList();

            return userList;
        }

        public async Task<List<GuildMemberViewModel>> GetAllUsers()
        {
            var guildMembers = await _context.GuildMembers
                .Include(x => x.Guild)
                .ToListAsync()
                .ConfigureAwait(false);
            var guildMemberList = guildMembers.Select(x => x.MapFromEntity()).ToList();

            return guildMemberList;
        }

        public async Task AddAllUsers(List<DiscordMember> users)
        {
            var userIds = users.Select(x => x.Id.ToString()).ToList();
            var existingUsers = await _context.GuildMembers
                .Where(x => userIds.Any(id => id == x.DiscordId))
				.ToListAsync()
				.ConfigureAwait(false);

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

        private async Task<bool> CheckIfUsersExist(List<DiscordUser> discordUsers)
        {
            bool usersExist = true;
            var nonExistentUsers = new List<string>();

            foreach (var user in discordUsers)
            {
                bool userExist = await _context.GuildMembers
                    .AnyAsync(x => x.DiscordId == user.Id.ToString())
                    .ConfigureAwait(false);

                if (!userExist)
                {
                    nonExistentUsers.Add(user.Username);
                    usersExist = false;
                }
            }

            if (!usersExist)
            {
                var users = string.Join(", ", nonExistentUsers);
                throw new Exception($"No such user(s): {users}");
            }

            return usersExist;
        }
    }

    public interface IGuildMemberService
    {
        Task CreateMember(DiscordUser user);
        Task DeleteMember(DiscordUser user);
        Task<GuildMemberViewModel> GetUser(DiscordUser discordUser);
        Task<List<GuildMemberViewModel>> GetUsers(List<DiscordUser> discordUsers);
        Task<List<GuildMemberViewModel>> GetAllUsers();
        Task AddAllUsers(List<DiscordMember> users);
    }
}
