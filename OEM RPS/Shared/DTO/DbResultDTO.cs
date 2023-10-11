using System;
namespace OEM_RPS.Shared.DTO
{
	public class DbResultDTO<T>
	{
		public bool IsSuccess { get; set; }
		public string? ErrorMessage { get; set; }

		public T? Entity { get; set; }
	}
}

