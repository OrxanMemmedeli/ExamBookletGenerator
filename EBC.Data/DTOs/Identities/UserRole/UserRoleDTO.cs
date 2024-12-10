
namespace EBC.Data.DTOs.Identities.UserRole;

public class UserRoleDTO
{
    public Guid[] Checked { get; set; }
    public List<EBC.Data.Entities.Identity.Role> Roles { get; set; }
    public Guid UserId { get; set; }
    public Guid[] FormChecked { get; set; }
}
