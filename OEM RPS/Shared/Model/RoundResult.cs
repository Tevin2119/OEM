using OEM_RPS.Shared.Enums;

namespace OEM_RPS.Shared.Model
{
    public class RoundResult: BaseEntity
    {
        public PositionEnum Player1Choice { get; set; }
        public PositionEnum Player2Choice { get; set; }
        public WinnerEnum Winner { get; set; }

        // Add a foreign key property to reference the RPSGame
        public int RPSGameId { get; set; }
    }
}