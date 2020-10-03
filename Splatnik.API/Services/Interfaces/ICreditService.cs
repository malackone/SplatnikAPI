using Splatnik.Contracts.V1.Requests;
using Splatnik.Data.Database.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Services.Interfaces
{
    public interface ICreditService
    {
        Task<Credit> NewCreditAsync(CreditRequest request, string userId);
        Task<Credit> GetCreditAsync(int creditId);
        Task<IList<Credit>> GetCreditsAsync(int budgetId);
        Task<bool> UpdateCreditAsync(UpdateCreditRequest request, int budgetId);
        Task<bool> DeleteCreditAsync(Credit credit);
        Task<Credit> RecalculateCreditAsync();
    }
}
