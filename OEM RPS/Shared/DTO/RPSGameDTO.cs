using System;
using OEM_RPS.Shared.Enums;

namespace OEM_RPS.Shared.DTO
{
	public class RPSGameDTO
	{
        public RPSGameDTO()
        {
            PlayerName = "P1"; //default to P1
        }

        public int GameID { get; set; }

		public string PlayerName { get; set; }

		public PositionEnum Choice { get; set; }

		public int BestOf { get; set; }
	}
}