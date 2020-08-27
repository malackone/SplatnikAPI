using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text;

namespace Splatnik.Data.Database.DbModels
{
	public class Budget
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }

		[Required]
		public DateTime UpdatedAt { get; set; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		[Required]
		[MaxLength(200)]
		public string Description { get; set; }

		[Required]
		[MaxLength(450)]
		public string UserId { get; set; }

		[ForeignKey(nameof(UserId))]
		public IdentityUser User { get; set; }

		public virtual List<Period> Periods { get; set; }
	}
}
