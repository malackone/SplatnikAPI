using Splatnik.Data.Database;
using Splatnik.Data.Database.DbModels;
using Splatnik.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
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

        public Task<IList<DebtPayment>> GetDebtPaymentsAsync(int debtID)
        {
            throw new NotImplementedException();
        }

    }
}
