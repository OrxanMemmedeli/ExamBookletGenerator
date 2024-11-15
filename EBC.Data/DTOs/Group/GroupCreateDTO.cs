using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Group;

public class GroupCreateDTO : BaseEntityCreateDTO
{
    public string Name { get; set; }
}
