using EBC.Core.Caching.Abstract;
using Microsoft.Extensions.Caching.Memory;

namespace EBC.Core.Caching.Concrete;

/// <summary>
/// MemoryCachingService sinfi IMemoryCache əsaslı keş xidmətini təmin edir.
/// Bu sinif keş məlumatlarını yazma, oxuma, silmə və bütün keşi təmizləmə əməliyyatlarını həyata keçirir.
/// </summary>
public class MemoryCachingService : ICachingService<IMemoryCache>
{
    private readonly IMemoryCache _cache;

    /// <summary>
    /// Konstruktor. IMemoryCache obyektini qəbul edir və keş xidmətini təmin edir.
    /// </summary>
    /// <param name="cache">IMemoryCache interfeysini implement edən keş obyekti.</param>
    public MemoryCachingService(IMemoryCache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// Verilən açar və dəyəri keşə yazır.
    /// </summary>
    /// <param name="key">Keşdə saxlanılacaq dəyərin açarı.</param>
    /// <param name="value">Keşdə saxlanılacaq obyekt.</param>
    /// <param name="expirationRelativeToNow">Məlumatın nə qədər müddət saxlanılacağını təyin edir.</param>
    public void WriteToCache(string key, object value, TimeSpan expirationRelativeToNow)
        => _cache.Set(key, value, expirationRelativeToNow);

    /// <summary>
    /// Verilən açara əsasən keşdən dəyəri oxuyur.
    /// </summary>
    /// <typeparam name="T">Keşdən oxunan obyektin növü.</typeparam>
    /// <param name="key">Keşdə saxlanılan məlumatın açarı.</param>
    /// <returns>Keşdən oxunan obyekt (T tipli), əgər məlumat tapılmasa, null qaytarılır.</returns>
    public T? ReadFromCache<T>(string key) where T : class
        => _cache.TryGetValue(key, out T? cachedValue) ? cachedValue : null;

    /// <summary>
    /// Verilən açara əsasən keşdəki məlumatı silir.
    /// </summary>
    /// <param name="key">Keşdə saxlanılan məlumatın açarı.</param>
    public void RemoveFromCache(string key)
        => _cache.Remove(key);

    /// <summary>
    /// Bütün keşi təmizləyir.
    /// </summary>
    public void RemoveAllCache()
    {
        if (_cache is MemoryCache memoryCache)
        {
            var percentage = 1.0; // 100%
            memoryCache.Compact(percentage);
        }
    }
}


