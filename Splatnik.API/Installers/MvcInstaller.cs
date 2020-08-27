using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Splatnik.API.Services;
using Splatnik.API.Services.Interfaces;
using Splatnik.API.Settings;
using Splatnik.Data.Repositories;
using Splatnik.Data.Repositories.Interfaces;
using System.Text;


namespace Splatnik.API.Installers
{
	public class MvcInstaller : IInstaller
	{
		public void InstallServices(IServiceCollection services, IConfiguration configuration)
		{
			var jwtSettings = new JwtSettings();
			configuration.Bind(nameof(jwtSettings), jwtSettings);
			services.AddSingleton(jwtSettings);

			services.AddControllers();

			services.AddScoped<IIdentityService, IdentityService>();
			services.AddScoped<IIdentityRepository, IdentityRepository>();

			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
				ValidateIssuer = false,
				ValidateAudience = false,
				RequireExpirationTime = false,
				ValidateLifetime = true
			};


			services.AddSingleton(tokenValidationParameters);


			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.SaveToken = true;
				x.TokenValidationParameters = tokenValidationParameters;
			});



		}
	}
}
