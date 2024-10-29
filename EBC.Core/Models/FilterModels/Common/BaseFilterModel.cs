using EBC.Core.CustomFilter.WorkFilter.Filters;

namespace EBC.Core.Models.FilterModels.Common;

public class BaseFilterModel
{
    public bool? Status { get; set; } = null;
    public DateTime? CreatedDate { get; set; } = null;
    public DateTime? ModifiedDate { get; set; } = null;

    public FilterOperation StatusFilterType { get; init; } = FilterOperation.Equals;
    public FilterOperation CreatedDateFilterType { get; init; } = FilterOperation.Equals;
    public FilterOperation ModifiedDateFilterType { get; init; } = FilterOperation.Equals;
}
