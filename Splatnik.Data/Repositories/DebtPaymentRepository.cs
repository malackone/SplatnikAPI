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
    public class DebtPaymentRepository : IDebtPaymentRepository
    {
        private readonly DataContext _dataContext;

        public DebtPaymentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IList<DebtPayment>> GetDebtPaymentsAsync(int debtId)
        {
            return await _dataContext.DebtPayments.Where(x => x.DebtId == debtId).ToListAsync();
        }

    }
}
