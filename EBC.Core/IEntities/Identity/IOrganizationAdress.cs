using EBC.Core.IEntities.Common;

namespace EBC.Core.IEntities.Identity;

public interface IOrganizationAdress : IBaseEntity
{
    public string RequestAdress { get; set; }

    public ICollection<IOrganizationAdressRole> OrganizationAdressRoles { get; set; }
}
