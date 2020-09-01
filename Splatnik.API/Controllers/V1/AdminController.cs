using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Splatnik.API.Services.Interfaces;
using Splatnik.Contracts.V1;
using Splatnik.Contracts.V1.Requests.IdentityManagement;
using Splatnik.Contracts.V1.Responses.IdentityManagement;

namespace Splatnik.API.Controllers.V1
{
	[Authorize(Policy = "Admin")]
	public class AdminController : Controller
	{
		private readonly IIdentityService _identityService;

		public AdminController(IIdentityService identityService)
		{
			_identityService = identityService;
		}


		[HttpPost(ApiRoutes.Identity.CreateRole)]
		public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
		{
			var roleResponse = await _identityService.CreateRoleAsync(request.Name);

			if (!roleResponse.Succeeded)
			{
				return BadRequest(new IdentityFailedResponse
				{
					Errors = roleResponse.Errors
				});
			}

			return Ok(new IdentitySuccessResponse
			{
				Message = String.Concat("\"", request.Name, "\" role created")
			});
		}


		[HttpPost(ApiRoutes.Identity.AssignRoleToUser)]
		public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserRequest request)
		{
			var response = await _identityService.AssignUserToRole(request.Username, request.RoleName);

			if (!response.Succeeded)
			{
				return BadRequest(new IdentityFailedResponse
				{
					Errors = response.Errors
				});
			}

			return Ok(new IdentitySuccessResponse
			{
				Message = "Role assinged to user"
			});
		}
	}
}
