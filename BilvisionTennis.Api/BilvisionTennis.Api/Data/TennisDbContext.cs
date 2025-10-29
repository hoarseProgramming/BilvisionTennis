
using Microsoft.EntityFrameworkCore;

namespace BilvisionTennisAPI.GraphQL.Data
{
    public class TennisDbContext(DbContextOptions<TennisDbContext> options) : DbContext(options)
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Rally> Rallies { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<Umpire> Umpires { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Match>().HasMany(m => m.Players).WithMany(p => p.Matches);
            modelBuilder.Entity<Match>().HasOne(m => m.Winner).WithMany(p => p.WonMatches).HasForeignKey(m => m.WInnerId);
            modelBuilder.Entity<Match>().HasOne(u => u.Umpire).WithMany(m => m.Matches).HasForeignKey(m => m.UmpireId);
        }
    }
}
