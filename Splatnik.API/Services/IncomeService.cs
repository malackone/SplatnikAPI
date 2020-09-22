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
    public class IncomeService : IIncomeService
	{
		private readonly IMapper _mapper;
		private readonly IIncomeRepository _incomeRepository;
		private readonly IBaseRepository<Income> _baseRepository;

		public IncomeService(IMapper mapper, IIncomeRepository incomeRepository, IBaseRepository<Income> baseRepository)
		{
			_mapper = mapper;
			_baseRepository = baseRepository;
			_incomeRepository = incomeRepository;
		}

		public async Task<Income> NewIncomeAsync(int periodID, IncomeRequest request)
		{
			var incomeDto = new IncomeDto
			{
				CreatedAt = DateTime.UtcNow,
				IncomeDate = request.IncomeDate,
				Name = request.Name,
				Description = request.Description,
				IncomeValue = request.IncomeValue,
				CurrencyId = request.CurrencyId,
				PeriodId = periodID
			};

			var income = _mapper.Map<Income>(incomeDto);

			var created = await _baseRepository.CreateEntityAsync(income);

			return created;
		}

		public async Task<Income> GetIncomeAsync(int periodId, int incomeId)
		{
			return await _baseRepository.GetEntityAsync(incomeId);
		}

		public async Task<IList<Income>> GetIncomesAsync(int periodId)
		{
			return await _incomeRepository.GetIncomesForPeriodAsync(periodId);
		}

		public async Task<bool> UpdateIncomeAsync(int incomeId, UpdateIncomeRequest request)
		{
			var incomeDto = new UpdateIncomeDto
			{
				Id = incomeId,
				Name = request.Name,
				Description = request.Description,
				IncomeDate = request.IncomeDate,
				IncomeValue = request.IncomeValue,
				CurrencyId = request.CurrencyId,
				PeriodId = request.PeriodId
			};

			var income = _mapper.Map<Income>(incomeDto);
			return await _baseRepository.UpdateEntityAsync(income);
		}

		public async Task<bool> DeleteIncomeAsync(Income income)
		{
			return await _baseRepository.DeleteEntityAsync(income);
		}
	}
}
