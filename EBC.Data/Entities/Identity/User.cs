using EBC.Core.Entities.Common;

namespace EBC.Data.Entities.Identity;

public class User : BaseEntity<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }

    public ICollection<UserRole> UserRoles { get; set; }
}
