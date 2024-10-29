namespace EBC.Core.CustomFilter.WorkFilter.SortingAndPaging;

/// <summary>
/// Sorğu üçün sıralama konfiqurasiyasını təmsil edir.
/// </summary>
public class SortingModel
{
    /// <summary>
    /// Hansı sahəyə görə sıralama aparılacağını təyin edir.
    /// </summary>
    public string SortBy { get; set; }

    /// <summary>
    /// Sıralamanın istiqamətini təyin edir (Ascending və ya Descending).
    /// </summary>
    public SortDirection Direction { get; set; } = SortDirection.Ascending;
}