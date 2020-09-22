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
    public class PeriodService : IPeriodService
	{
		private readonly IMapper _mapper;
		private readonly IBaseRepository<Period> _baseRepository;
		private readonly IPeriodRepository _periodRepository;

		public PeriodService(IMapper mapper, IBaseRepository<Period> baseRepository, IPeriodRepository periodRepository)
        {
			_mapper = mapper;
			_baseRepository = baseRepository;
			_periodRepository = periodRepository;
        }


		public async Task<Period> NewPeriodAsync(PeriodRequest periodRequest, int budgetId)
		{
			var periodDto = new PeriodDto
			{
				CreatedAt = DateTime.UtcNow,
				DisplayName = periodRequest.DisplayName,
				FirstDay = periodRequest.FirstDay,
				LastDay = periodRequest.LastDay,
				Notes = periodRequest.Notes,
				BudgetId = budgetId
			};

			var period = _mapper.Map<Period>(periodDto);

			var created = await _baseRepository.CreateEntityAsync(period);

			return created;

		}

		public async Task<Period> GetPeriodAsync(int budgetId, int periodId)
		{
			return await _baseRepository.GetEntityAsync(periodId);
		}

		public async Task<IList<Period>> GetBudgetPeriodsAsync(int budgetId)
		{
			return await _periodRepository.GetBudgetPeriodsAsync(budgetId);
		}
	}
}
