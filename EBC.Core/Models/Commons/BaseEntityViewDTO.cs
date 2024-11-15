using EBC.Core.Attributes;
using EBC.Core.CustomFilter.WorkFilter.Filters;

namespace EBC.Core.Models.Commons;

public abstract class BaseEntityViewDTO : BaseEntityDTO
{
    [IgnoreValidation]
    public Guid Id { get; set; }
    [IgnoreValidation]
    public DateTime CreatedDate { get; set; }
    [IgnoreValidation]
    public DateTime ModifiedDate { get; set; }

    public FilterOperation CreatedDateFilterType { get; init; } = FilterOperation.Equals;
    public FilterOperation ModifiedDateFilterType { get; init; } = FilterOperation.Equals;
}
