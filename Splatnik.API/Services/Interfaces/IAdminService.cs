using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Services.Interfaces
{
	public interface IAdminService
	{
		Task<IdentityResult> CreateRoleAsync(string roleName);
		Task<IdentityResult> AssignUserToRole(string username, string roleName);
	}
}
