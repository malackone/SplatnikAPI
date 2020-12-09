using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Splatnik.API.Services.Interfaces;
using Splatnik.API.Settings;
using Splatnik.Data.Database.DbModels;
using Splatnik.Data.Domain;
using Splatnik.Data.Repositories.Interfaces;

namespace Splatnik.API.Services
{
	public class IdentityService : IIdentityService
	{
		private readonly JwtSettings _jwtSettings;
		private readonly TokenValidationParameters _tokenValidationParameters;
		private readonly IIdentityRepository _identityRepository;
		private readonly IEmailService _emailService;

		public IdentityService(JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters, IIdentityRepository identityRepository, IEmailService emailService)
		{
			_jwtSettings = jwtSettings;
			_tokenValidationParameters = tokenValidationParameters;
			_identityRepository = identityRepository;
			_emailService = emailService;
		}


		public async Task<AuthenticationResult> RegisterAsync(string email, string password)
		{
			var existingUser = await _identityRepository.FindByEmailAsync(email);

			if(existingUser != null)
			{
				return new AuthenticationResult
				{
					Errors = new[] { "There is already user with this email" }
				};
			}

			var newUser = new IdentityUser
			{
				Id = Guid.NewGuid().ToString(),
				Email = email,
				UserName = email
			};

			var createdUser = await _identityRepository.CreateAsync(newUser, password);
			
			if (!createdUser.Succeeded)
			{
				return new AuthenticationResult
				{
					Errors = createdUser.Errors.Select(x => x.Description)
				};
			}

			// Assign default role "User" to new registered user
			var userInDb = await _identityRepository.FindByEmailAsync(email);
			var defaultUserRoleToAssign = await _identityRepository.FindByNameAsync("User");
			await _identityRepository.AssingRoleToUserAsync(userInDb, defaultUserRoleToAssign);


			// generate confirmation token and send email
			var token = await _identityRepository.GenerateEmailConfirmationToken(userInDb);

			var confirmEmailResponse = await _emailService.SendEmailAsync(userInDb.Email, token);

			var response = confirmEmailResponse.Body.ReadAsStringAsync();

			return await GenerateAuthenticationResultForUserAsync(newUser);
		}

		public async Task<AuthenticationResult> LoginAsync(string email, string password)
		{
			var user = await _identityRepository.FindByEmailAsync(email);

			if(user == null)
			{
				return new AuthenticationResult
				{
					Errors = new[] { "User does not exist" }
				};
			}

			var userHasValidPassword = await _identityRepository.CheckPasswordAsync(user, password);

			if (!userHasValidPassword)
			{
				return new AuthenticationResult
				{
					Errors = new[] { "User/Password is not valid" }
				};
			}

			return await GenerateAuthenticationResultForUserAsync(user);


		}

		public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
		{
			var validatedToken = GetPrincipalFromToken(token);

			if(validatedToken == null)
			{
				return new AuthenticationResult
				{
					Errors = new[] { "Invalid Token" }
				};
			}

			var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

			var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);

			if(expiryDateTimeUtc > DateTime.UtcNow)
			{
				return new AuthenticationResult
				{
					Errors = new[] { "This token has not expired yet" }
				};
			}

			var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

			var storedRefreshToken = await _identityRepository.GetRefreshTokenAsync(refreshToken);

			if(storedRefreshToken == null)
			{
				return new AuthenticationResult
				{
					Errors = new[] { "This refresh token does not exist" }
				};
			}

			if(DateTime.UtcNow > storedRefreshToken.ExpiryDate)
			{
				return new AuthenticationResult
				{
					Errors = new[] { "This refresh token has expired" }
				};
			}

			if (storedRefreshToken.Used)
			{
				return new AuthenticationResult
				{
					Errors = new[] { "This refresh token has been used" }
				};
			}

			if(storedRefreshToken.JwtId != jti)
			{
				return new AuthenticationResult
				{
					Errors = new[] { "This refresh tiken does not match this JWT" }
				};
			}

			storedRefreshToken.Used = true;
			await _identityRepository.RefreshTokenUpdateAsync(storedRefreshToken);

			var user = await _identityRepository.FindByIdAsync(validatedToken);

			return await GenerateAuthenticationResultForUserAsync(user);

		}

		public async Task<bool> CheckIfUserExists(string userId)
		{
			var user = await _identityRepository.FindUserByIdAsync(userId);

			if (user == null)
				return false;
			return true;
		}

		public async Task<IdentityResult> ConfirmEmail(string username, string token)
		{
			var user = await _identityRepository.FindByEmailAsync(username);

			return await _identityRepository.ConfirmEmail(user, token);
		}

		private ClaimsPrincipal GetPrincipalFromToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			try
			{
				var tokenValidationParameters = _tokenValidationParameters.Clone();
				tokenValidationParameters.ValidateLifetime = false;
				var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

				if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
				{
					return null;
				}

				return principal;
			}

			catch
			{
				return null;
			}
		}

		private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
		{
			return (validatedToken is JwtSecurityToken jwtSecurityToken) && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
		}

		private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(IdentityUser user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim("id", user.Id)
			};

			var userClaims = await _identityRepository.GetClaimsAsync(user);

			claims.AddRange(userClaims);

			var userRoles = await _identityRepository.GetRolesAsync(user);
			foreach (var userRole in userRoles)
			{
				claims.Add(new Claim(ClaimTypes.Role, userRole));

				var role = await _identityRepository.FindByNameAsync(userRole);
				if (role == null)
					continue;

				var roleClaims = await _identityRepository.GetClaimsAsync(role);

				foreach (var roleClaim in roleClaims)
				{
					if (claims.Contains(roleClaim))
						continue;

					claims.Add(roleClaim);
				}
			}

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			var refreshToken = new RefreshToken
			{
				JwtId = token.Id,
				UserId = user.Id,
				CreationDate = DateTime.UtcNow,
				ExpiryDate = DateTime.UtcNow.AddDays(7)
			};

			await _identityRepository.AddTokenAsync(refreshToken);

			return new AuthenticationResult
			{
				Success = true,
				Token = tokenHandler.WriteToken(token),
				RefreshToken = refreshToken.Token
			};


		}
		
		
		/*
		private SecurityToken GenerateEmailConfirmationJwtToken(IdentityUser user, string identityToken)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Sub, user.Email),
				new Claim("id", user.Id),
				new Claim("identityToken",identityToken)
			};

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			return tokenHandler.CreateToken(tokenDescriptor);

		}

		*/
	}
}
