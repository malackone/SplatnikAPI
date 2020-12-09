using Splatnik.Data.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using System.Text;

namespace Splatnik.Data.Database.DbModels
{
	public class Credit : BaseEntity
	{
		[Required]
		[MaxLength(450)]
		public string UserId { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		[MaxLength(200)]
		public string Description { get; set; }

		[MaxLength(100)]
		public string BankName { get; set; }

		[MaxLength(40)]
		public string BankAccountNumber { get; set; }

		[MaxLength(100)]
		public string ContractNumber { get; set; }

		[Required]
		public int InitNoOfPayments { get; set; }

		[Required]
		[Column(TypeName = "decimal(10, 2)")]
		public decimal SinglePaymentAmount { get; set; }

		[Required]
		[Column(TypeName = "decimal(10, 2)")]
		public decimal InitialCreditValue { get; set; }

		[Required]
		public int CurrencyId { get; set; }

		[ForeignKey(nameof(CurrencyId))]
		public Currency Currency { get; set; }

		[Required]
		public int BudgetId { get; set; }

		[ForeignKey(nameof(BudgetId))]
		public Budget Budget { get; set; }

		public virtual List<CreditPayment> CreditPayments { get; set; }

	}
}
