using Microsoft.Extensions.DependencyInjection;
using Vkr.Application.Interfaces.Infractructure;
namespace Vkr.Infractructure;

public static class DependecyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();

        return services;
    }
}