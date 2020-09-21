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
		private readonly IBudgetService _budgetService;
		private readonly IIdentityService _identityService;
		private readonly IMapper _mapper;
		private readonly IUriService _uriService;

		public UserBudgetController(IBudgetService budgetService, IIdentityService identityService,IMapper mapper, IUriService uriService)
		{
			_budgetService = budgetService;
			_identityService = identityService;
			_mapper = mapper;
			_uriService = uriService;
		}


        #region Budgets
        [HttpPost(ApiRoutes.UserBudget.NewBudget)]
		public async Task<IActionResult> NewBudget([FromBody] BudgetRequest request)
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
		[HttpPost(ApiRoutes.UserBudget.NewBudgetPeriod)]
		public async Task<IActionResult> NewPeriod([FromRoute] int budgetId, [FromBody] PeriodRequest request)
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
			var periodList = await _budgetService.GetBudgetPeriodsAsync(budgetId);
			if (periodList.Any(x => x.FirstDay <= request.FirstDay && x.LastDay >= request.FirstDay))
			{
				return BadRequest(new ErrorResponse(new ErrorModel { FieldName = nameof(request.FirstDay), Message = "First day of new period cannot be between other period dates"}));
			}
			if (periodList.Any(x => x.FirstDay <= request.LastDay && x.LastDay >= request.LastDay))
            {
				return BadRequest(new ErrorResponse(new ErrorModel { FieldName = nameof(request.LastDay), Message = "Last day of new period cannot be between other period dates" }));
			}

			// create new period
			var period = await _budgetService.NewPeriodAsync(request, budgetId);

			if(period == null)
            {
				return BadRequest();
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

			var period = await _budgetService.GetPeriodAsync(budgetId, periodId);

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

			var periods = await _budgetService.GetBudgetPeriodsAsync(budgetId);

			if (periods == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no Period in Budget with id: {budgetId}" }));
			}

			return Ok(new Response<List<PeriodResponse>>(_mapper.Map<List<PeriodResponse>>(periods)));

		}
        #endregion

        #region Expenses
		[HttpPost(ApiRoutes.UserBudget.NewExpense)]
		public async Task<IActionResult> NewExpense([FromRoute] int budgetId, int periodId, [FromBody] ExpenseRequest request)
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

			var period = await _budgetService.GetPeriodAsync(budgetId, periodId);

			if (period == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no Period with id: {periodId} in Budget with id: {budgetId}" }));
			}

			var expense = await _budgetService.NewExpenseAsync(periodId, request);

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


			var expense = await _budgetService.GetExpenseAsync(periodId, expenseId);

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

			var expenseInDb = await _budgetService.GetExpenseAsync(periodId, expenseId);

			if (expenseInDb == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no expense with id: {expenseId} for period id: {periodId}" }));
			}

			var updateExpense = await _budgetService.UpdateExpenseAsync(periodId, request);

			if (!updateExpense)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not update expense with id:{expenseId}" }));
			}

			var updatedExpense = await _budgetService.GetIncomeAsync(request.PeriodId, expenseId);

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

			var expenseInDb = await _budgetService.GetExpenseAsync(periodId, expenseId);
			if(expenseInDb == null)
            {
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no expense with id:{expenseId}"}));
            }

			var deleteExpense = await _budgetService.DeleteExpenseAsync(expenseInDb);
			if (!deleteExpense)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not delete expense with id: {expenseId}" }));
			}

			return NoContent();
		}
		#endregion

		#region Incomes
		[HttpPost(ApiRoutes.UserBudget.NewIncome)]
		public async Task<IActionResult> NewIncome([FromRoute] int budgetId, int periodId, [FromBody] IncomeRequest request)
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


			var period = await _budgetService.GetPeriodAsync(budgetId, periodId);
			if (period == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no Period with id: {periodId} in Budget with id: {budgetId}" }));
			}


			var income = await _budgetService.NewIncomeAsync(periodId, request);
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


			var income = await _budgetService.GetIncomeAsync(periodId, incomeId);
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

			var incomeInDb = await _budgetService.GetIncomeAsync(periodId, incomeId);

			if(incomeInDb == null)
            {
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no income with id: {incomeId} for period id: {periodId}" }));
			}

			var updateIncome = await _budgetService.UpdateIncomeAsync(periodId, request);

            if (!updateIncome)
            {
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not update income with id:{incomeId}" }));
            }

			var updatedIncome = await _budgetService.GetIncomeAsync(request.PeriodId, incomeId);

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

			var incomeInDb = await _budgetService.GetIncomeAsync(periodId, incomeId);
			if(incomeInDb == null)
            {
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is not income with id:{incomeId}" }));
            }


			var deleteIncome = await _budgetService.DeleteIncomeAsync(incomeInDb);
			if (!deleteIncome)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not delete income with id:{incomeId}" }));
			}

			return NoContent();
		}
        #endregion

        #region Debts



        #endregion
    }
}
