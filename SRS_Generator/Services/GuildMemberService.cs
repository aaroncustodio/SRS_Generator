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

        public async Task CreateMember(DiscordUser mentionedUser)
        {
            if (mentionedUser == null)
            {
                throw new Exception("Please mention the user to be added.");
            }

            var memberId = mentionedUser.Id.ToString();
            var member = await _context.GuildMembers.FirstOrDefaultAsync(x => x.DiscordId == memberId).ConfigureAwait(false);
            //var query = _context.GuildMembers.AsQueryable();
            //var memberList = query.Where().ToListAsync();
            if (member != null)
            {
                throw new Exception("Member already exists!");
            }

            var newMember = new GuildMemberViewModel
            {
                DiscordId = mentionedUser.Id.ToString(),
                Username = mentionedUser.Username,
                Discriminator = mentionedUser.Discriminator
            };

            var memberEntity = newMember.MapToEntity();

            _context.GuildMembers.Add(memberEntity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }

    public interface IGuildMemberService
    {
        Task CreateMember(DiscordUser mentionedUser);
    }
}
