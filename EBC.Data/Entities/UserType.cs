using EBC.Core.Entities.Common;

namespace EBC.Data.Entities;

public class UserType : BaseEntity<Guid>
{
    public UserType()
    {
        AppUsers = new HashSet<AppUser>();
    }

    public string Type { get; set; }

    public ICollection<AppUser> AppUsers { get; set; }

}
