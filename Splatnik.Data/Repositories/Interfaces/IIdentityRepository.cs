using Splatnik.Data.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Splatnik.Data.Database.DbModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Splatnik.Data.Repositories.Interfaces
{
	public interface IIdentityRepository
	{
		Task<IdentityUser> FindByEmailAsync(string email);
		Task<IdentityResult> CreateAsync(IdentityUser newUser, string password);
		Task AddTokenAsync(RefreshToken refreshToken);
		Task<IList<Claim>> GetClaimsAsync(IdentityUser user);
		Task<IList<string>> GetRolesAsync(IdentityUser user);
		Task<IdentityRole> FindByNameAsync(string userRole);
		Task<IList<Claim>> GetClaimsAsync(IdentityRole role);
		Task<bool> CheckPasswordAsync(IdentityUser user, string password);
		Task<RefreshToken> GetRefreshTokenAsync(string refreshToken);
		Task RefreshTokenUpdateAsync(RefreshToken storedRefreshToken);
		Task<IdentityUser> FindByIdAsync(ClaimsPrincipal validatedToken);
	}
}
