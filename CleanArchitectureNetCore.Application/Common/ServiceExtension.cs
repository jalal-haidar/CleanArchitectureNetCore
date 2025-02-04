using CleanArchitectureNetCore.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureNetCore.Application.Common
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
           services.AddScoped<AuthService>();
           services.AddScoped<PatientService>();
           services.AddScoped<RecommendationService>();
           services.AddScoped<RoleService>();
           services.AddScoped<UserService>();

        }
    }
}
