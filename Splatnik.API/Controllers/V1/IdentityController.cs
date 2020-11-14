using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Splatnik.API.Services.Interfaces;
using Splatnik.Contracts.V1;
using Splatnik.Contracts.V1.Requests.Authorization;
using Splatnik.Contracts.V1.Responses.Authorization;
using Splatnik.Contracts.V1.Responses.IdentityManagement;

namespace Splatnik.API.Controllers.V1
{
	public class IdentityController : Controller
	{
		private readonly IIdentityService _identityService;

		public IdentityController(IIdentityService identityService)
		{
			_identityService = identityService;
		}

		[HttpPost(ApiRoutes.Identity.Register)]
		public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new AuthFailedResponse
				{
					Errors = ModelState.Values.SelectMany(x => x.Errors.Select(s => s.ErrorMessage))
				});
			}


			var authResponse = await _identityService.RegisterAsync(request.Email, request.Password);

			if (!authResponse.Success)
			{
				return BadRequest(new AuthFailedResponse
				{
					Errors = authResponse.Errors
				});
			}

			return Ok(new AuthSuccesResponse
			{
				Token = authResponse.Token,
				RefreshToken = authResponse.RefreshToken
			});


		}


		[HttpPost(ApiRoutes.Identity.Login)]
		public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
		{
			var authResponse = await _identityService.LoginAsync(request.Email, request.Password);

			if (!authResponse.Success)
			{
				return BadRequest(new AuthFailedResponse
				{
					Errors = authResponse.Errors
				});
			}

			return Ok(new AuthSuccesResponse
			{
				Token = authResponse.Token,
				RefreshToken = authResponse.RefreshToken
			});
		}


		[HttpPost(ApiRoutes.Identity.Refresh)]
		public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
		{
			var authResponse = await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken);

			if (!authResponse.Success)
			{
				return BadRequest(new AuthFailedResponse
				{
					Errors = authResponse.Errors
				});
			}


			return Ok(new AuthSuccesResponse
			{
				Token = authResponse.Token,
				RefreshToken = authResponse.RefreshToken
			});
		}


		[HttpPost(ApiRoutes.Identity.ConfirmEmail)]
		public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
		{

			var confirmation = await _identityService.ConfirmEmail(request.Email, request.Token);

			if (!confirmation.Succeeded)
			{
				return BadRequest(new IdentityFailedResponse
				{
					Errors = confirmation.Errors
				});
			}

			return Ok(new IdentitySuccessResponse
			{
				Message = "Email Confirmed"
			});
		}

	}
}
