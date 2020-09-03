using Microsoft.AspNetCore.Identity;
using Splatnik.API.Services.Interfaces;
using Splatnik.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Services
{
	public class AdminService : IAdminService
	{
		private readonly IIdentityRepository _identityRepository;

		public AdminService(IIdentityRepository identityRepository)
		{
			_identityRepository = identityRepository;
		}

		public async Task<IdentityResult> CreateRoleAsync(string name)
		{
			var newRole = new IdentityRole
			{
				Name = name,
				Id = Guid.NewGuid().ToString(),
				NormalizedName = name.ToUpper()
			};

			return await _identityRepository.CreateRoleAsync(newRole);
		}

		public async Task<IdentityResult> AssignUserToRole(string username, string roleName)
		{
			var user = await _identityRepository.FindByEmailAsync(username);
			var role = await _identityRepository.FindByNameAsync(roleName);

			return await _identityRepository.AssingRoleToUserAsync(user, role);

		}

	}
}
