using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.QuestionLevel;

public class QuestionLevelViewDTO : BaseEntityViewDTO
{
    public string Name { get; set; }
    public short Level { get; set; }
}
