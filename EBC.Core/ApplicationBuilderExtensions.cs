using AspNetCoreRateLimit;
using EBC.Core.Helpers.StartupFinders;
using EBC.Core.Middlewares;
using Hangfire;
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

        // MiniProfiler Middleware aktiv edilməsi
        app.UseMiniProfiler();

        // Health Checks
        UseHealthChecks(app);

        // WatchDog
        UseWatchDog(app);

        //Hangfire
        UseHangfire(app);

        //RateLimit
        UseRateLimit(app);

        //OrganizationAddressFinder sinifini işə salır və URL-ləri bazaya əlavə edir.
        await OrganizationAddressFinder.GenerateAsync(app);

        return app;
    }


    #region Optional

    private static void UseRateLimit(IApplicationBuilder app)
    {
        if (!ServiceOptions.UseRateLimiting)
            return;

        app.UseIpRateLimiting();
    }

    private static void UseHangfire(IApplicationBuilder app)
    {
        if (!ServiceOptions.UseHangfire)
            return;

        app.UseHangfireDashboard();
    }

    private static void UseWatchDog(IApplicationBuilder app)
    {
        if (!ServiceOptions.UseWatchDog)
            return;

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
    }

    private static void UseHealthChecks(IApplicationBuilder app)
    {
        if (!ServiceOptions.UseHealthChecks)
            return;

        // Health Checks endpointi
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        // Health Checks UI
        app.UseHealthChecksUI(config => config.UIPath = "/health-ui");
    }

    private static void UseDefaultCors(IApplicationBuilder app)
    {
        if (!ServiceOptions.UseDefaultCors)
            return;

        app.UseCors("AllowAllOrigins");

    }

    #endregion
}
