using System;
using System.Collections.Generic;
using System.Text;

namespace Splatnik.Data.Domain.Interfaces
{
	public interface IBaseEntity
	{
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
