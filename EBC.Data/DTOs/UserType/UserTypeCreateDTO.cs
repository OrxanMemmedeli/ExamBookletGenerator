using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.UserType;

public class UserTypeCreateDTO : BaseEntityCreateDTO
{
    public string Type { get; set; }
}
