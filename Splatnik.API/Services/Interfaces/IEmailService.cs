using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Services.Interfaces
{
	public interface IEmailService
	{
		Task<Response> SendEmailAsync(string email, string token);
		Task<Response> Execute(string apiKey, string subject, string message, string email);

	}
}
