using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EBC.Data;

public static class DataApplicationBuilderExtensions
{
    public static async Task AddSeedDataAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var seedData = scope.ServiceProvider.GetRequiredService<EBC.Data.SeedData.SeedData>();
        await seedData.WriteSeedDataAsync();
    }
}
