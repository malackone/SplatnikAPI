using AutoMapper;
using Splatnik.API.Dtos;
using Splatnik.API.Services.Interfaces;
using Splatnik.Contracts.V1.Requests;
using Splatnik.Data.Database.DbModels;
using Splatnik.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Splatnik.API.Services
{
	public class BudgetService : IBudgetService
	{
		private readonly IBudgetRepository _budgetRepository;
		private readonly IMapper _mapper;

		public BudgetService(IBudgetRepository budgetRepository, IMapper mapper)
		{
			_budgetRepository = budgetRepository;
			_mapper = mapper;

		}

		public async Task<Budget> CreateBudgetAsync(NewBudgetRequest budgetRequest, string userId)
		{

			var budgetDto = new BudgetDto
			{
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				Name = budgetRequest.Name,
				Description = budgetRequest.Description,
				UserId = userId
			};

			var budget = _mapper.Map<Budget>(budgetDto);
			
			var created = await _budgetRepository.CreateBudgetAsync(budget);

			return created;
		}

		public async Task<Budget> GetBudgetAsync(int budgetId)
		{
			return await _budgetRepository.GetBudgetAsync(budgetId);
		}

		public async Task<IList<Budget>> GetUserBudgets(string userId) 
		{
			return await _budgetRepository.GetUserBudgets(userId);
		}

		public async Task<Period> CreatePeriodAsync(NewPeriodRequest periodRequest, string userId)
        {

			var periodDto = new PeriodDto
			{
				CreatedAt = DateTime.UtcNow,
				DisplayName = periodRequest.DisplayName,
				FirstDay = periodRequest.FirstDay,
				LastDay = periodRequest.LastDay,
				Notes = periodRequest.Notes,
				BudgetId = periodRequest.BudgetId
			};

			var period = _mapper.Map<Period>(periodDto);

			var created = await _budgetRepository.CreatePeriodAsync(period);

			return created;

        }

		public async Task<Period> GetPeriodAsync(int periodId)
        {
			return await _budgetRepository.GetPeriodAsync(periodId);
        }

		public async Task<Period> GetCurrentPeriodAsync(int budgetId, DateTime today)
        {
			return await _budgetRepository.GetCurrentPeriodAsync(budgetId, today);
        }

		public async Task<IList<Period>> GetBudgetPeriodsAsync(int budgetId)
        {
			return await _budgetRepository.GetBudgetPeriodsAsync(budgetId);
        }
    }
}
