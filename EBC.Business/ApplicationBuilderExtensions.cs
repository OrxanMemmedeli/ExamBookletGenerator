using EBC.Business.Helpers.StartupFinders;
using Microsoft.AspNetCore.Builder;

namespace EBC.Business;

public static class ApplicationBuilderExtensions
{
    public static async Task<IApplicationBuilder> UseBusinessLayerCustomApplication(this IApplicationBuilder app)
    {

        //OrganizationAddressFinder sinifini işə salır və URL-ləri bazaya əlavə edir.
        await OrganizationAddressFinder.GenerateAsync(app);

        return app;
    }
}
