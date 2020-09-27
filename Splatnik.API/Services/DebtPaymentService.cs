using AutoMapper;
using Splatnik.API.Dtos;
using Splatnik.API.Services.Interfaces;
using Splatnik.Contracts.V1.Requests;
using Splatnik.Data.Database.DbModels;
using Splatnik.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Splatnik.API.Services
{
    public class DebtPaymentService : IDebtPaymentService
    {
        private IMapper _mapper;
        private readonly IBaseRepository<DebtPayment> _baseRepository;
        private readonly IDebtPaymentRepository _debtPaymentRepository;

        public DebtPaymentService(IMapper mapper, IBaseRepository<DebtPayment> baseRepository, IDebtPaymentRepository debtPaymentRepository)
        {
            _mapper = mapper;
            _baseRepository = baseRepository;
            _debtPaymentRepository = debtPaymentRepository;
        }


        public async Task<DebtPayment> NewDebtPaymentAsync(DebtPaymentRequest request, int periodId)
        {
            var debtPaymentDto = new DebtPaymentDto
            {
                CreatedAt = DateTime.UtcNow,
                Description = request.Description,
                DebtPaymentValue = request.DebtPaymentValue,
                CurrencyId = request.CurrencyId,
                PeriodId = periodId
            };

            var debtPayment = _mapper.Map<DebtPayment>(debtPaymentDto);

            return await _baseRepository.CreateEntityAsync(debtPayment);
        }

        public async Task<DebtPayment> GetDebtPaymentAsync(int debtPaymentId)
        {
            return await _baseRepository.GetEntityAsync(debtPaymentId);
        }

        public async Task<IList<DebtPayment>> GetDebtPaymentsAsync(int debtId)
        {
            return await _debtPaymentRepository.GetDebtPaymentsAsync(debtId);
        }

        public async Task<bool> UpdateDebtPaymentAsync(int debtPaymentId, UpdateDebtPaymentRequest request)
        {
            var currentDebtPayment = await _baseRepository.GetEntityAsync(debtPaymentId);

            currentDebtPayment.UpdatedAt = DateTime.UtcNow;
            currentDebtPayment.Description = request.Description;
            currentDebtPayment.DebtPaymentValue = request.DebtPaymentValue;
            currentDebtPayment.CurrencyId = request.CurrencyId;
            currentDebtPayment.PeriodId = request.PeriodId;

            return await _baseRepository.UpdateEntityAsync(currentDebtPayment);
        }

        public async Task<bool> DeleteDebtPaymentAsync(DebtPayment debtPayment)
        {
            return await _baseRepository.DeleteEntityAsync(debtPayment);
        }
    }
}
