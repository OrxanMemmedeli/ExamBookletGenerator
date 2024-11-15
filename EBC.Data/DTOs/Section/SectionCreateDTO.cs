using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Section;

public class SectionCreateDTO : BaseEntityCreateDTO
{
    public string Name { get; set; }

    public Guid SubjectId { get; set; }
}
