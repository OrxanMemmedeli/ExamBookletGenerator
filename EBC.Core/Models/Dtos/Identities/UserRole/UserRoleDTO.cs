
namespace EBC.Core.Models.Dtos.Identities.UserRole;

public class UserRoleDTO
{
    public Guid[] Checked { get; set; }
    public List<EBC.Core.Entities.Identity.Role> Roles { get; set; }
    public Guid UserId { get; set; }
    public Guid[] FormChecked { get; set; }
}
