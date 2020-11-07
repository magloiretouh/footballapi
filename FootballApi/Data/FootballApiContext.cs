using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FootballApi.Models;

namespace FootballApi.Models
{
    public class FootballApiContext : DbContext
    {
        public FootballApiContext (DbContextOptions<FootballApiContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                        .HasOne(m => m.LeavingTeam)
                        .WithMany(t => t.LeavingTransactions)
                        .HasForeignKey(m => m.LeavingTeamID)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                        .HasOne(m => m.ComingTeam)
                        .WithMany(t => t.ComingTransactions)
                        .HasForeignKey(m => m.ComingTeamID)
                        .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<FootballApi.Models.Championship> Championship { get; set; }

        public DbSet<FootballApi.Models.FootballTeam> FootballTeam { get; set; }

        public DbSet<FootballApi.Models.Stadium> Stadium { get; set; }

        public DbSet<FootballApi.Models.Player> Player { get; set; }

        public DbSet<FootballApi.Models.Transaction> Transaction { get; set; }
    }
}
