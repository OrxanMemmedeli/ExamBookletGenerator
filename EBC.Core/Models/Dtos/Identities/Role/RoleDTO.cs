using EBC.Core.CustomFilter.WorkFilter.Filters;
using EBC.Core.Models.Commons;

namespace EBC.Core.Models.Dtos.Identities.Role;

public class RoleDTO : BaseEntityViewDTO
{
    public string Name { get; set; }

    public FilterOperation NameFilterType { get; init; } = FilterOperation.Contains;

}
