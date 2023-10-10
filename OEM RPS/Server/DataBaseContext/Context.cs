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

        public DbSet<RPSGame> RockPaperScissors { get; set; }
        public DbSet<RoundResult> RockPaperScissorsRounds { get; set; }

    }
}

