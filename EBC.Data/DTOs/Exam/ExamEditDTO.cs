using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Exam;

public class ExamEditDTO : BaseEntityEditDTO
{
    public string Name { get; set; }
    public Guid GradeId { get; set; }
    public Guid ExamParameterId { get; set; }
}
