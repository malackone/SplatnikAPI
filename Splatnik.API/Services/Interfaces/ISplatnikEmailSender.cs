using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Services.Interfaces
{
	public interface ISplatnikEmailSender
	{
		Task SendConfirmationEmail(string email, string token);
	}
}
