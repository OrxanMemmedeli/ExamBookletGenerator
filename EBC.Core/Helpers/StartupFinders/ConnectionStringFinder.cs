using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace EBC.Core.Helpers.StartupFinders;

/// <summary>
/// ConnectionString məlumatının json üzərindən dinamik olaraq tapılması
/// </summary>
public static class ConnectionStringFinder
{
    /// <summary>
    /// Əlaqə sətirini mühit və maşın adına görə əldə edir.
    /// </summary>
    /// <param name="configuration">Tətbiq konfiqurasiyası.</param>
    /// <returns>Əlaqə sətiri.</returns>
    public static string GetConnectionString(IConfiguration configuration)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        var machineName = Environment.MachineName;

        // Machine adlarını əldə edir, əgər `MachineNames` kəsiyində varsa
        var machineNames = configuration.GetSection("MachineNames").Get<string[]>() ?? Array.Empty<string>();

        // Əgər cari maşın adı konfiqurasiya edilmiş siyahıda yoxdursa, `_` konfiqurasiyasını götür
        var configKey = machineNames.Contains(machineName) ? $"{environment}:{machineName}" : $"{environment}:_";

        var connectionString = configuration[configKey];
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string not found in the configuration.");

        return connectionString;
    }

    /// <summary>
    /// Machine adını JSON faylına əlavə edir (əgər mövcud deyilsə).
    /// </summary>
    /// <param name="newMachineName">Əlavə ediləcək maşın adı.</param>
    /// <param name="machineNames">Hazırki maşın adları massivi.</param>
    /// <param name="configFilePath">JSON konfiqurasiya faylının tam yolu.</param>
    public static void SetNewMachineName(string newMachineName, string[] machineNames, string configFilePath)
    {
        //var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"); Layihə tərəfindən göndərilir. 
        if (!machineNames.Contains(newMachineName))
        {
            if (!File.Exists(configFilePath))
                throw new FileNotFoundException($"The configuration file at '{configFilePath}' could not be found.");

            var json = File.ReadAllText(configFilePath);
            var jsonObj = JsonDocument.Parse(json).RootElement.Clone();

            // Yeni maşın adını `MachineNames` siyahısına əlavə edir
            var updatedMachines = machineNames.Append(newMachineName).ToArray();

            var newJson = JsonSerializer.Serialize(new
            {
                MachineNames = updatedMachines
            }, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(configFilePath, newJson);
        }
    }
}
