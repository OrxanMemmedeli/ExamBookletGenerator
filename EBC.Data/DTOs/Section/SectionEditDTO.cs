using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Section;

public class SectionEditDTO : BaseEntityEditDTO
{
    public string Name { get; set; }

    public Guid SubjectId { get; set; }
}
