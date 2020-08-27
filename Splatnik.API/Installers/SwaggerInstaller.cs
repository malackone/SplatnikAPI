using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Splatnik.API.Installers
{
	public class SwaggerInstaller : IInstaller
	{
		public void InstallServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddSwaggerGen(s =>
			{
				s.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "Splatnik API",
					Description = "API dla projektu splatnik"
				});
			});
		}
	}
}
