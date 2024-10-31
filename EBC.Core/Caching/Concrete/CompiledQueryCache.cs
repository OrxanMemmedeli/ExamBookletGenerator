using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EBC.Core.Caching.Concrete;

/// <summary>
/// LINQ sorğularını kompilyasiya edib cache-də saxlayan köməkçi sinif.
/// Bu sinif spesifik sorğular üçün kompilyasiya olunmuş sorğuları saxlayır və 
/// sorğunun təkrar icrası zamanı performansı artırmaq məqsədi ilə həmin sorğuları birbaşa cache-dən istifadə edir.
/// </summary>
/// <typeparam name="TEntity">İşləyəcək entity növü.</typeparam>
public static class CompiledQueryCache<TEntity> where TEntity : class
{
    /// <summary>
    /// Kompilyasiya olunmuş sorğuları saxlamaq üçün bir `Dictionary`.
    /// Hər bir sorğu üçün unikal açar olaraq istifadə edilən `queryKey`, 
    /// sorğunun kompilyasiya olunmuş versiyasını saxlayır və təkrar istifadəni təmin edir.
    /// </summary>
    public static readonly Dictionary<string, Func<DbContext, IEnumerable<TEntity>>> Queries = new();

    /// <summary>
    /// Əgər sorğu `queryKey` açarına uyğun olaraq daha əvvəl kompilyasiya edilməyibsə, 
    /// sorğunu kompilyasiya edir və cache-ə əlavə edir, əks halda mövcud kompilyasiya olunmuş sorğunu qaytarır.
    /// </summary>
    /// <param name="queryKey">Sorğunun unikal açarı.</param>
    /// <param name="queryExpr">Kompilyasiya olunacaq sorğu ifadəsi.</param>
    /// <returns>Kompilyasiya olunmuş sorğu.</returns>
    /// <exception cref="ArgumentNullException">Sorğu ifadəsi null olduqda atılır.</exception>
    public static Func<DbContext, IEnumerable<TEntity>> GetOrAddCompiledQuery(string queryKey, Expression<Func<DbContext, IQueryable<TEntity>>> queryExpr)
    {
        if (queryExpr == null) throw new ArgumentNullException(nameof(queryExpr));

        if (!Queries.ContainsKey(queryKey)) // Sorğu daha əvvəl kompilyasiya edilməyibsə, onu kompilyasiya edir və cache-ə əlavə edir
            Queries.Add(queryKey, EF.CompileQuery(queryExpr)); 

        // Kompilyasiya olunmuş sorğunu qaytarır
        return Queries[queryKey];
    }

    /// <summary>
    /// Əgər sorğu `queryKey` açarına uyğun olaraq daha əvvəl kompilyasiya edilməyibsə, 
    /// sorğunu kompilyasiya edir və cache-ə əlavə edir, əks halda mövcud kompilyasiya olunmuş sorğunu qaytarır.
    /// </summary>
    /// <param name="queryKey">Sorğunun unikal açarı.</param>
    /// <param name="queryExpr">IEnumerable nəticələr üçün nəzərdə tutulub.</param>
    /// <returns>Kompilyasiya olunmuş sorğu.</returns>
    /// <exception cref="ArgumentNullException">Sorğu ifadəsi null olduqda atılır.</exception>
    public static Func<DbContext, IEnumerable<TEntity>> GetOrAddCompiledQuery(string queryKey, Expression<Func<DbContext, IEnumerable<TEntity>>> queryExpr)
    {
        if (queryExpr == null) throw new ArgumentNullException(nameof(queryExpr));

        if (!Queries.ContainsKey(queryKey)) // Sorğu daha əvvəl kompilyasiya edilməyibsə, onu kompilyasiya edir və cache-ə əlavə edir
            Queries.Add(queryKey, EF.CompileQuery(queryExpr));

        // Kompilyasiya olunmuş sorğunu qaytarır
        return Queries[queryKey];
    }

    /// <summary>
    /// Cache-də saxlanılan bütün kompilyasiya olunmuş sorğuları silir.
    /// Bu metod bütün sorğuların yaddaşdan təmizlənməsini və növbəti istifadə zamanı yenidən kompilyasiya olunmasını təmin edir.
    /// </summary>
    public static void ClearAllQueries() => Queries.Clear();

    /// <summary>
    /// Verilmiş `queryKey`-ə uyğun olan yalnız bir kompilyasiya olunmuş sorğunu cache-dən çıxarır.
    /// Bu metod müəyyən bir sorğunun təkrar kompilyasiya edilməsi lazım olduqda istifadə edilir.
    /// </summary>
    /// <param name="queryKey">Silinəcək sorğunun açarı.</param>
    public static void ClearQuery(string queryKey) => Queries.Remove(queryKey);
}