using EBC.Core.Models.Commons;

namespace EBC.Core.Models.Dtos.Identities.User;

public class UserEditDTO : BaseEntityEditDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
}