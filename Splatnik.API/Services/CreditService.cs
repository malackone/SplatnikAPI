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
	public class CreditService : ICreditService
	{
		private readonly IMapper _mapper;
		private readonly IBaseRepository<Credit> _baseRepository;
		private readonly ICreditRepository _creditRepository;

		public CreditService(IMapper mapper, IBaseRepository<Credit> baseRepository, ICreditRepository creditRepository)
		{
			_mapper = mapper;
			_baseRepository = baseRepository;
			_creditRepository = creditRepository;
		}


		public async Task<Credit> NewCreditAsync(CreditRequest request, string userId)
		{
			var creditDto = new CreditDto
			{
				CreatedAt = DateTime.UtcNow,
				BankAccountNumber = request.BankAccountNumber,
				BankName = request.BankName,
				BudgetId = request.BudgetId,
				ContractNumber = request.ContractNumber,
				CurrencyId = request.CurrencyId,
				Description = request.Description,
				InitialCreditValue = request.InitialCreditValue,
				InitNoOfPayments = request.InitNoOfPayments,
				Name = request.Name,
				UserId = userId
			};

			var credit = _mapper.Map<Credit>(creditDto);

			return await _baseRepository.CreateEntityAsync(credit);
		}
		
		public async Task<Credit> GetCreditAsync(int creditId)
		{
			return await _baseRepository.GetEntityAsync(creditId);
		}

		public async Task<IList<Credit>> GetCreditsAsync(int budgetId, string userId)
		{
			return await _creditRepository.GetUserBudgetCreditsAsync(budgetId, userId);
		}

		public async Task<IList<Credit>> GetUserCreditsASync(string userId)
		{
			return await _creditRepository.GetUserCreditsAsync(userId);
		}

		public Task<Credit> RecalculateCreditAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<bool> UpdateCreditAsync(UpdateCreditRequest request, int creditId)
		{
			var updateDto = new UpdateCreditDto
			{
				Id = creditId,
				BankAccountNumber = request.BankAccountNumber,
				BankName = request.BankName,
				ContractNumber = request.ContractNumber,
				CurrencyId = request.CurrencyId,
				Description = request.Description,
				InitialCreditValue = request.InitialCreditValue,
				InitNoOfPayments = request.InitNoOfPayments,
				Name = request.Name,
			};

			var credit = _mapper.Map<Credit>(updateDto);

			return await _baseRepository.UpdateEntityAsync(credit);
		}

		public async Task<bool> DeleteCreditAsync(Credit credit)
		{
			return await _baseRepository.DeleteEntityAsync(credit);
		}

	}
}
