using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;
using SendGrid;
using Splatnik.API.Settings;
using Splatnik.API.Services.Interfaces;

namespace Splatnik.API.Services
{
	public class EmailSender : IEmailService
	{
		private readonly IUriService _uriService;
		private readonly SendGridSettings _sendGridSettings;


		public EmailSender(SendGridSettings sendGridSettings, IUriService uriService)
		{
			_uriService = uriService;
			_sendGridSettings = sendGridSettings;
		}


		public Task<Response> SendEmailAsync(string email, string token)
		{
			var subject = "Splatnik | Confirm your email addres";
			var confirmationLink = _uriService.GetConfirmationLink(email, token);
			var message = confirmationLink.AbsoluteUri;
			return Execute(_sendGridSettings.SendGridKey, subject, message, email);
		}


		public async Task<Response> Execute(string apiKey, string subject, string message, string email)
		{
			var client = new SendGridClient(apiKey);
			var msg = new SendGridMessage()
			{
				From = new EmailAddress(_sendGridSettings.SendGridUser),
				Subject = subject,
				PlainTextContent = message,
				HtmlContent = message
			};

			msg.AddTo(new EmailAddress(email));

			msg.SetClickTracking(false, false);
			return await client.SendEmailAsync(msg);

		}
	}
}
