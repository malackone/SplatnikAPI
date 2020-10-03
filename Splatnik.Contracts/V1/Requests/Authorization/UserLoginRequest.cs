using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Splatnik.Contracts.V1.Requests.Authorization
{
	public class UserLoginRequest
	{
		[EmailAddress]
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
