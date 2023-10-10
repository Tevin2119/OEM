using System;
using System.ComponentModel.DataAnnotations;

namespace OEM_RPS.Shared.Model
{
	public abstract class BaseEntity
	{
		[Key]
		public int Id { get; set; }
		public DateTime createdAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}

