using Splatnik.Contracts.V1.Requests;
using Splatnik.Data.Database.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Services.Interfaces
{
    public interface IPeriodService
    {
        Task<Period> NewPeriodAsync(PeriodRequest request, int budgetId);
        Task<Period> GetPeriodAsync(int budgetId, int periodId);
        Task<IList<Period>> GetBudgetPeriodsAsync(int budgetId);
    }
}
