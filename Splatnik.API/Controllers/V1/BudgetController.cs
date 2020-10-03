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
using System.Runtime.CompilerServices;

namespace Splatnik.API.Controllers.V1
{
	[Authorize(Policy = SecurityPolicies.User, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class BudgetController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IUriService _uriService;
		private readonly IIdentityService _identityService;
		private readonly IBudgetService _budgetService;
		private readonly IPeriodService _periodService;
		
		public BudgetController(IMapper mapper, IUriService uriService, IIdentityService identityService, IBudgetService budgetService, IPeriodService periodService)
		{
			_mapper = mapper;
			_identityService = identityService;
			_uriService = uriService;
			_budgetService = budgetService;
			_periodService = periodService;
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
			var period = await _periodService.NewPeriodAsync(request, userId);

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

			var period = await _periodService.GetPeriodAsync(periodId);

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
    }
}
