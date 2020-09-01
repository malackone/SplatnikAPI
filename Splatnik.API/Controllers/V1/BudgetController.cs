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
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class BudgetController : Controller
	{
		private readonly IBudgetService _budgetService;
		private readonly IMapper _mapper;
		private readonly IUriService _uriService;

		public BudgetController(IBudgetService budgetService, IMapper mapper, IUriService uriService)
		{
			_budgetService = budgetService;
			_mapper = mapper;
			_uriService = uriService;
		}

		[HttpPost(ApiRoutes.Budget.Create)]
		public async Task<IActionResult> Create([FromBody] CreateBudgetRequest request)
		{
			var budget = await _budgetService.CreateBudgetAsync(request);

			if(budget == null)
			{
				return BadRequest();
			}

			var locationUri = _uriService.GetBudgetUri(budget.ToString());

			return Created(locationUri, new Response<BudgetResponse>(_mapper.Map<BudgetResponse>(budget)));

		}


		[HttpGet(ApiRoutes.Budget.Get)]
		public async Task<IActionResult> Get([FromRoute] int budgetId)
		{
			var budget = await _budgetService.GetBudgetAsync(budgetId);

			if(budget == null)
			{
				return NotFound();
			}

			return Ok(new Response<BudgetResponse>(_mapper.Map<BudgetResponse>(budget)));
		}
	}
}
