using EBC.Core.Entities.Common;

namespace EBC.Core.Entities.Identity;

public class OrganizationAdress : BaseEntity<Guid>
{
    public string RequestAdress { get; set; }

    public ICollection<OrganizationAdressRole> OrganizationAdressRoles { get; set; }
}
