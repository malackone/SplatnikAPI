using System;
using System.Collections.Generic;
using System.Text;

namespace Splatnik.Contracts.V1.Responses
{
	public class AuthSuccesResponse
	{
		public string Token { get; set; }
		public string RefreshToken { get; set; }
	}
}
