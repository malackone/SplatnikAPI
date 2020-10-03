using Splatnik.Data.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Splatnik.Data.Database.DbModels
{
	public class Expense : BaseEntity
	{
		[Required]
		[MaxLength(450)]
		public string UserId { get; set; }

		[Required]
		public DateTime IncomeDate { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		[MaxLength(200)]
		public string Description { get; set; }

		[Required]
		[Column(TypeName = "decimal(10, 2)")]
		public decimal ExpenseValue { get; set; }

		[Required]
		public int PeriodId { get; set; }

		[ForeignKey(nameof(PeriodId))]
		public Period Period { get; set; }


		[Required]
		public int CurrencyId { get; set; }

		[ForeignKey(nameof(CurrencyId))]
		public Currency Currency { get; set; }
	}
}
