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
    public class ExpenseService : IExpenseService
    {
		private readonly IMapper _mapper;
		private readonly IBaseRepository<Expense> _baseRepository;
		private readonly IExpenseRepository _expenseRepository;

		public ExpenseService(IMapper mapper, IExpenseRepository expenseRepository, IBaseRepository<Expense> baseRepository)
		{
			_expenseRepository = expenseRepository;
			_baseRepository = baseRepository;
			_mapper = mapper;
		}

		public async Task<Expense> NewExpenseAsync(ExpenseRequest request, string userId)
		{
			var expenseDto = new ExpenseDto
			{
				UserId = userId,
				CreatedAt = DateTime.UtcNow,
				IncomeDate = request.IncomeDate,
				Name = request.Name,
				Description = request.Description,
				ExpenseValue = request.ExpenseValue,
				CurrencyId = request.CurrencyId,
				PeriodId = request.PeriodId
			};

			var expense = _mapper.Map<Expense>(expenseDto);

			var created = await _baseRepository.CreateEntityAsync(expense);

			return created;
		}

		public async Task<Expense> GetExpenseAsync(int expenseId)
		{
			return await _baseRepository.GetEntityAsync(expenseId);
		}

		public async Task<IList<Expense>> GetExpensesAsync(int periodId)
		{
			return await _expenseRepository.GetExpensesForPeriodAsync(periodId);
		}

		public async Task<bool> UpdateExpenseAsync(int expenseId, UpdateExpenseRequest request)
		{
			var expenseDto = new UpdateExpenseDto
			{
				Id = expenseId,
				Name = request.Name,
				Description = request.Description,
				IncomeDate = request.IncomeDate,
				ExpenseValue = request.ExpenseValue,
				CurrencyId = request.CurrencyId,
				PeriodId = request.PeriodId
			};

			var expense = _mapper.Map<Expense>(expenseDto);

			return await _baseRepository.UpdateEntityAsync(expense);
		}

		public async Task<bool> DeleteExpenseAsync(Expense expense)
		{
			return await _baseRepository.DeleteEntityAsync(expense);
		}
	}
}
