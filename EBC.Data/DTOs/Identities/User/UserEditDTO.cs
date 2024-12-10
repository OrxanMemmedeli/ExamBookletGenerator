using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Identities.User;

public class UserEditDTO : BaseEntityEditDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
}