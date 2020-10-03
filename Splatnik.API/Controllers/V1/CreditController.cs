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
	public class CreditController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IUriService _uriService;
		private readonly IIdentityService _identityService;
		private readonly IBudgetService _budgetService;
		private readonly ICreditService _creditService;
		private readonly ICreditPaymentService _creditPaymentService;


		public CreditController(IMapper mapper, IUriService uriService, IIdentityService identityService, IBudgetService budgetService, ICreditService creditService, ICreditPaymentService creditPaymentService)
		{
			_mapper = mapper;
			_uriService = uriService;
			_identityService = identityService;
			_budgetService = budgetService;
			_creditService = creditService;
			_creditPaymentService = creditPaymentService;
		}


		[HttpPost(ApiRoutes.UserCredits.CreateCredit)]
		public async Task<IActionResult> CreateCredit([FromBody] CreditRequest request)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			// check if new credit budgetId is correct
			var budget = await _budgetService.GetBudgetAsync(request.BudgetId);
			if (budget == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {request.BudgetId}" }));
			}

			if (budget.UserId != userId)
			{
				return Forbid();
			}

			// create credit
			var newCredit = await _creditService.NewCreditAsync(request, userId);

			if (newCredit == null)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = "Could not create new credit" }));
			}

			var locationUri = _uriService.GetCreditUri(newCredit.Id);

			return Created(locationUri, new Response<CreditResponse>(_mapper.Map<CreditResponse>(newCredit)));

		}


		[HttpGet(ApiRoutes.UserCredits.BudgetCredit)]
		public async Task<IActionResult> GetCredit([FromRoute] int creditId)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var credit = await _creditService.GetCreditAsync(creditId);
			if (credit == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no credit with id: {creditId}" }));
			}

			if (credit.UserId != userId)
			{
				return Forbid();
			}

			return Ok(new Response<CreditResponse>(_mapper.Map<CreditResponse>(credit)));
		}


		[HttpGet(ApiRoutes.UserCredits.BudgetCredits)]
		public async Task<IActionResult> GetCredits()
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var credits = await _creditService.GetUserCreditsASync(userId);

			if (credits == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no credit" }));
			}

			return Ok(new Response<IList<CreditResponse>>(_mapper.Map<IList<CreditResponse>>(credits)));
		}


		[HttpPatch(ApiRoutes.UserCredits.BudgetCredit)]
		public async Task<IActionResult> UpdateCredit([FromRoute] int creditId, [FromBody] UpdateCreditRequest request)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var creditInDb = await _creditService.GetCreditAsync(creditId);
			if (creditInDb == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no credit with id: {creditId}" }));
			}

			if (creditInDb.UserId != userId)
			{
				return Forbid();
			}

			var updateCredit = await _creditService.UpdateCreditAsync(request, creditId);
			if (!updateCredit)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not update credit with id:{creditId}" }));
			}

			var updatedCredit = await _creditService.GetCreditAsync(creditId);
			return Ok(new Response<CreditResponse>(_mapper.Map<CreditResponse>(updatedCredit)));
		}


		[HttpDelete(ApiRoutes.UserCredits.BudgetCredit)]
		public async Task<IActionResult> DeleteCredit([FromRoute] int creditId)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var creditInDb = await _creditService.GetCreditAsync(creditId);
			if (creditInDb == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is not credit with id:{creditId}" }));
			}

			if (creditInDb.UserId != userId)
			{
				return Forbid();
			}

			var deleteCredit = await _creditService.DeleteCreditAsync(creditInDb);
			if (!deleteCredit)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not delete credit with id:{creditId}" }));
			}

			return NoContent();
		}

		[HttpPost(ApiRoutes.UserCredits.CreateCreditPayment)]
		public async Task<IActionResult> CreateCreditPayment([FromRoute] int creditId, [FromBody] CreditPaymentRequest request)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			// check if new credit budgetId is correct
			var credit = await _creditService.GetCreditAsync(creditId);
			if (credit == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {creditId}" }));
			}

			if (credit.UserId != userId)
			{
				return Forbid();
			}

			// create creditPayment
			var newCreditPayment = await _creditPaymentService.CreateCreditPaymentAsync(request, creditId, userId);

			if (newCreditPayment == null)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = "Could not create new credit payment" }));
			}

			var locationUri = _uriService.GetCreditPaymentUri(creditId, newCreditPayment.Id);

			return Created(locationUri, new Response<CreditPaymentResponse>(_mapper.Map<CreditPaymentResponse>(newCreditPayment)));
		}


		[HttpGet(ApiRoutes.UserCredits.BudgetCreditPayment)]
		public async Task<IActionResult> GetCreditPayment([FromRoute] int creditId, int creditPaymentId)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var creditPayment = await _creditPaymentService.GetCreditPaymentAsync(creditPaymentId);
			if (creditPayment == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no creditPayment with id: {creditPaymentId} in Credit with id: {creditId}" }));
			}

			if (creditPayment.UserId != userId)
			{
				return Forbid();
			}

			return Ok(new Response<CreditPaymentResponse>(_mapper.Map<CreditPaymentResponse>(creditPayment)));
		}


		[HttpGet(ApiRoutes.UserCredits.BudgetCreditPayments)]
		public async Task<IActionResult> GetCreditPayments([FromRoute] int creditId)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var credit = await _creditService.GetCreditAsync(creditId);
			if (credit == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no credit with id: {creditId}" }));
			}

			if (credit.UserId != userId)
			{
				return Forbid();
			}

			var creditPayments = await _creditPaymentService.GetCreditPaymentsAsync(creditId, userId);
			if (creditPayments == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no credit payments in Credit with id: {creditId}" }));
			}

			return Ok(new Response<IList<CreditPaymentResponse>>(_mapper.Map<IList<CreditPaymentResponse>>(creditPayments)));
		}


		[HttpPatch(ApiRoutes.UserCredits.UpdateBudgetCreditPayment)]
		public async Task<IActionResult> UpdateCreditPayment([FromRoute] int creditId, int creditPaymentId, [FromBody] UpdateCreditPaymentRequest request)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var credit = await _creditService.GetCreditAsync(creditId);
			if (credit == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {creditId}" }));
			}

			if (credit.UserId != userId)
			{
				return Forbid();
			}

			var creditPaymentInDb = await _creditPaymentService.GetCreditPaymentAsync(creditPaymentId);
			if (creditPaymentInDb == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no creditPayment with id: {creditPaymentId} for credit id: {creditId}" }));
			}

			var updateCredit = await _creditPaymentService.UpdateCreditPaymentAsync(request, creditPaymentId);
			if (!updateCredit)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not update credit payment with id:{creditPaymentId}" }));
			}

			var updatedCreditPayment = await _creditPaymentService.GetCreditPaymentAsync(creditPaymentId);
			return Ok(new Response<CreditPaymentResponse>(_mapper.Map<CreditPaymentResponse>(updatedCreditPayment)));
		}


		[HttpDelete(ApiRoutes.UserCredits.DeleteBudgetCreditPayment)]
		public async Task<IActionResult> DeleteCreditPayment([FromRoute] int creditId, int creditPaymentId)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == "id").Value;

			// check if user exists
			var userExists = await _identityService.CheckIfUserExists(userId);
			if (!userExists)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no user with id: {userId}" }));
			}

			var credit = await _creditService.GetCreditAsync(creditId);
			if (credit == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is no budget with id: {creditId}" }));
			}

			if (credit.UserId != userId)
			{
				return Forbid();
			}

			var creditPaymentInDb = await _creditPaymentService.GetCreditPaymentAsync(creditPaymentId);
			if (creditPaymentInDb == null)
			{
				return NotFound(new ErrorResponse(new ErrorModel { Message = $"There is not credit payment with id:{creditPaymentId}" }));
			}


			var deleteCreditPayment = await _creditPaymentService.DeleteCreditPaymentAsync(creditPaymentInDb);
			if (!deleteCreditPayment)
			{
				return BadRequest(new ErrorResponse(new ErrorModel { Message = $"Could not delete credit payment with id:{creditPaymentId}" }));
			}

			return NoContent();
		}
	}
}
