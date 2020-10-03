using Splatnik.Data.Database.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Splatnik.Data.Repositories.Interfaces
{
    public interface IIncomeRepository
    {
        Task<IList<Income>> GetIncomesForPeriodAsync(int periodId);
    }
}
