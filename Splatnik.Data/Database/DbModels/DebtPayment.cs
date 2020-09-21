using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Splatnik.Data.Database.DbModels
{
    public class DebtPayment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal DebtPaymentValue { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [ForeignKey(nameof(CurrencyId))]
        public Currency Currency { get; set; }

        [Required]
        public int DebtId { get; set; }

        [ForeignKey(nameof(DebtId))]
        public Debt Debt { get; set; }

        [Required]
        public int PeriodId { get; set; }

        [ForeignKey(nameof(PeriodId))]
        public Period Period { get; set; }

    }
}