using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.ExamParameter;

public class ExamParameterEditDTO : BaseEntityEditDTO
{
    public string Name { get; set; }
    public int SubjectCount { get; set; }
    public string Description { get; set; }
}
