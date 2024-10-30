using EBC.Core.Helpers.StartupFinders;
using EBC.Core.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EBC.Core;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Core hissede olan middleware və xidmətlərin təyin edilə bilməsi üçün extention method.
    /// </summary>
    /// <param name="app">IApplicationBuilder instansı.</param>
    /// <returns>Əlavə edilmiş middleware və xidmətlər ilə IApplicationBuilder.</returns>
    public static IApplicationBuilder UseCoreLayerCustomApplication(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalErrorHandlingMiddleware>();

        //OrganizationAddressFinder sinifini işə salır və URL-ləri bazaya əlavə edir.
        OrganizationAddressFinder.GenerateAsync(app).GetAwaiter().GetResult();

        return app;
    }

    public static async Task AddSeedDataAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var seedData = scope.ServiceProvider.GetRequiredService<EBC.Core.SeedData.SeedData>();
        await seedData.WriteSeedDataAsync();
    }
}
