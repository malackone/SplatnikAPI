using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Splatnik.Data.Database.DbModels
{
	public class Period
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }

		[Required]
		public DateTime FirstDay { get; set; }

		[Required]
		public DateTime LastDay { get; set; }

		[Required]
		[MaxLength(7)]
		public string DisplayName { get; set; }

		[MaxLength(1000)]
		public string Notes { get; set; }

		[Required]
		public int BudgetId { get; set; }

		[ForeignKey(nameof(BudgetId))]
		public Budget Budget { get; set; }

		public virtual List<Income> Incomes { get; set; }
		public virtual List<Expense> Expenses { get; set; }
	}
}
