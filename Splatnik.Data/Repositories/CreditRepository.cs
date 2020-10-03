using Microsoft.EntityFrameworkCore;
using Splatnik.Data.Database;
using Splatnik.Data.Database.DbModels;
using Splatnik.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatnik.Data.Repositories
{
    public class CreditRepository : ICreditRepository
    {
        private readonly DataContext dataContext;

        public CreditRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<IList<Credit>> GetUserBudgetCreditsAsync(int budgetId, string userId)
        {
            return await dataContext.Credits.Where(x => x.BudgetId == budgetId && x.UserId == userId).ToListAsync();
        }

        public async Task<IList<Credit>> GetUserCreditsAsync(string userId)
        {
            return await dataContext.Credits.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
