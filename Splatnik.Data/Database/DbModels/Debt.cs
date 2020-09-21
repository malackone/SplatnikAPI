using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Splatnik.Data.Database.DbModels
{
    public class Debt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal InitialValue { get; set; }

        [Required]
        public int BudgetId { get; set; }

        [ForeignKey(nameof(BudgetId))]
        public Budget Budget { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [ForeignKey(nameof(CurrencyId))]
        public Currency Currency { get; set; }

        public virtual List<DebtPayment> DebtPayments { get; set; }
    }
}
