using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Splatnik.Data.Database.DbModels
{
    public class CreditPayment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime PaidAt { get; set; }

        [Required]
        public DateTime PlannedDateOfPayment { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PaymentValue { get; set; }

        [Required]
        public bool IsPaid { get; set; }

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