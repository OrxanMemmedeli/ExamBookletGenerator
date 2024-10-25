using EBC.Core.IEntities.Common;

namespace EBC.Core.IEntities.Identity;

public interface IRole : IBaseEntity
{
    public string Name { get; set; }

    public ICollection<IUserRole> UserRoles { get; set; }
    public ICollection<IOrganizationAdressRole> OrganizationAdressRoles { get; set; }
}
