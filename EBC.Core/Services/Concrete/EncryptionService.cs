using EBC.Core.Constants;
using EBC.Core.Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace EBC.Core.Services.Concrete;

/// <summary>
/// EncryptionService sinfi məlumatların şifrələnməsi və deşifrələnməsi üçün istifadə olunur.
/// Varsayılan bir açar və ya istifadəçinin təyin etdiyi bir açar ilə məlumatları AES və ya SHA256 alqoritmləri ilə şifrələyir.
/// </summary>
public static class EncryptionService
{
    private const string DefaultKey = "T)=~3g6FVW+h=X}";
    private static readonly byte[] _defaultKeyBytes = ComputeHash(DefaultKey);

    /// <summary>
    /// Verilən string dəyəri SHA256 ilə hash edir və nəticəni byte[] formatında qaytarır.
    /// </summary>
    /// <param name="input">Hash etmək üçün istifadə olunan string dəyər.</param>
    /// <returns>SHA256 hash-inin nəticəsi byte[] formatında qaytarılır.</returns>
    private static byte[] ComputeHash(string input)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
    }

    /// <summary>
    /// Məlumatı şifrələyir. Əgər açar təyin edilməyibsə, varsayılan açar istifadə olunur.
    /// </summary>
    /// <param name="data">Şifrələnəcək məlumat (string formatında).</param>
    /// <param name="key">Şifrələmə açarı (opsional).</param>
    /// <returns>Şifrələnmiş məlumat base64 formatında qaytarılır.</returns>
    public static string Encrypt(string data, string? key = null)
    {
        var keyBytes = string.IsNullOrEmpty(key) ? _defaultKeyBytes : ComputeHash(key);
        return TransformData(data, keyBytes, true);
    }

    /// <summary>
    /// Şifrələnmiş məlumatı deşifrə edir. Əgər açar təyin edilməyibsə, varsayılan açar istifadə olunur.
    /// </summary>
    /// <param name="encryptedData">Deşifrələnəcək məlumat (string formatında, base64 kodlu).</param>
    /// <param name="key">Deşifrələmə açarı (opsional).</param>
    /// <returns>Deşifrələnmiş məlumat (string formatında).</returns>
    public static string Decrypt(string encryptedData, string? key = null)
    {
        var keyBytes = string.IsNullOrEmpty(key) ? _defaultKeyBytes : ComputeHash(key);
        return TransformData(encryptedData, keyBytes, false);
    }

    /// <summary>
    /// Məlumatı şifrələyən və ya deşifrələyən əsas metod. Verilən məlumatı şifrələyir və ya deşifrə edir.
    /// </summary>
    /// <param name="data">Şifrələnəcək və ya deşifrələnəcək məlumat (string formatında).</param>
    /// <param name="key">Şifrələmə və ya deşifrələmə üçün istifadə olunan açar (byte[] formatında).</param>
    /// <param name="encrypt">Əgər true olarsa, məlumat şifrələnir; false olarsa, məlumat deşifrələnir.</param>
    /// <returns>Əməliyyatın nəticəsi string formatında qaytarılır (base64 formatında).</returns>
    private static string TransformData(string data, byte[] key, bool encrypt)
    {
        using var aes = Aes.Create();
        aes.Key = key.Take(32).ToArray(); // AES-256 üçün 32 bayt uzunluğunda açar tələb olunur
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.GenerateIV(); // Initialization Vector (IV) yaradılır

        byte[] resultBytes;
        byte[] dataBytes = encrypt ? Encoding.UTF8.GetBytes(data) : Convert.FromBase64String(data);

        using var ms = new MemoryStream();
        if (encrypt)
            ms.Write(aes.IV, 0, aes.IV.Length); // Şifrələnmiş məlumatın başında IV saxlanılır

        using (var cryptoStream = new CryptoStream(ms, encrypt ? aes.CreateEncryptor(aes.Key, aes.IV) : aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
        {
            cryptoStream.Write(dataBytes, 0, dataBytes.Length);
            cryptoStream.FlushFinalBlock();
        }

        resultBytes = ms.ToArray();

        return encrypt ? Convert.ToBase64String(resultBytes) : Encoding.UTF8.GetString(resultBytes.Skip(aes.IV.Length).ToArray());
    }
}