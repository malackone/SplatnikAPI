using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Splatnik.Data.Database;
using Splatnik.Data.Database.DbModels;
using Splatnik.Data.Repositories.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Splatnik.Data.Repositories
{
	public class IdentityRepository : IIdentityRepository
	{
		private readonly DataContext _dataContext;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public IdentityRepository(DataContext dataContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_dataContext = dataContext;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task AddTokenAsync(RefreshToken refreshToken)
		{
			await _dataContext.RefreshTokens.AddAsync(refreshToken);
		}

		public async Task<IdentityUser> FindByEmailAsync(string email)
		{
			return await _userManager.FindByEmailAsync(email);
		}

		public async Task<IdentityResult> CreateAsync(IdentityUser newUser, string password)
		{
			return await _userManager.CreateAsync(newUser, password);
		}

		public async Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
		{
			return await _userManager.GetClaimsAsync(user);
		}

		public async Task<IList<string>> GetRolesAsync(IdentityUser user)
		{
			return await _userManager.GetRolesAsync(user);
		}

		public async Task<IdentityRole> FindByNameAsync(string userRole)
		{
			return await _roleManager.FindByNameAsync(userRole);
		}

		public async Task<IList<Claim>> GetClaimsAsync(IdentityRole role)
		{
			return await _roleManager.GetClaimsAsync(role);
		}

		public async Task<bool> CheckPasswordAsync(IdentityUser user, string password)
		{
			return await _userManager.CheckPasswordAsync(user, password);
		}

		public async Task<RefreshToken> GetRefreshTokenAsync(string refreshToken) 
		{
			return await _dataContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
		}

		public async Task RefreshTokenUpdateAsync(RefreshToken storedRefreshToken)
		{
			_dataContext.RefreshTokens.Update(storedRefreshToken);
			await _dataContext.SaveChangesAsync();
		}

		public async Task<IdentityUser> FindByIdAsync(ClaimsPrincipal validatedToken)
		{
			return await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
		}

		public async Task<IdentityResult> CreateRoleAsync(IdentityRole role)
		{

			return await _roleManager.CreateAsync(role);
		}

		public async Task<IdentityResult> AssingRoleToUserAsync(IdentityUser user, IdentityRole role) 
		{
			return await _userManager.AddToRoleAsync(user, role.Name);
		}

		public async Task<IdentityResult> ConfirmEmail(IdentityUser user, string token)
		{
			return await _userManager.ConfirmEmailAsync(user, token);
		}

		public async Task<string> GenerateEmailConfirmationToken(IdentityUser user)
		{
			return await _userManager.GenerateEmailConfirmationTokenAsync(user);
		}
		
		public async Task<IdentityUser> FindUserByIdAsync(string userId)
		{
			return await _userManager.FindByIdAsync(userId);
		}
	}
}
