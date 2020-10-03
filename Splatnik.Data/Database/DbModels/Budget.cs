using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Splatnik.Data.Domain;

namespace Splatnik.Data.Database.DbModels
{
	public class Budget : BaseEntity
	{
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
		public virtual List<Credit> Credits { get; set; }
		public virtual List<Debt> Debts { get; set; }
	}
}
