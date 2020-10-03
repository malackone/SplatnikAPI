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
		private readonly IMapper _mapper;
		private readonly IBaseRepository<Budget> _baseRepository;
		private readonly IBudgetRepository _budgetRepository;


		public BudgetService(IMapper mapper, IBaseRepository<Budget> baseRepository, IBudgetRepository budgetRepository)
		{
			_mapper = mapper;
			_baseRepository = baseRepository;
			_budgetRepository = budgetRepository;
		}

        public async Task<Budget> NewBudgetAsync(BudgetRequest budgetRequest, string userId)
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
			
			var created = await _baseRepository.CreateEntityAsync(budget);

			return created;
		}

		public async Task<Budget> GetBudgetAsync(int budgetId)
		{
			return await _baseRepository.GetEntityAsync(budgetId);
		}

		public async Task<IList<Budget>> GetUserBudgets(string userId) 
		{
			return await _budgetRepository.GetUserBudgets(userId);
		}
	}
}
