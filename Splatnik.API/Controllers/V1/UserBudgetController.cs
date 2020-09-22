using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Splatnik.API.Security;
using Splatnik.API.Services.Interfaces;
using Splatnik.Contracts.V1;
using Splatnik.Contracts.V1.Requests;
using Splatnik.Contracts.V1.Responses;

namespace Splatnik.API.Controllers.V1
{
	[Authorize(Policy = SecurityPolicies.User, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class UserBudgetController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IUriService _uriService;
		private readonly IIdentityService _identityService;
		private readonly IBudgetService _budgetService;
		private readonly IPeriodService _periodService;
		private readonly IIncomeService _incomeService;
		private readonly IExpenseService _expenseService;
		private readonly IDebtService _debtService;
		private readonly ICreditService _creditService;
		
		public UserBudgetController(IMapper mapper, IUriService uriService, IIdentityService identityService, IBudgetService budgetService, IPeriodService periodService, IIncomeService incomeService, 
			IExpenseService expenseService, IDebtService debtService, ICreditService creditService)
		{
			_mapper = mapper;
			_identityService = identityService;
			_uriService = uriService;
			_budgetService = budgetService;
			_periodService = periodService;
			_incomeService = incomeService;
			_expenseService = expenseService;
			_debtService = debtService;
			_creditService = creditService;
		}


        #region Budgets
        [HttpPost(ApiRoutes.UserBudget.CreateBudget)]
		public async Task<IActionResult> CreateBudget([FromBody] BudgetRequest request)
		{ 
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			var userExists = await _identityService.CheckIfUserExists(userId);
            if (!userExists)
            {
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}"}));
            }

			var budget = await _budgetService.NewBudgetAsync(request, userId);

			if(budget == null)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = "Could not create new budget"}));
			}

			var locationUri = _uriService.GetBudgetUri(budget.Id);

			return Created(locationUri, new Response<BudgetResponse>(_mapper.Map<BudgetResponse>(budget)));
		}


		[HttpGet(ApiRoutes.UserBudget.Budget)]
		public async Task<IActionResult> GetBudget([FromRoute] int budgetId)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			var userExists = await _identityService.CheckIfUserExists(userId);
            if (!userExists)
            {
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
            }

			var budget = await _budgetService.GetBudgetAsync(budgetId);

			if(budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {budgetId}" }));
			}

			return Ok(new Response<BudgetResponse>(_mapper.Map<BudgetResponse>(budget)));
		}


		[HttpGet(ApiRoutes.UserBudget.Budgets)]
		public async Task<IActionResult> UserBudgets()
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			var userExists = await _identityService.CheckIfUserExists(userId);
            if (!userExists)
            {
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var budgets = await _budgetService.GetUserBudgets(userId);

			return Ok(new Response<IEnumerable<BudgetResponse>>(_mapper.Map<IEnumerable<BudgetResponse>>(budgets)));
		}
        #endregion

        #region Periods
		[HttpPost(ApiRoutes.UserBudget.CreateBudgetPeriod)]
		public async Task<IActionResult> CreatePeriod([FromRoute] int budgetId, [FromBody] PeriodRequest request)
        {

			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			// check if new period budgetId is correct
			var budget = await _budgetService.GetBudgetAsync(budgetId);
			if(budget == null)
            {
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {budgetId}" }));
            }

			if(budget.UserId != userId)
            {
				return Forbid();
            }


			// Check if new period dates are between any other period dates
			var periodList = await _periodService.GetBudgetPeriodsAsync(budgetId);
			if (periodList.Any(x => x.FirstDay <= request.FirstDay && x.LastDay >= request.FirstDay))
			{
				return BadRequest(new ErrorResponse(new ErrorModel { FieldName = nameof(request.FirstDay), Message = "First day of new period cannot be between other period dates"}));
			}
			if (periodList.Any(x => x.FirstDay <= request.LastDay && x.LastDay >= request.LastDay))
            {
				return BadRequest(new ErrorResponse(new ErrorModel { FieldName = nameof(request.LastDay), Message = "Last day of new period cannot be between other period dates" }));
			}

			// create new period
			var period = await _periodService.NewPeriodAsync(request, budgetId);

			if(period == null)
            {
				return BadRequest(new ErrorResponse(new ErrorModel { Message = "Could not create new period"}));
            }

			var locationUri = _uriService.GetPeriodUri(budgetId, period.Id);

			return Created(locationUri, new Response<PeriodResponse>(_mapper.Map<PeriodResponse>(period)));

		}


		[HttpGet(ApiRoutes.UserBudget.BudgetPeriod)]
		public async Task<IActionResult> GetBudgetPeriod ([FromRoute] int budgetId, int periodId)
        {
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var budget = await _budgetService.GetBudgetAsync(budgetId);
			if (budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {budgetId}" }));
			}

			if (budget.UserId != userId)
			{
				return Forbid();
			}

			var period = await _periodService.GetPeriodAsync(budgetId, periodId);

			if(period == null)
            {
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no Period with id: {periodId} in Budget with id: {budgetId}" }));
            }

			return Ok(new Response<PeriodResponse>(_mapper.Map<PeriodResponse>(period)));
		}


		[HttpGet(ApiRoutes.UserBudget.BudgetPeriods)]
		public async Task<IActionResult> GetBudgetPeriods([FromRoute] int budgetId)
        {
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var budget = await _budgetService.GetBudgetAsync(budgetId);
			if (budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {budgetId}" }));
			}

			if (budget.UserId != userId)
			{
				return Forbid();
			}

			var periods = await _periodService.GetBudgetPeriodsAsync(budgetId);

			if (periods == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no Period in Budget with id: {budgetId}" }));
			}

			return Ok(new Response<List<PeriodResponse>>(_mapper.Map<List<PeriodResponse>>(periods)));

		}
        #endregion

        #region Expenses
		[HttpPost(ApiRoutes.UserBudget.CreateExpense)]
		public async Task<IActionResult> CreateExpense([FromRoute] int budgetId, int periodId, [FromBody] ExpenseRequest request)
        {
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var budget = await _budgetService.GetBudgetAsync(budgetId);
			if (budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {budgetId}" }));
			}

			if (budget.UserId != userId)
			{
				return Forbid();
			}

			var period = await _periodService.GetPeriodAsync(budgetId, periodId);

			if (period == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no Period with id: {periodId} in Budget with id: {budgetId}" }));
			}

			var expense = await _expenseService.NewExpenseAsync(periodId, request);

			if(expense == null)
            {
				return BadRequest();
            }

			var locationUri = _uriService.GetExpenseUri(budgetId, periodId, expense.Id);

			return Created(locationUri, new Response<ExpenseResponse>(_mapper.Map<ExpenseResponse>(expense)));


		}

		
		[HttpGet(ApiRoutes.UserBudget.BudgetPeriodExpense)]
		public async Task<IActionResult> GetExpense([FromRoute] int budgetId, int periodId, int expenseId)
        {
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var budget = await _budgetService.GetBudgetAsync(budgetId);
			if (budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {budgetId}" }));
			}

			if (budget.UserId != userId)
			{
				return Forbid();
			}


			var expense = await _expenseService.GetExpenseAsync(periodId, expenseId);

			if(expense == null)
            {
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no expense with id: {expenseId} for period id: {periodId}"}));
            }

			return Ok(new Response<ExpenseResponse>(_mapper.Map<ExpenseResponse>(expense)));
		}


		[HttpPatch(ApiRoutes.UserBudget.UpdateExpense)]
		public async Task<IActionResult> UpdateExpense([FromRoute] int budgetId, int periodId, int expenseId, [FromBody] UpdateExpenseRequest request)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var budget = await _budgetService.GetBudgetAsync(budgetId);
			if (budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {budgetId}" }));
			}

			if (budget.UserId != userId)
			{
				return Forbid();
			}

			var expenseInDb = await _expenseService.GetExpenseAsync(periodId, expenseId);

			if (expenseInDb == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no expense with id: {expenseId} for period id: {periodId}" }));
			}

			var updateExpense = await _expenseService.UpdateExpenseAsync(periodId, request);

			if (!updateExpense)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not update expense with id:{expenseId}" }));
			}

			var updatedExpense = await _expenseService.GetExpenseAsync(request.PeriodId, expenseId);

			return Ok(new Response<ExpenseResponse>(_mapper.Map<ExpenseResponse>(updatedExpense)));

		}


		[HttpDelete(ApiRoutes.UserBudget.DeleteExpense)]
		public async Task<IActionResult> DeleteExpense([FromRoute] int budgetId, int periodId, int expenseId)
        {
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var budget = await _budgetService.GetBudgetAsync(budgetId);
			if (budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {budgetId}" }));
			}

			if (budget.UserId != userId)
			{
				return Forbid();
			}

			var expenseInDb = await _expenseService.GetExpenseAsync(periodId, expenseId);
			if(expenseInDb == null)
            {
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no expense with id:{expenseId}"}));
            }

			var deleteExpense = await _expenseService.DeleteExpenseAsync(expenseInDb);
			if (!deleteExpense)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not delete expense with id: {expenseId}" }));
			}

			return NoContent();
		}
		#endregion

		#region Incomes
		[HttpPost(ApiRoutes.UserBudget.CreateIncome)]
		public async Task<IActionResult> CreateIncome([FromRoute] int budgetId, int periodId, [FromBody] IncomeRequest request)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var budget = await _budgetService.GetBudgetAsync(budgetId);
			if (budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {budgetId}" }));
			}

			if (budget.UserId != userId)
			{
				return Forbid();
			}


			var period = await _periodService.GetPeriodAsync(budgetId, periodId);
			if (period == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no Period with id: {periodId} in Budget with id: {budgetId}" }));
			}


			var income = await _incomeService.NewIncomeAsync(periodId, request);
			if (income == null)
			{
				return BadRequest();
			}

			var locationUri = _uriService.GetIncomeUri(budgetId, periodId, income.Id);

			return Created(locationUri, new Response<IncomeResponse>(_mapper.Map<IncomeResponse>(income)));


		}


		[HttpGet(ApiRoutes.UserBudget.BudgetPeriodIncome)]
		public async Task<IActionResult> GetIncome([FromRoute] int budgetId, int periodId, int incomeId)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var budget = await _budgetService.GetBudgetAsync(budgetId);
			if (budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {budgetId}" }));
			}

			if (budget.UserId != userId)
			{
				return Forbid();
			}


			var income = await _incomeService.GetIncomeAsync(periodId, incomeId);
			if (income == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no income with id: {incomeId} for period id: {periodId}" }));
			}

			return Ok(new Response<IncomeResponse>(_mapper.Map<IncomeResponse>(income)));
		}


		[HttpPatch(ApiRoutes.UserBudget.UpdateIncome)]
		public async Task<IActionResult> UpdateIncome([FromRoute] int budgetId, int periodId, int incomeId, [FromBody] UpdateIncomeRequest request)
        {
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var budget = await _budgetService.GetBudgetAsync(budgetId);
			if (budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {budgetId}" }));
			}

			if (budget.UserId != userId)
			{
				return Forbid();
			}

			var incomeInDb = await _incomeService.GetIncomeAsync(periodId, incomeId);

			if(incomeInDb == null)
            {
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no income with id: {incomeId} for period id: {periodId}" }));
			}

			var updateIncome = await _incomeService.UpdateIncomeAsync(periodId, request);

            if (!updateIncome)
            {
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not update income with id:{incomeId}" }));
            }

			var updatedIncome = await _incomeService.GetIncomeAsync(request.PeriodId, incomeId);

			return Ok(new Response<IncomeResponse>(_mapper.Map<IncomeResponse>(updatedIncome)));

		}


		[HttpDelete(ApiRoutes.UserBudget.DeleteIncome)]
		public async Task<IActionResult> DeleteIncome([FromRoute] int budgetId, int periodId, int incomeId)
        {
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var budget = await _budgetService.GetBudgetAsync(budgetId);
			if (budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {budgetId}" }));
			}

			if (budget.UserId != userId)
			{
				return Forbid();
			}

			var incomeInDb = await _incomeService.GetIncomeAsync(periodId, incomeId);
			if(incomeInDb == null)
            {
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is not income with id:{incomeId}" }));
            }


			var deleteIncome = await _incomeService.DeleteIncomeAsync(incomeInDb);
			if (!deleteIncome)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not delete income with id:{incomeId}" }));
			}

			return NoContent();
		}
        #endregion

        #region Debts
		[HttpPost(ApiRoutes.UserBudget.CreateDebt)]
		public async Task<IActionResult> CreateDebt([FromRoute] int budgetId, [FromBody] DebtRequest request)
        {
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			// check if new debt budgetId is correct
			var budget = await _budgetService.GetBudgetAsync(budgetId);
			if (budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {budgetId}" }));
			}

			if (budget.UserId != userId)
			{
				return Forbid();
			}

			// create debt
			var newDebt = _debtService.NewDebtAsync(request, budgetId);

			if(newDebt == null)
            {
				return BadRequest(new ErrorResponse(new ErrorModel { Message = "Could not create new debt" }));
            }

			var locationUri = _uriService.GetDebtUri(budgetId, newDebt.Id);

			return Created(locationUri, new Response<DebtResponse>(_mapper.Map<DebtResponse>(newDebt)));

		}


		[HttpGet(ApiRoutes.UserBudget.BudgetDebt)]
		public async Task<IActionResult> GetDebt([FromRoute] int budgetId, int debtId)
        {
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var budget = await _budgetService.GetBudgetAsync(budgetId);
			if (budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {budgetId}" }));
			}

			if (budget.UserId != userId)
			{
				return Forbid();
			}

			var debt = await _debtService.GetDebtAsync(budgetId, debtId);

			if (debt == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no debt with id: {debtId} in Budget with id: {budgetId}" }));
			}

			return Ok(new Response<DebtResponse>(_mapper.Map<DebtResponse>(debt)));

		}


		[HttpGet(ApiRoutes.UserBudget.BudgetDebts)]
		public async Task<IActionResult> GetDebts([FromRoute] int budgetId)
        {
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var budget = await _budgetService.GetBudgetAsync(budgetId);
			if (budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {budgetId}" }));
			}

			if (budget.UserId != userId)
			{
				return Forbid();
			}

			var debt = await _debtService.GetDebtsAsync(budgetId);

			if (debt == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no debt in Budget with id: {budgetId}" }));
			}

			return Ok(new Response<IList<DebtResponse>>(_mapper.Map<IList<DebtResponse>>(debt)));
		}


		[HttpPatch(ApiRoutes.UserBudget.BudgetDebt)]
		public async Task<IActionResult> UpdateDebt([FromRoute] int budgetId, int debtId, [FromBody] UpdateDebtRequest request)
        {
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var budget = await _budgetService.GetBudgetAsync(budgetId);
			if (budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {budgetId}" }));
			}

			if (budget.UserId != userId)
			{
				return Forbid();
			}


			var debtInDb = await _debtService.GetDebtAsync(budgetId, debtId);
			if (debtInDb == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no debt with id: {debtId} for budget id: {budgetId}" }));
			}


			var updateDebt = await _debtService.UpdateDebtAsync(debtId, request);
			if (!updateDebt)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not update debt with id:{debtId}" }));
			}

			var updatedDebt = await _debtService.GetDebtAsync(budgetId, debtId);
			return Ok(new Response<DebtResponse>(_mapper.Map<DebtResponse>(updatedDebt)));
		}


		[HttpDelete(ApiRoutes.UserBudget.BudgetDebt)]
		public async Task<IActionResult> DeleteDebt([FromRoute] int budgetId, int debtId)
        {
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var budget = await _budgetService.GetBudgetAsync(budgetId);
			if (budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {budgetId}" }));
			}

			if (budget.UserId != userId)
			{
				return Forbid();
			}

			var debtInDb = await _debtService.GetDebtAsync(budgetId, debtId);
			if (debtInDb == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is not debt with id:{debtId}" }));
			}


			var deleteDebt = await _debtService.DeleteDebtAsync(debtInDb);
			if (!deleteDebt)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not delete debt with id:{debtId}" }));
			}

			return NoContent();
		}
		#endregion
	}
}
