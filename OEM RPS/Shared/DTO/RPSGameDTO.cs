using System;
using OEM_RPS.Shared.Enums;

namespace OEM_RPS.Shared.DTO
{
	public class RPSGameDTO
	{
		public int GameID { get; set; }

		public string PlayerName { get; set; }

		public Position Choice { get; set; }

		public bool RandomMode { get; set; }
	}
}

