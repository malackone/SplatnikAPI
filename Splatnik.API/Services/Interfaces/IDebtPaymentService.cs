using Splatnik.Contracts.V1.Requests;
using Splatnik.Data.Database.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Splatnik.API.Services.Interfaces
{
    public interface IDebtPaymentService
    {
        Task<DebtPayment> NewDebtPaymentAsync(DebtPaymentRequest request, int periodId);
        Task<DebtPayment> GetDebtPaymentAsync(int debtPaymentId);
        Task<IList<DebtPayment>> GetDebtPaymentsAsync(int debtId);
        Task<bool> UpdateDebtPaymentAsync(int debtPaymentId, UpdateDebtPaymentRequest request);
        Task<bool> DeleteDebtPaymentAsync(DebtPayment debtPayment);

    }
}
