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

		[HttpPost(ApiRoutes.User.NewBudget)]
		public async Task<IActionResult> Create([FromBody] NewBudgetRequest request)
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

			var locationUri = _uriService.GetBudgetUri(budget.ToString());

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
		public async Task<IActionResult> GetUserBudgets()
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
	}
}
