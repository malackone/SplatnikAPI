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
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Splatnik.API.Controllers.V1
{
	[Authorize(Policy = SecurityPolicies.User, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class IncomeController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IUriService _uriService;
		private readonly IIdentityService _identityService;
		private readonly IBudgetService _budgetService;
		private readonly IPeriodService _periodService;
		private readonly IIncomeService _incomeService;

		public IncomeController(IMapper mapper, IUriService uriService, IIdentityService identityService, IBudgetService budgetService, IPeriodService periodService, IIncomeService incomeService)
		{
			_mapper = mapper;
			_uriService = uriService;
			_identityService = identityService;
			_budgetService = budgetService;
			_periodService = periodService;
			_incomeService = incomeService;
		}



		[HttpPost(ApiRoutes.UserIncomes.CreateIncome)]
		public async Task<IActionResult> CreateIncome([FromBody] IncomeRequest request)
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


			var income = await _incomeService.NewIncomeAsync(request, userId);
			if (income == null)
			{
				return BadRequest();
			}

			var locationUri = _uriService.GetIncomeUri(income.Id);

			return Created(locationUri, new Response<IncomeResponse>(_mapper.Map<IncomeResponse>(income)));


		}


		[HttpGet(ApiRoutes.UserIncomes.BudgetPeriodIncome)]
		public async Task<IActionResult> GetIncome([FromRoute] int incomeId)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var budgets = await _budgetService.GetUserBudgets(userId);
			
			var income = await _incomeService.GetIncomeAsync(incomeId);
			if (income == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no income with id: {incomeId}"}));
			}






			return Ok(new Response<IncomeResponse>(_mapper.Map<IncomeResponse>(income)));


		}


		[HttpPatch(ApiRoutes.UserIncomes.UpdateIncome)]
		public async Task<IActionResult> UpdateIncome([FromRoute] int incomeId, [FromBody] UpdateIncomeRequest request)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var incomeInDb = await _incomeService.GetIncomeAsync(incomeId);
			if (incomeInDb == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no income with id: {incomeId}" }));
			}

			if (incomeInDb.UserId != userId)
			{
				return Forbid();
			}

			var updateIncome = await _incomeService.UpdateIncomeAsync(incomeId, request);
			if (!updateIncome)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not update income with id:{incomeId}" }));
			}

			var updatedIncome = await _incomeService.GetIncomeAsync(incomeId);

			return Ok(new Response<IncomeResponse>(_mapper.Map<IncomeResponse>(updatedIncome)));
		}


		[HttpDelete(ApiRoutes.UserIncomes.DeleteIncome)]
		public async Task<IActionResult> DeleteIncome([FromRoute] int incomeId)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var incomeInDb = await _incomeService.GetIncomeAsync(incomeId);
			if (incomeInDb == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is not income with id:{incomeId}" }));
			}

			if (incomeInDb.UserId != userId)
			{
				return Forbid();
			}


			var deleteIncome = await _incomeService.DeleteIncomeAsync(incomeInDb);
			if (!deleteIncome)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not delete income with id:{incomeId}" }));
			}

			return NoContent();
		}

	}
}
