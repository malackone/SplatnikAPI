using System;
using System.Collections.Generic;
using System.Text;

namespace Splatnik.Contracts.V1.Requests.IdentityManagement
{
	public class AssignRoleToUserRequest
	{
		public string Username { get; set; }
		public string RoleName { get; set; }
	}
}
