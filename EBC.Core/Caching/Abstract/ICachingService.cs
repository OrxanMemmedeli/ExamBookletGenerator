namespace EBC.Core.Caching.Abstract;

/// <summary>
/// ICachingService interfeysi keş əməliyyatları üçün istifadə olunur.
/// Bu interfeys ümumi keş funksiyalarını təmin edir, o cümlədən yazma, oxuma, silmə və bütün keşləri təmizləmə.
/// </summary>
/// <typeparam name="TCache">Keş növü (TCache) hansısa bir keş providerini ifadə edir.</typeparam>
public interface ICachingService<TCache>
{
    /// <summary>
    /// Keşə yeni bir dəyər yazır.
    /// </summary>
    /// <param name="key">Keşdə saxlanılacaq dəyərin açarı.</param>
    /// <param name="value">Keşdə saxlanılacaq obyekt.</param>
    /// <param name="expirationRelativeToNow">Məlumatın nə qədər müddət saxlanılacağını təyin edir.</param>
    void WriteToCache(string key, object value, TimeSpan expirationRelativeToNow);

    /// <summary>
    /// Keşdən açara əsasən dəyər oxuyur.
    /// </summary>
    /// <typeparam name="T">Keşdən oxunan obyektin növü.</typeparam>
    /// <param name="key">Keşdəki məlumatın açarı.</param>
    /// <returns>Tip T olan obyekt qaytarılır. Əgər məlumat tapılmasa, null qaytarılır.</returns>
    T? ReadFromCache<T>(string key) where T : class;

    /// <summary>
    /// Verilən açar üzrə keşi silir.
    /// </summary>
    /// <param name="key">Silinməli olan məlumatın açarı.</param>
    void RemoveFromCache(string key);

    /// <summary>
    /// Bütün keşi təmizləyir.
    /// </summary>
    void RemoveAllCache();
}

