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
using System.Threading.Tasks;

namespace Splatnik.API.Controllers.V1
{
    [Authorize(Policy = SecurityPolicies.User, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class DebtController : ControllerBase 
    {
		private readonly IMapper _mapper;
		private readonly IUriService _uriService;
		private readonly IIdentityService _identityService;
		private readonly IBudgetService _budgetService;
		private readonly IDebtService _debtService; 
		private readonly IDebtPaymentService _debtPaymentService;


		public DebtController(IMapper mapper, IUriService uriService, IIdentityService identityService, IBudgetService budgetService, IDebtService debtService, IDebtPaymentService debtPaymentService)
		{
			_mapper = mapper;
			_uriService = uriService;
			_identityService = identityService;
			_budgetService = budgetService;
			_debtService = debtService;
			_debtPaymentService = debtPaymentService;
		}


		[HttpPost(ApiRoutes.UserDebts.CreateDebt)]
		public async Task<IActionResult> CreateDebt([FromBody] DebtRequest request)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			// check if new debt budgetId is correct
			var budget = await _budgetService.GetBudgetAsync(request.BudgetId);
			if (budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {request.BudgetId}" }));
			}

			if (budget.UserId != userId)
			{
				return Forbid();
			}

			// create debt
			var newDebt = await _debtService.NewDebtAsync(request, userId);

			if (newDebt == null)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = "Could not create new debt" }));
			}

			var locationUri = _uriService.GetDebtUri(newDebt.Id);

			return Created(locationUri, new Response<DebtResponse>(_mapper.Map<DebtResponse>(newDebt)));

		}


		[HttpGet(ApiRoutes.UserDebts.BudgetDebt)]
		public async Task<IActionResult> GetDebt([FromRoute] int debtId)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var debt = await _debtService.GetDebtAsync(debtId);
			if (debt == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no debt with id: {debtId}" }));
			}

			if (debt.UserId != userId)
			{
				return Forbid();
			}

			return Ok(new Response<DebtResponse>(_mapper.Map<DebtResponse>(debt)));
		}


		[HttpGet(ApiRoutes.UserDebts.BudgetDebts)]
		public async Task<IActionResult> GetDebts()
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var debts = await _debtService.GetDebtsByUserIdAsync(userId);

			if (debts == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no debt" }));
			}

			return Ok(new Response<IList<DebtResponse>>(_mapper.Map<IList<DebtResponse>>(debts)));
		}


		[HttpPatch(ApiRoutes.UserDebts.BudgetDebt)]
		public async Task<IActionResult> UpdateDebt([FromRoute] int debtId, [FromBody] UpdateDebtRequest request)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var debtInDb = await _debtService.GetDebtAsync(debtId);
			if (debtInDb == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no debt with id: {debtId}" }));
			}

			if (debtInDb.UserId != userId)
			{
				return Forbid();
			}

			var updateDebt = await _debtService.UpdateDebtAsync(debtId, request);
			if (!updateDebt)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not update debt with id:{debtId}" }));
			}

			var updatedDebt = await _debtService.GetDebtAsync(debtId);
			return Ok(new Response<DebtResponse>(_mapper.Map<DebtResponse>(updatedDebt)));
		}


		[HttpDelete(ApiRoutes.UserDebts.BudgetDebt)]
		public async Task<IActionResult> DeleteDebt([FromRoute] int debtId)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var debtInDb = await _debtService.GetDebtAsync(debtId);
			if (debtInDb == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is not debt with id:{debtId}" }));
			}

			if (debtInDb.UserId != userId)
			{
				return Forbid();
			}

			var deleteDebt = await _debtService.DeleteDebtAsync(debtInDb);
			if (!deleteDebt)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not delete debt with id:{debtId}" }));
			}

			return NoContent();
		}

		[HttpPost(ApiRoutes.UserDebts.CreateDebtPayment)]
		public async Task<IActionResult> CreateDebtPayment([FromRoute] int debtId, [FromBody] DebtPaymentRequest request)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			// check if new debt budgetId is correct
			var debt = await _debtService.GetDebtAsync(debtId);
			if (debt == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {debtId}" }));
			}

			if (debt.UserId != userId)
			{
				return Forbid();
			}

			// create debtPayment
			var newDebtPayment = await _debtPaymentService.NewDebtPaymentAsync(request, debtId, userId);

			if (newDebtPayment == null)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = "Could not create new debt payment" }));
			}

			var locationUri = _uriService.GetDebtPaymentUri(debtId, newDebtPayment.Id);

			return Created(locationUri, new Response<DebtPaymentResponse>(_mapper.Map<DebtPaymentResponse>(newDebtPayment)));
		}


		[HttpGet(ApiRoutes.UserDebts.BudgetDebtPayment)]
		public async Task<IActionResult> GetDebtPayment([FromRoute] int debtId, int debtPaymentId)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var debtPayment = await _debtPaymentService.GetDebtPaymentAsync(debtPaymentId);
			if (debtPayment == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no debtPayment with id: {debtPaymentId} in Debt with id: {debtId}" }));
			}

			if (debtPayment.UserId != userId)
			{
				return Forbid();
			}

			return Ok(new Response<DebtPaymentResponse>(_mapper.Map<DebtPaymentResponse>(debtPayment)));
		}


		[HttpGet(ApiRoutes.UserDebts.BudgetDebtPayments)]
		public async Task<IActionResult> GetDebtPayments([FromRoute] int debtId)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var debt = await _debtService.GetDebtAsync(debtId);
			if (debt == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no debt with id: {debtId}" }));
			}

			if (debt.UserId != userId)
			{
				return Forbid();
			}

			var debtPayments = await _debtPaymentService.GetDebtPaymentsAsync(debtId);
			if (debtPayments == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no debt payments in Debt with id: {debtId}" }));
			}

			return Ok(new Response<IList<DebtPaymentResponse>>(_mapper.Map<IList<DebtPaymentResponse>>(debtPayments)));
		}


		[HttpPatch(ApiRoutes.UserDebts.UpdateBudgetDebtPayment)]
		public async Task<IActionResult> UpdateDebtPayment([FromRoute] int debtId, int debtPaymentId, [FromBody] UpdateDebtPaymentRequest request)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var debt = await _debtService.GetDebtAsync(debtId);
			if (debt == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {debtId}" }));
			}

			if (debt.UserId != userId)
			{
				return Forbid();
			}

			var debtPaymentInDb = await _debtPaymentService.GetDebtPaymentAsync(debtPaymentId);
			if (debtPaymentInDb == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no debtPayment with id: {debtPaymentId} for debt id: {debtId}" }));
			}

			var updateDebt = await _debtPaymentService.UpdateDebtPaymentAsync(debtPaymentId, request);
			if (!updateDebt)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not update debt payment with id:{debtPaymentId}" }));
			}

			var updatedDebtPayment = await _debtPaymentService.GetDebtPaymentAsync(debtPaymentId);
			return Ok(new Response<DebtPaymentResponse>(_mapper.Map<DebtPaymentResponse>(updatedDebtPayment)));
		}


		[HttpDelete(ApiRoutes.UserDebts.DeleteBudgetDebtPayment)]
		public async Task<IActionResult> DeleteDebtPayment([FromRoute] int debtId, int debtPaymentId)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var debt = await _debtService.GetDebtAsync(debtId);
			if (debt == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {debtId}" }));
			}

			if (debt.UserId != userId)
			{
				return Forbid();
			}

			var debtPaymentInDb = await _debtPaymentService.GetDebtPaymentAsync(debtPaymentId);
			if (debtPaymentInDb == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is not debt payment with id:{debtPaymentId}" }));
			}


			var deleteDebtPayment = await _debtPaymentService.DeleteDebtPaymentAsync(debtPaymentInDb);
			if (!deleteDebtPayment)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not delete debt payment with id:{debtPaymentId}" }));
			}

			return NoContent();
		}

	}
}
