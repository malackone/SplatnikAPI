using System;
using System.Collections.Generic;
using System.Text;

namespace Splatnik.Contracts.V1.Requests.Authorization
{
	public class ConfirmEmailRequest
	{
		public string Email { get; set; }
		public string Token { get; set; }
	}
}
