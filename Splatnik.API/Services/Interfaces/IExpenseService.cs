using Splatnik.Contracts.V1.Requests;
using Splatnik.Data.Database.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<Expense> NewExpenseAsync(int periodId, ExpenseRequest request);
        Task<Expense> GetExpenseAsync(int periodId, int expenseId);
        Task<IList<Expense>> GetExpensesAsync(int periodId);
        Task<bool> UpdateExpenseAsync(int expenseId, UpdateExpenseRequest request);
        Task<bool> DeleteExpenseAsync(Expense expense);
    }
}
