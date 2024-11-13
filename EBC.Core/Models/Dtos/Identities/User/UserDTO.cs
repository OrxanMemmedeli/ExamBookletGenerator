using EBC.Core.CustomFilter.WorkFilter.Filters;
using EBC.Core.Models.Commons;

namespace EBC.Core.Models.Dtos.Identities.User;

public class UserDTO : BaseEntityViewDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }

    public FilterOperation FirstNameFilterType { get; init; } = FilterOperation.Contains;
    public FilterOperation LastNameFilterType { get; init; } = FilterOperation.Contains;
    public FilterOperation UserNameFilterType { get; init; } = FilterOperation.Equals;

}