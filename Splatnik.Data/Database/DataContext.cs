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
	}
}
