namespace EBC.Core.CustomFilter.WorkFilter.Filters;

/// <summary>
/// Filtr üçün istifadə olunan əsas məlumatları saxlayan sinif.
/// Bir filtr açarı, əməliyyat tipi və dəyərlərini təyin edir.
/// </summary>
public class FilterData
{
    /// <summary>
    /// Filtr tətbiq olunan obyektin açarı (məsələn, sütun adı).
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Filtr üçün istifadə edilən əməliyyat (məsələn, bərabər, böyükdür və s.).
    /// </summary>
    public FilterOperation Operation { get; set; }

    /// <summary>
    /// Filtr dəyərləri (məsələn, bərabər olduğu və ya arasında olduğu dəyərlər).
    /// </summary>
    public object[] Values { get; set; }

    /// <summary>
    /// FilterData sinifini açar, əməliyyat və dəyərlər ilə başlatır.
    /// </summary>
    /// <param name="key">Obyektin açarı.</param>
    /// <param name="operation">Filtr əməliyyatı.</param>
    /// <param name="values">Dəyərlər.</param>
    public FilterData(string key, FilterOperation operation, params object[] values)
    {
        Key = key;
        Operation = operation;
        Values = values;
    }
}
