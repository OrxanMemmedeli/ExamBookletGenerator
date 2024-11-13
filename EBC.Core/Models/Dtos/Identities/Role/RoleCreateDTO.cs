using EBC.Core.Models.Commons;

namespace EBC.Core.Models.Dtos.Identities.Role;

public class RoleCreateDTO : BaseEntityCreateDTO
{
    public string Name { get; set; }
}
