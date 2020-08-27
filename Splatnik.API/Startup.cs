using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Splatnik.API.Installers;

namespace Splatnik.API
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			var builder = new ConfigurationBuilder()
								.SetBasePath(env.ContentRootPath)
								.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
								.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
								.AddEnvironmentVariables();

			Configuration = builder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.InstallServiceInAssembly(Configuration);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Splatnik API v1");
			});

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
