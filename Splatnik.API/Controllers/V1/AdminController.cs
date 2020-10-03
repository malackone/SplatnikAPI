using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using Splatnik.API.Security;
using Splatnik.API.Services.Interfaces;
using Splatnik.API.Settings;
using Splatnik.Contracts.V1;
using Splatnik.Contracts.V1.Requests.IdentityManagement;
using Splatnik.Contracts.V1.Responses.IdentityManagement;

namespace Splatnik.API.Controllers.V1
{
	[Authorize(Policy = SecurityPolicies.Admin, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class AdminController : Controller
	{
		private readonly IAdminService _adminService;
		private readonly IConfiguration _configuration;

		public AdminController(IAdminService adminService, IConfiguration configuration)
		{
			_adminService = adminService;
			_configuration = configuration;
		}


		[HttpPost(ApiRoutes.Admin.Role)]
		public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
		{
			var roleResponse = await _adminService.CreateRoleAsync(request.Name);

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


		[HttpPost(ApiRoutes.Admin.UserRoles)]
		public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserRequest request)
		{
			var response = await _adminService.AssignUserToRole(request.Username, request.RoleName);

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


		[AllowAnonymous]
		[HttpGet(ApiRoutes.Admin.TestEmail)]
		public async Task<IActionResult> SendEmail()
		{
			var sendGridSettings = new SendGridSettings();
			_configuration.GetSection(nameof(SendGridSettings)).Bind(sendGridSettings);

			var apiKey = sendGridSettings.SendGridKey;
			var client = new SendGridClient(apiKey);
			var from = new EmailAddress("wojteklaskowski89@gmail.com", "Example User");
			var subject = "Sending with SendGrid is Fun";
			var to = new EmailAddress("test@example.com", "Example User");
			var plainTextContent = "and easy to do anywhere, even with C#";
			var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
			var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
			var response = await client.SendEmailAsync(msg);

			return Ok(response.Body.ReadAsStringAsync());
		}
	}
}
