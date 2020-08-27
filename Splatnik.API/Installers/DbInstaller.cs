using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Splatnik.Data.Database;

namespace Splatnik.API.Installers
{
	public class DbInstaller : IInstaller
	{
		public void InstallServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<DataContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

			services.AddDefaultIdentity<IdentityUser>()
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<DataContext>();
		}
	}
}
