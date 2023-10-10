using OEM_RPS.Shared.Enums;
using OEM_RPS.Shared.Model;

namespace OEM_RPS.Shared
{
	public class RPSGame: BaseEntity
	{
        public string Player1 { get; set; }
        public string Player2 { get; set; }

        public int RoundsToWin { get; set; } // The number of rounds needed to win the game

        public List<RoundResult> RoundResults { get; set; } // List to store results of each round

        public bool Closed { get; set; }

        public RPSGame(string playerName, int bestOf)
        {
            // Initialize the list of round results
            RoundResults = new List<RoundResult>();

            Player1 = playerName;
            RoundsToWin = bestOf;
        }

        //check to Close method compare RoundsToWin against RoundResults when Equal game is closed

    }
}

