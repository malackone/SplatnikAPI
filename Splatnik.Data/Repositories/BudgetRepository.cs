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
    public class BudgetRepository : IBudgetRepository
    {
        private readonly DataContext _dataContext;
        public BudgetRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public async Task<IList<Budget>> GetUserBudgets(string userId)
        {
            return await _dataContext.Budgets.Include(x => x.Periods).Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
