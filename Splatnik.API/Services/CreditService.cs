using Splatnik.API.Services.Interfaces;
using Splatnik.Contracts.V1.Requests;
using Splatnik.Data.Database.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Services
{
    public class CreditService : ICreditService
    {
        public Task<bool> DeleteCreditAsync(Credit credit)
        {
            throw new NotImplementedException();
        }

        public Task<Credit> GetCreditAsync(int creditId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Credit>> GetCreditsAsync(int budgetId)
        {
            throw new NotImplementedException();
        }

        public Task<Credit> NewCreditAsync(CreditRequest request, int budgetId)
        {
            throw new NotImplementedException();
        }

        public Task<Credit> RecalculateCreditAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateCreditAsync(UpdateCreditRequest request, int budgetId)
        {
            throw new NotImplementedException();
        }
    }
}
