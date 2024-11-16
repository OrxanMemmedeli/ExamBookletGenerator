using EBC.Core.Attributes;
using EBC.Core.CustomFilter.WorkFilter.Filters;

namespace EBC.Core.Models.Commons;

public abstract class BaseEntityViewDTO : BaseEntityDTO
{
    [IgnoreValidation]
    public Guid Id { get; set; }

}
