using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Splatnik.Data.Database.DbModels;

namespace Splatnik.Data.Database
{
	public class DataContext : IdentityDbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}

		public DbSet<RefreshToken> RefreshTokens { get; set; }
		public DbSet<Budget> Budgets { get; set; }
		public DbSet<Period> Periods { get; set; }
		public DbSet<Income> Incomes { get; set; }
		public DbSet<Expense> Expenses { get; set; }
		public DbSet<Currency> Currencies { get; set; }
		public DbSet<Debt> Debts { get; set; }
		public DbSet<DebtPayment> DebtPayments { get; set; }
		public DbSet<Credit> Credits { get; set; }
		public DbSet<CreditPayment> CreditPayments { get; set; }


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Debt>()
				.HasMany(d => d.DebtPayments)
				.WithOne(d => d.Debt)
				.HasForeignKey(f => f.DebtId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<DebtPayment>()
				.HasOne(d => d.Debt)
				.WithMany(d => d.DebtPayments)
				.IsRequired()
				.OnDelete(DeleteBehavior.NoAction);

			builder.Entity<Credit>()
				.HasMany(c => c.CreditPayments)
				.WithOne(c => c.Credit)
				.HasForeignKey(c => c.CreditId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<CreditPayment>()
				.HasOne(c => c.Credit)
				.WithMany(c => c.CreditPayments)
				.IsRequired()
				.OnDelete(DeleteBehavior.NoAction);
		}
		
	}
}
