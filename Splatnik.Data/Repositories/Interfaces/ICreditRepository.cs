using Splatnik.Data.Database.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Splatnik.Data.Repositories.Interfaces
{
    public interface ICreditRepository
    {
        Task<IList<Credit>> GetUserCreditsAsync(string userId);
        Task<IList<Credit>> GetUserBudgetCreditsAsync(int budgetId, string userId);
    }
}
