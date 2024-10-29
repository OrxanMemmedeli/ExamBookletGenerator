namespace EBC.Core.CustomFilter.WorkFilter.Filters;

/// <summary>
/// Filtr modelini təyin edir, bir və ya bir neçə filtr məlumatı və əlaqə növlərini saxlayır.
/// </summary>
public class FilterModel
{
    /// <summary>
    /// Əsas filtr məlumatları, `FilterData` obyektində saxlanılır.
    /// </summary>
    public FilterData FilterData { get; set; }

    /// <summary>
    /// Bu filtr ilə növbəti filtr arasında olan şərt (And/Or).
    /// </summary>
    public FilterPartRelations Condition { get; set; }

    /// <summary>
    /// Əgər bir neçə filtr qrup şəklində istifadə olunursa, əlavə filtr modellərini saxlayır.
    /// </summary>
    public List<FilterModel> FilterGroup { get; set; }

    /// <summary>
    /// FilterModel sinifini `FilterData`, əlaqə şərti və filtr qrupu ilə başlatır.
    /// </summary>
    /// <param name="filterData">Əsas filtr məlumatları.</param>
    /// <param name="condition">Filtr əlaqə şərti.</param>
    /// <param name="filterGroup">Filtr qrupunun siyahısı.</param>
    public FilterModel(FilterData filterData, FilterPartRelations condition, List<FilterModel> filterGroup = null)
    {
        FilterData = filterData;
        Condition = condition;
        FilterGroup = filterGroup ?? new List<FilterModel>();
    }
}
