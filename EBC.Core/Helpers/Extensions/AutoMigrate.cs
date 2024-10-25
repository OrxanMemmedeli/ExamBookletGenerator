using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EBC.Core.Helpers.Extensions;

/// <summary>
/// Verilən tətbiq kontekstində avtomatik miqrasiya əməliyyatını həyata keçirən genişləndirici metod.
/// </summary>
public static class AutoMigrate
{
    /// <summary>
    /// Tətbiq başlanarkən verilənlər bazası miqrasiyalarını avtomatik tətbiq edir.
    /// </summary>
    /// <param name="app">Tətbiq obyekti.</param>
    /// <param name="configuration">Konfiqurasiya obyekti.</param>
    /// <returns>Tətbiq obyekti qaytarılır.</returns>
    public static IApplicationBuilder UseAutoMigration(this IApplicationBuilder app, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        // Performans üçün səmərəli konteks yaratmaq və tezmiqrasiyanı tətbiq etmək üçün müstəqil xidmət sahəsi (scope) yaradırıq.
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

        // Null yoxlaması ilə təhlükəsiz miqrasiya əməliyyatı.
        dbContext.Database.Migrate();

        return app;
    }
}
