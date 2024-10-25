using EBC.Core.IEntities.Common;

namespace EBC.Core.IEntities.Identity;

public interface IUser : IBaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }

    public ICollection<IUserRole> UserRoles { get; set; }
}
