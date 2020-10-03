using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Splatnik.API.Security;
using Splatnik.API.Services.Interfaces;
using Splatnik.Contracts.V1;
using Splatnik.Contracts.V1.Requests;
using Splatnik.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Splatnik.API.Controllers.V1
{
    [Authorize(Policy = SecurityPolicies.User, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ExpenseController : ControllerBase
    {
		private readonly IMapper _mapper;
		private readonly IUriService _uriService;
		private readonly IIdentityService _identityService;
		private readonly IBudgetService _budgetService;
		private readonly IPeriodService _periodService;
		private readonly IExpenseService _expenseService;


		public ExpenseController(IMapper mapper, IUriService uriService, IIdentityService identityService, IBudgetService budgetService, IPeriodService periodService, IExpenseService expenseService)
		{
			_mapper = mapper;
			_uriService = uriService;
			_identityService = identityService;
			_budgetService = budgetService;
			_periodService = periodService;
			_expenseService = expenseService;
		}


        [HttpPost(ApiRoutes.UserExpenses.CreateExpense)]
		public async Task<IActionResult> CreateExpense([FromBody] ExpenseRequest request)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var budget = await _budgetService.GetBudgetAsync(request.BudgetId);
			if (budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {request.BudgetId}" }));
			}

			if (budget.UserId != userId)
			{
				return Forbid();
			}

			var period = await _periodService.GetPeriodAsync(request.PeriodId);

			if (period == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no Period with id: {request.PeriodId} in Budget with id: {request.BudgetId}" }));
			}

			var expense = await _expenseService.NewExpenseAsync(request, userId);
			if (expense == null)
			{
				return BadRequest();
			}

			var locationUri = _uriService.GetExpenseUri(expense.Id);

			return Created(locationUri, new Response<ExpenseResponse>(_mapper.Map<ExpenseResponse>(expense)));


		}


		[HttpGet(ApiRoutes.UserExpenses.BudgetPeriodExpense)]
		public async Task<IActionResult> GetExpense([FromRoute]int expenseId)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var expense = await _expenseService.GetExpenseAsync(expenseId);
			if (expense == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no expense with id: {expenseId}" }));
			}

			if (expense.UserId != userId)
			{
				return Forbid();
			}


			return Ok(new Response<ExpenseResponse>(_mapper.Map<ExpenseResponse>(expense)));
		}


		[HttpPatch(ApiRoutes.UserExpenses.UpdateExpense)]
		public async Task<IActionResult> UpdateExpense(int expenseId, [FromBody] UpdateExpenseRequest request)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var expenseInDb = await _expenseService.GetExpenseAsync(expenseId);
			if (expenseInDb == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no expense with id: {expenseId}" }));
			}

			if (expenseInDb.UserId != userId)
			{
				return Forbid();
			}

			var updateExpense = await _expenseService.UpdateExpenseAsync(expenseId, request);
			if (!updateExpense)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not update expense with id:{expenseId}" }));
			}

			var updatedExpense = await _expenseService.GetExpenseAsync(expenseId);

			return Ok(new Response<ExpenseResponse>(_mapper.Map<ExpenseResponse>(updatedExpense)));

		}


		[HttpDelete(ApiRoutes.UserExpenses.DeleteExpense)]
		public async Task<IActionResult> DeleteExpense([FromRoute] int expenseId)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var expenseInDb = await _expenseService.GetExpenseAsync(expenseId);
			if (expenseInDb == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no expense with id:{expenseId}" }));
			}

			if (expenseInDb.UserId != userId)
			{
				return Forbid();
			}

			var deleteExpense = await _expenseService.DeleteExpenseAsync(expenseInDb);
			if (!deleteExpense)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not delete expense with id: {expenseId}" }));
			}

			return NoContent();
		}

	}
}
