using Splatnik.Data.Database.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Splatnik.Data.Repositories.Interfaces
{
	public interface IDebtPaymentRepository
	{
		Task<IList<DebtPayment>> GetDebtPaymentsAsync(int debtId, string userId);
		Task<IList<DebtPayment>> GetUserDebtPayments(string userId);
	}
}
