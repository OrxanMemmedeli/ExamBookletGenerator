using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Identities.Role;

public class RoleCreateDTO : BaseEntityCreateDTO
{
    public string Name { get; set; }
}
