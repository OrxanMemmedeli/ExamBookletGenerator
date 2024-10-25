namespace EBC.Core.Services.Abstract;


/// <summary>
/// JWT tokenləri yaratmaq və doğrulamaq üçün interfeys.
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Verilmiş gizli açar və issuer ilə JWT token yaradır.
    /// </summary>
    /// <param name="secretKey">Token yaratmaq üçün istifadə olunan gizli açar.</param>
    /// <param name="issuer">Token üçün issuer məlumatı.</param>
    /// <returns>Yaradılmış JWT token.</returns>
    string GenerateToken(string secretKey, string issuer);

    /// <summary>
    /// Verilmiş tokenin etibarlılığını yoxlayır.
    /// </summary>
    /// <param name="token">Yoxlanacaq JWT token.</param>
    /// <param name="secretKey">Token doğrulamaq üçün istifadə olunan gizli açar.</param>
    /// <param name="issuer">Token üçün gözlənilən issuer məlumatı.</param>
    /// <returns>Token etibarlıdırsa `true`, yoxsa `false` qaytarır.</returns>
    bool ValidateToken(string token, string secretKey, string issuer);
}
