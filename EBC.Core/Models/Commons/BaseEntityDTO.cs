using EBC.Core.Attributes;
using EBC.Core.CustomFilter.WorkFilter.Filters;

namespace EBC.Core.Models.Commons;

public abstract class BaseEntityDTO
{
    [IgnoreValidation]
    public bool Status { get; set; }

    [IgnoreValidation]
    public bool IsDeleted { get; set; }

    [IgnoreValidation]
    public DateTime CreatedDate { get; set; }

    [IgnoreValidation]
    public DateTime ModifiedDate { get; set; }


    public FilterOperation CreatedDateFilterType { get; init; } = FilterOperation.Equals;
    public FilterOperation ModifiedDateFilterType { get; init; } = FilterOperation.Equals;
    public FilterOperation StatusFilterType { get; init; } = FilterOperation.Equals;
    public FilterOperation IsDeletedFilterType { get; init; } = FilterOperation.Equals;
}
