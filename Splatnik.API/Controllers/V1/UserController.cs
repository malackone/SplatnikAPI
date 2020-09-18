using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Splatnik.API.Services.Interfaces;
using Splatnik.Contracts.V1;
using Splatnik.Contracts.V1.Requests;
using Splatnik.Contracts.V1.Responses;

namespace Splatnik.API.Controllers.V1
{
	[Authorize(Policy = "User", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class UserController : Controller
	{
		private readonly IBudgetService _budgetService;
		private readonly IIdentityService _identityService;
		private readonly IMapper _mapper;
		private readonly IUriService _uriService;

		public UserController(IBudgetService budgetService, IIdentityService identityService,IMapper mapper, IUriService uriService)
		{
			_budgetService = budgetService;
			_identityService = identityService;
			_mapper = mapper;
			_uriService = uriService;
		}


        #region Budgets
        [HttpPost(ApiRoutes.User.NewBudget)]
		public async Task<IActionResult> NewBudget([FromBody] NewBudgetRequest request)
		{

			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			var user = await _identityService.CheckIfUserExists(userId);
            if (!user)
            {
				return NotFound();
            }

			var budget = await _budgetService.CreateBudgetAsync(request, userId);

			if(budget == null)
			{
				return BadRequest();
			}

			var locationUri = _uriService.GetBudgetUri(budget.Id.ToString());

			return Created(locationUri, new Response<BudgetResponse>(_mapper.Map<BudgetResponse>(budget)));

		}


		[HttpGet(ApiRoutes.User.UserBudget)]
		public async Task<IActionResult> GetBudget([FromRoute] int budgetId)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			var user = await _identityService.CheckIfUserExists(userId);
            if (!user)
            {
				return NotFound();
            }

			var budget = await _budgetService.GetBudgetAsync(budgetId);

			if(budget == null)
			{
				return NotFound();
			}

			return Ok(new Response<BudgetResponse>(_mapper.Map<BudgetResponse>(budget)));
		}


		[HttpGet(ApiRoutes.User.UserBudgets)]
		public async Task<IActionResult> UserBudgets()
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			var user = await _identityService.CheckIfUserExists(userId);
            if (!user)
            {
				return NotFound();
			}

			var budgets = await _budgetService.GetUserBudgets(userId);

			return Ok(new Response<IEnumerable<BudgetResponse>>(_mapper.Map<IEnumerable<BudgetResponse>>(budgets)));
		}
        #endregion

        #region Periods
		[HttpPost(ApiRoutes.User.NewPeriod)]
		public async Task<IActionResult> NewPeriod([FromBody] NewPeriodRequest request)
        {

			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var user = await _identityService.CheckIfUserExists(userId);
			if (!user)
			{
				return NotFound();
			}

			// check if new period budgetId is correct
			var budget = await _budgetService.GetBudgetAsync(request.BudgetId);

			if(budget == null)
            {
				return NotFound();
            }

			if(budget.UserId != userId)
            {
				return BadRequest();
            }


			// Check if new period dates are between any other period dates
			var periodList = await _budgetService.GetBudgetPeriodsAsync(request.BudgetId);
			if (periodList.Any(x => x.FirstDay <= request.FirstDay && x.LastDay >= request.FirstDay))
			{
				return BadRequest(new ErrorResponse(new ErrorModel { FieldName = nameof(request.FirstDay), Message = "First day of new period cannot be between other period dates"}));
			}

			if (periodList.Any(x => x.FirstDay <= request.LastDay && x.LastDay >= request.LastDay))
            {
				return BadRequest(new ErrorResponse(new ErrorModel { FieldName = nameof(request.LastDay), Message = "Last day of new period cannot be between other period dates" }));
			}

			// create new period
			var period = await _budgetService.CreatePeriodAsync(request, userId);

			if(period == null)
            {
				return BadRequest();
            }

			var locationUri = _uriService.GetPeriodUri(period.ToString());

			return Created(locationUri, new Response<PeriodResponse>(_mapper.Map<PeriodResponse>(period)));

		}
        #endregion
    }
}
