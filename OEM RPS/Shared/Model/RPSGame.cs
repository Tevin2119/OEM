using System.ComponentModel.DataAnnotations;
using OEM_RPS.Shared.Model;

namespace OEM_RPS.Shared
{
	public class RPSGame: BaseEntity
	{
        [Required]
        [MaxLength(25)]
        public string Player1 { get; set; }

        [Required]
        [MaxLength(25)]
        public string Player2 { get; set; }

        [Required]
        [Range(1, 10)]
        public int RoundsToWin { get; set; } // The number of rounds needed to win the game

        public List<RoundResult> RoundResults { get; set; } // List to store results of each round

        public bool Closed { get; set; }

        //by default the Player2 will draw at random, only when the player request to memorize moves will this affect the moveSet
        public bool RandomMode { get; set; }

        public RPSGame()
        {
            // Initialize the list of round results
            RoundResults = new List<RoundResult>();
            Player1 = "P1";
            Player2 = "CPU";
        }
    }
}

