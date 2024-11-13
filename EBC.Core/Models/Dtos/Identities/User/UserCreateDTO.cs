using EBC.Core.Models.Commons;

namespace EBC.Core.Models.Dtos.Identities.User;

public class UserCreateDTO : BaseEntityCreateDTO
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }
}
