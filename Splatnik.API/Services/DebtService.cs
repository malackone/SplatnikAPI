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
    public class DebtService : IDebtService
    {
		private readonly IMapper _mapper;
		private readonly IBaseRepository<Debt> _baseRepository;
		private readonly IDebtRepository _debtRepository;

		public DebtService(IMapper mapper, IBaseRepository<Debt> baseRepository, IDebtRepository debtRepository)
		{
			_mapper = mapper;
			_baseRepository = baseRepository;
			_debtRepository = debtRepository;
		}

		public async Task<Debt> NewDebtAsync(DebtRequest request, int budgetId)
		{
			var debtDto = new DebtDto
			{
				CreatedAt = DateTime.UtcNow,
				Name = request.Name,
				Description = request.Description,
				InitialValue = request.InitialValue,
				CurrencyId = request.CurrencyId,
				BudgetId = budgetId
			};

			var debt = _mapper.Map<Debt>(debtDto);

			var created = await _baseRepository.CreateEntityAsync(debt);

			return created;
		}

		public async Task<Debt> GetDebtAsync(int budgetId, int debtId)
		{
			return await _baseRepository.GetEntityAsync(debtId);
		}

		public async Task<IList<Debt>> GetDebtsAsync(int budgetId)
		{
			return await _debtRepository.GetDebtsForBudgetAsync(budgetId);
		}

		public async Task<bool> UpdateDebtAsync(int debtId, UpdateDebtRequest request)
		{
			var debtDto = new UpdateDebtDto
			{
				Name = request.Name,
				Description = request.Description,
				InitialValue = request.InitialValue
			};

			var debt = _mapper.Map<Debt>(debtDto);
			return await _baseRepository.UpdateEntityAsync(debt);
		}

		public async Task<bool> DeleteDebtAsync(Debt debt)
		{
			return await _baseRepository.DeleteEntityAsync(debt);
		}

	}
}
