using EBC.Core.Services.Abstract;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace EBC.Core.Services.Concrete;

/// <summary>
/// JWT tokenləri yaratmaq və doğrulamaq üçün xidmət sinfi.
/// </summary>
public class JwtService : IJwtService
{
    /// <summary>
    /// Verilmiş gizli açar və issuer ilə JWT token yaradır.
    /// </summary>
    /// <param name="secretKey">Token yaratmaq üçün istifadə olunan gizli açar.</param>
    /// <param name="issuer">Token üçün issuer məlumatı.</param>
    /// <returns>Yaradılmış JWT token.</returns>
    public string GenerateToken(string secretKey, string issuer)
    {
        // JWT tokenləri idarə etmək üçün bir vasitəçi yaradırıq
        var tokenHandler = new JwtSecurityTokenHandler();

        // Gizli açarı bayt massivinə çeviririk
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);

        // Simmetrik açarı yaratmaq üçün bayt massivindən istifadə edirik
        var securityKey = new SymmetricSecurityKey(keyBytes);

        // Tokeni imzalamaq üçün HmacSha256 alqoritmindən istifadə edərək imzalama məlumatı yaradılır
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Tokenin detalları burada təyin edilir
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            // Tokenin etibarlılıq müddəti 1 saat təyin edilir
            Expires = DateTime.UtcNow.AddHours(1),
            // Issuer məlumatı tokenə əlavə edilir
            Issuer = issuer,
            // Tokenin imzalanması üçün istifadə ediləcək imzalama məlumatı
            SigningCredentials = signingCredentials
        };

        // Token yaradılır
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Tokeni mətn formatında qaytarırıq
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Verilmiş tokenin etibarlılığını yoxlayır.
    /// </summary>
    /// <param name="token">Yoxlanacaq JWT token.</param>
    /// <param name="secretKey">Token doğrulamaq üçün istifadə olunan gizli açar.</param>
    /// <param name="issuer">Token üçün gözlənilən issuer məlumatı.</param>
    /// <returns>Token etibarlıdırsa `true`, yoxsa `false` qaytarır.</returns>
    public bool ValidateToken(string token, string secretKey, string issuer)
    {
        // JWT tokenləri idarə etmək üçün vasitəçi yaradırıq
        var tokenHandler = new JwtSecurityTokenHandler();

        // Gizli açarı bayt massivinə çeviririk
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);

        try
        {
            // Tokenin doğrulama parametrləri burada təyin edilir
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true, // Issuer məlumatını doğrulamağı aktiv edirik
                ValidIssuer = issuer, // Doğrulanacaq issuer məlumatı
                ValidateIssuerSigningKey = true, // İmzalama açarının doğrulanmasını aktiv edirik
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes), // İmzalama açarı
                ValidateLifetime = true, // Tokenin etibarlılıq müddətini doğrulamağı aktiv edirik
                ValidateAudience = false, // Audiencə doğrulaması deaktivdir
                ClockSkew = TimeSpan.Zero // Saat fərqini sıfır olaraq təyin edirik
            }, out _);

            // Doğrulama uğurludursa, `true` qaytarırıq
            return true;
        }
        catch (Exception)
        {
            // Doğrulama uğursuz olarsa, `false` qaytarırıq
            return false;
        }
    }
}


