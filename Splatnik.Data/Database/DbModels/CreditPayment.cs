using Splatnik.Data.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Splatnik.Data.Database.DbModels
{
    public class CreditPayment : BaseEntity
    {
        [Required]
        [MaxLength(450)]
        public string UserId { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PaymentValue { get; set; }

        [Required]
        public int PeriodId { get; set; }

        [ForeignKey(nameof(PeriodId))]
        public Period Period { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [ForeignKey(nameof(CurrencyId))]
        public Currency Currency { get; set; }

        [Required]
        public int CreditId { get; set; }

        [ForeignKey(nameof(CreditId))]
        public Credit Credit { get; set; }
    }
}