using AutoMapper;
using Splatnik.API.Dtos;
using Splatnik.API.Services.Interfaces;
using Splatnik.Contracts.V1.Requests;
using Splatnik.Data.Database.DbModels;
using Splatnik.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Services
{
    public class CreditPaymentService : ICreditPaymentService
    {
        private readonly IMapper _mapper;
        private readonly IBaseRepository<CreditPayment> _baseRepository;
        private readonly ICreditPaymentRepository _creditPaymentRepository;

        public CreditPaymentService(IMapper mapper, IBaseRepository<CreditPayment> baseRepository, ICreditPaymentRepository creditPaymentRepository)
        {
            _mapper = mapper;
            _baseRepository = baseRepository;
            _creditPaymentRepository = creditPaymentRepository;
        }



        public async Task<CreditPayment> CreateCreditPaymentAsync(CreditPaymentRequest request, int creditId, string userId)
        {
            var paymentDto = new CreditPaymentDto
            {
                CreatedDate = DateTime.UtcNow,
                PaymentDate = request.PaymentDate,
                PaymentValue = request.PaymentValue,
                CurrencyId = request.CurrencyId,
                CreditId = creditId,
                PeriodId = request.PeriodId,
                UserId = userId
            };

            var payment = _mapper.Map<CreditPayment>(paymentDto);

            return await _baseRepository.CreateEntityAsync(payment);
        }

        public async Task<CreditPayment> GetCreditPaymentAsync(int creditPaymentId)
        {
            return await _baseRepository.GetEntityAsync(creditPaymentId);

        }

        public async Task<IList<CreditPayment>> GetCreditPaymentsAsync(int creditId, string userId)
        {
            return await _creditPaymentRepository.GetCreditPaymentsAsync(creditId, userId);
        }

        public async Task<bool> UpdateCreditPaymentAsync(UpdateCreditPaymentRequest request, int creditPaymentId)
        {
            var paymentDto = new UpdateCreditPaymentDto
            {
                Id = creditPaymentId,
                PaymentDate = request.PaymentDate,
                PaymentValue = request.PaymentValue
            };

            var payment = _mapper.Map<CreditPayment>(paymentDto);

            return await _baseRepository.UpdateEntityAsync(payment);
        }

        public async Task<bool> DeleteCreditPaymentAsync(CreditPayment payment)
        {
            return await _baseRepository.DeleteEntityAsync(payment);
        }
    }
}
