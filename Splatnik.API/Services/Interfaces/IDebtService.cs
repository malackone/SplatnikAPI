using Splatnik.Contracts.V1.Requests;
using Splatnik.Data.Database.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Services.Interfaces
{
    public interface IDebtService
    {
        Task<Debt> NewDebtAsync(DebtRequest request, string userId);
        Task<Debt> GetDebtAsync(int debtId);
        Task<IList<Debt>> GetDebtsByUserIdAsync(string userId);
        Task<bool> UpdateDebtAsync(int debtId, UpdateDebtRequest request);
        Task<bool> DeleteDebtAsync(Debt debt);
    }
}
