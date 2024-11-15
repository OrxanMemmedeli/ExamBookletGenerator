using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.QuestionLevel;

public class QuestionLevelCreateDTO : BaseEntityCreateDTO
{
    public string Name { get; set; }
    public short Level { get; set; }
}
