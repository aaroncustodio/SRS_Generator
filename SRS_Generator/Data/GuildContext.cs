using Microsoft.EntityFrameworkCore;
using SRS_Generator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRS_Generator.Data
{
    public class GuildContext : DbContext
    {
        public GuildContext(DbContextOptions<GuildContext> options) : base(options)
        {

        }

        public DbSet<Guild> Guilds { get; set; }
        public DbSet<GuildMember> GuildMembers { get; set; }
        public DbSet<SwitchRequest> SwitchRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //One-to-many guild member relationship
            modelBuilder.Entity<Guild>()
                .HasMany(g => g.Members)
                .WithOne(m => m.Guild);
            modelBuilder.Entity<GuildMember>()
                .HasKey(m => m.DiscordId);
        }
    }
}
