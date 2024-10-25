using Microsoft.AspNetCore.Http;
using System.Net;

namespace EBC.Core.Helpers.Extensions;

/// <summary>
/// IP ünvanları ilə işləmək üçün genişləndirici metodlar sinfi.
/// </summary>
public static class IPAdressExtension
{
    /// <summary>
    /// Müştərinin IP ünvanını əldə edir. Əgər X-Forwarded-For başlığı mövcuddursa, ilk ictimai IP ünvanını qaytarır.
    /// </summary>
    /// <param name="context">Http konteksti.</param>
    /// <returns>Müştərinin IP ünvanı və ya "0.0.0.0" əgər əldə etmək mümkün olmasa.</returns>
    public static string GetClientIpAddress(this HttpContext context)
    {
        // Kontekstin null olmaması üçün yoxlama.
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        // Müştərinin uzaq IP ünvanını əldə edir və null olarsa, "0.0.0.0" qaytarır.
        var remoteIpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";

        // X-Forwarded-For başlığı mövcuddursa, burada IP ünvanlarını ayırırıq.
        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var xForwardedForHeader))
        {
            var publicForwardingIps = xForwardedForHeader.ToString().Split(',').Select(ip => ip.Trim()).ToList();

            // Əgər mövcuddursa, sonuncu (ictimai) IP ünvanını qaytarır.
            if (publicForwardingIps.Any())
                return publicForwardingIps.Last();
        }

        // İstifadəçi IP ünvanını yoxlamaq üçün Parse etməyə çalışırıq.
        return IPAddress.TryParse(remoteIpAddress, out _) ? remoteIpAddress : "0.0.0.0";
    }
}
