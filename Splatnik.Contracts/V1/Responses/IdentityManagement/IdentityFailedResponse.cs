using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Splatnik.Contracts.V1.Responses.IdentityManagement
{
	public class IdentityFailedResponse
	{
		public IEnumerable<IdentityError> Errors { get; set; }
	}
}
