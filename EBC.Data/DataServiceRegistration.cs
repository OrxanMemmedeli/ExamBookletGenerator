using Microsoft.Extensions.DependencyInjection;

namespace EBC.Data;

public static class DataServiceRegistration
{
    public static IServiceCollection AddDataLayerServices(this IServiceCollection services)
    {
        // SeedData
        services.AddScoped<EBC.Data.SeedData.SeedData>();

        return services;
    }
}
