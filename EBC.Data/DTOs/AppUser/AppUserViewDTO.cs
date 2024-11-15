using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.AppUser;

public class AppUserViewDTO : BaseEntityViewDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }

    public bool ImagePath { get; set; }

    public string UserTypeName { get; set; }
}
