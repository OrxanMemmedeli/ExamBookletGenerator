using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.User;

public class UserViewDTO : BaseEntityViewDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }

    public bool ImagePath { get; set; }

    public string UserTypeName { get; set; }
}
