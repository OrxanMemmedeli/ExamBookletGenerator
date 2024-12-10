using EBC.Core.Entities.Common;

namespace EBC.Data.Entities.Identity;

public class Role : BaseEntity<Guid>
{
    public string Name { get; set; }

    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<OrganizationAdressRole> OrganizationAdressRoles { get; set; }
}
