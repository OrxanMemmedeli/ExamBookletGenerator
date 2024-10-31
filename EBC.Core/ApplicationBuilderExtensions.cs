using EBC.Core.Helpers.StartupFinders;
using EBC.Core.Middlewares;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using WatchDog;
using WatchDog.src.Enums;

namespace EBC.Core;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Core hissede olan middleware və xidmətlərin təyin edilə bilməsi üçün extention method.
    /// </summary>
    /// <param name="app">IApplicationBuilder instansı.</param>
    /// <returns>Əlavə edilmiş middleware və xidmətlər ilə IApplicationBuilder.</returns>
    public static async Task<IApplicationBuilder> UseCoreLayerCustomApplication(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalErrorHandlingMiddleware>();

        app.UseMiniProfiler();

        // MiniProfiler Middleware aktiv edilməsi
        app.UseMiniProfiler();

        // Health Checks endpointi
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        // Health Checks UI
        app.UseHealthChecksUI(config => config.UIPath = "/health-ui");

        // WatchDog Middleware aktiv edilməsi
        app.UseWatchDogExceptionLogger();
        app.UseWatchDog(config =>
        {
            config.WatchPageUsername = "admin";
            config.WatchPagePassword = "Qwerty@123";
            //Optional
            config.Blacklist = "/health, /health-ui, /profiler"; //Prevent logging for specified endpoints
            config.Serializer = WatchDogSerializerEnum.Newtonsoft; //If your project use a global json converter
            config.CorsPolicy = "MyCorsPolicy";
            config.UseOutputCache = true;
            config.UseRegexForBlacklisting = true;
        });


        //OrganizationAddressFinder sinifini işə salır və URL-ləri bazaya əlavə edir.
        await OrganizationAddressFinder.GenerateAsync(app);

        return app;
    }

    public static async Task AddSeedDataAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var seedData = scope.ServiceProvider.GetRequiredService<EBC.Core.SeedData.SeedData>();
        await seedData.WriteSeedDataAsync();
    }
}
