using EBC.Core.Entities.Common;
using EBC.Data.Entities.Identity;

namespace EBC.Data.Entities;

public class UserType : BaseEntity<Guid>
{
    public UserType()
    {
        Users = new HashSet<User>();
    }

    public string Type { get; set; }

    public ICollection<User> Users { get; set; }

}
