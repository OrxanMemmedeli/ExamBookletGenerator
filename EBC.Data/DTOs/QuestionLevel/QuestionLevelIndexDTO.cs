using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.QuestionLevel;

public class QuestionLevelIndexDTO : BaseEntityViewDTO
{
    public string Name { get; set; }
    public short Level { get; set; }
}
