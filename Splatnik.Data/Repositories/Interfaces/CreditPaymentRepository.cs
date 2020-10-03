using Microsoft.EntityFrameworkCore;
using Splatnik.Data.Database;
using Splatnik.Data.Database.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatnik.Data.Repositories.Interfaces
{
    public class CreditPaymentRepository : ICreditPaymentRepository
    {
        private readonly DataContext dataContext;

        public CreditPaymentRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<IList<CreditPayment>> GetCreditPaymentsAsync(int creditId, string userId)
        {
            return await dataContext.CreditPayments.Where(x => x.CreditId == creditId && x.UserId == userId).ToListAsync();
        }
    }
}
