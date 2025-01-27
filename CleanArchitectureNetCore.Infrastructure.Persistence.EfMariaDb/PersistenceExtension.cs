using CleanArchitectureNetCore.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CleanArchitectureNetCore.Infrastructure.Persistence.EfMariaDb
{
    public static class PersistenceExtension
  {
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<AppDbContext>(options =>
      {
        options.UseSqlServer(configuration.GetConnectionString("Default"));
      });

      services.AddTransient<IUnitOfWork, UnitOfWork>();

      return services;
    }
  }
}
