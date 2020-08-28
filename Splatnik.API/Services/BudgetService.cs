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
	public class BudgetService : IBudgetService
	{
		private readonly IBudgetRepository _budgetRepository;
		private readonly IMapper _mapper;

		public BudgetService(IBudgetRepository budgetRepository, IMapper mapper)
		{
			_budgetRepository = budgetRepository;
			_mapper = mapper;

		}

		public async Task<Budget> CreateBudgetAsync(CreateBudgetRequest budgetRequest)
		{

			var budgetDto = new BudgetDto
			{
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				Name = budgetRequest.Name,
				Description = budgetRequest.Description,
				UserId = budgetRequest.UserId,
			};

			var budget = _mapper.Map<Budget>(budgetDto);
			
			var created = await _budgetRepository.CreateBudgetAsync(budget);

			return created;
		}

		public async Task<Budget> GetBudgetAsync(int budgetId)
		{
			return await _budgetRepository.GetBudgetAsync(budgetId);
		}
	}
}
