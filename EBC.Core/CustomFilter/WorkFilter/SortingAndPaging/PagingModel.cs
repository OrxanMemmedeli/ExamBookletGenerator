namespace EBC.Core.CustomFilter.WorkFilter.SortingAndPaging;

/// <summary>
/// Səhifələmə üçün lazımi məlumatları təmin edən model.
/// </summary>
public class PagingModel
{
    /// <summary>
    /// Cari səhifənin nömrəsi (default olaraq 1).
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Hər səhifədə göstəriləcək elementlərin sayı (default olaraq 10).
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Ümumi elementlərin sayı.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Ümumi səhifə sayı (hesablanır).
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>
    /// Səhifədə göstəriləcək elementlərin sayını müəyyən edən konstruktur.
    /// </summary>
    /// <param name="pageNumber">Cari səhifə nömrəsi.</param>
    /// <param name="pageSize">Hər səhifədə göstəriləcək elementlərin sayı.</param>
    public PagingModel(int pageNumber = 1, int pageSize = 10)
    {
        PageNumber = pageNumber > 0 ? pageNumber : 1;
        PageSize = pageSize > 0 ? pageSize : 10;
    }

    /// <summary>
    /// Səhifə başlanğıcındakı ilk elementin nömrəsini qaytarır.
    /// </summary>
    public int? Skip => (PageNumber - 1) * PageSize;

    /// <summary>
    /// Göstəriləcək maksimum element sayını təyin edir.
    /// </summary>
    public int? Take => PageSize;
}
