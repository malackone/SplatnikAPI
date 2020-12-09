using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Splatnik.API.Installers;
using Splatnik.API.Settings;

namespace Splatnik.API
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

		public Startup(IWebHostEnvironment env)
		{
			var builder = new ConfigurationBuilder()
								.SetBasePath(env.ContentRootPath)
								.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
								.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
								.AddUserSecrets<Program>()
								.AddEnvironmentVariables();

			Configuration = builder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.InstallServiceInAssembly(Configuration);
			services.AddAutoMapper(typeof(Startup));

			services.AddCors(options =>
			{
				options.AddPolicy(name: MyAllowSpecificOrigins,
								  builder =>
								  {
									  builder.WithOrigins("http://localhost:4200")
												  .AllowAnyHeader()
												  .AllowAnyMethod();
								  });
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			var swaggerOptions = new SwaggerSettings();
			Configuration.GetSection(nameof(SwaggerSettings)).Bind(swaggerOptions);

			app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
			});
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseCors(MyAllowSpecificOrigins);
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
