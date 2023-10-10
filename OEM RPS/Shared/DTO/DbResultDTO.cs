using System;
namespace OEM_RPS.Shared.DTO
{
	public class DbResultDTO
	{
		public DbResultDTO()
		{
		}

		public bool IsSuccess { get; set; }
		public String ErrorMessage { get; set; }
	}
}

