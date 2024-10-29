using EBC.Core.CustomFilter.WorkFilter.SortingAndPaging;

namespace EBC.Core.CustomFilter.WorkFilter.Filters;

/// <summary>
/// Bir neçə filtr, səhifələmə və sıralama məlumatlarını tək bir modeldə birləşdirən sinif.
/// Filtr məlumatları siyahısı, səhifələmə və sıralama modellərini ehtiva edir.
/// </summary>
public class ViewFilter
{
    /// <summary>
    /// Tətbiq olunacaq filtr modellərini saxlayır.
    /// </summary>
    public List<FilterModel> Filters { get; set; }

    /// <summary>
    /// Səhifələmə məlumatlarını saxlayır.
    /// </summary>
    public PagingModel Paging { get; set; }

    /// <summary>
    /// Sıralama məlumatlarını saxlayır.
    /// </summary>
    public SortingModel Sorting { get; set; }

    /// <summary>
    /// ViewFilter sinifini filtr siyahısı, səhifələmə və sıralama ilə başlatır.
    /// </summary>
    /// <param name="filters">Tətbiq olunacaq filter modelləri.</param>
    /// <param name="paging">Səhifələmə məlumatları.</param>
    /// <param name="sorting">Sıralama məlumatları.</param>
    public ViewFilter(List<FilterModel> filters = null, PagingModel paging = null, SortingModel sorting = null)
    {
        Filters = filters ?? new List<FilterModel>();
        Paging = paging;
        Sorting = sorting;
    }
}
