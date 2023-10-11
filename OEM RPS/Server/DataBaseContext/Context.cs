using Microsoft.EntityFrameworkCore;
using OEM_RPS.Shared;
using OEM_RPS.Shared.Model;

namespace OEMRPS.Server.DataBaseContext
{
	public class Context : DbContext
	{
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<RPSGame> RockPaperScissorsGame { get; set; }
        public DbSet<RoundResult> RockPaperScissorsRounds { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RPSGame>()
                .Property(e => e.Player1)
                .IsRequired();

            modelBuilder.Entity<RPSGame>()
                .Property(e => e.Player2)
                .IsRequired();

            // Other entity configurations
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var currentTime = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity baseEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            baseEntity.createdAt = currentTime;
                            //baseEntity.UpdatedAt = currentTime;
                        break;

                        case EntityState.Modified:
                            entry.Property(nameof(baseEntity.createdAt)).IsModified = false; // Don't update createdAt on modification
                            baseEntity.UpdatedAt = currentTime;
                        break;
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }


    }
}

