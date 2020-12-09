using Splatnik.Contracts.V1.Requests;
using Splatnik.Data.Database.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Services.Interfaces
{
	public interface ICreditPaymentService
	{
		Task<CreditPayment> CreateCreditPaymentAsync(CreditPaymentRequest request, int creditId, string userId);
		Task<IList<CreditPayment>> GetCreditPaymentsAsync(int creditId, string userId);
		Task<CreditPayment> GetCreditPaymentAsync(int creditPaymentId);
		Task<bool> UpdateCreditPaymentAsync(UpdateCreditPaymentRequest request, int creditPaymentId);
		Task<bool> DeleteCreditPaymentAsync(CreditPayment payment);
	}
}
