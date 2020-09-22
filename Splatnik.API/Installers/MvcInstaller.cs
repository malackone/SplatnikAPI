using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Splatnik.API.Filters;
using Splatnik.API.Security;
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

			var sendGridSettings = new SendGridSettings();
			configuration.Bind(nameof(sendGridSettings), sendGridSettings);

			services.AddSingleton(jwtSettings);
			services.AddSingleton(sendGridSettings);

			services.AddScoped<IIdentityRepository, IdentityRepository>();
			services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IBudgetRepository, BudgetRepository>();
            services.AddScoped<IDebtRepository, DebtRepository>();
            services.AddScoped<IPeriodRepository, PeriodRepository>();
            services.AddScoped<IIncomeRepository, IncomeRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IDebtPaymentRepository, DebtPaymentRepository>();
            services.AddScoped<ICreditRepository, CreditRepository>();

			services.AddScoped<IAdminService, AdminService>();
			services.AddScoped<IEmailService, EmailSender>();
			services.AddScoped<IIdentityService, IdentityService>();
			services.AddScoped<IBudgetService, BudgetService>();
			services.AddScoped<IDebtService, DebtService>();
			services.AddScoped<IExpenseService, ExpenseService>();
			services.AddScoped<IIncomeService, IncomeService>();
			services.AddScoped<IPeriodService, PeriodService>();
			services.AddScoped<ICreditService, CreditService>();
			//services.AddScoped<IDebtPaymentService, DebtPaymentService>();

			services.AddControllers();

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

			services.AddSingleton(tokenValidationParameters);

			services.AddAuthorization(options =>
			{
				options.AddPolicy(SecurityPolicies.Admin, policy => { policy.RequireRole(SecurityPolicies.Admin); });

				options.AddPolicy(SecurityPolicies.User, policy =>{ policy.RequireRole(SecurityPolicies.User); });
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
