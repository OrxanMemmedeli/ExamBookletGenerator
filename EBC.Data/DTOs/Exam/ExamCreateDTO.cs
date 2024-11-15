using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Exam;

public class ExamCreateDTO : BaseEntityCreateDTO
{
    public string Name { get; set; }
    public Guid GradeId { get; set; }
    public Guid ExamParameterId { get; set; }
}
