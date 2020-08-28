using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Splatnik.API.Filters;
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
			services.AddScoped<IBudgetService, BudgetService>();
			services.AddScoped<IBudgetRepository, BudgetRepository>();


			services
				.AddMvc(options =>
				{
					options.EnableEndpointRouting = false;
					options.Filters.Add<ValidationFilter>();
				})
				.AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Startup>())
				.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

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


			services.AddSingleton<IUriService>(prov =>
			{
				var accessor = prov.GetRequiredService<IHttpContextAccessor>();
				var request = accessor.HttpContext.Request;
				var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), "/");
				return new UriService(absoluteUri);
			});

		}
	}
}
