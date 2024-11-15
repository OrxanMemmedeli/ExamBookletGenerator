using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Question;

public class QuestionEditDTO : BaseEntityEditDTO
{
    public string Content { get; set; }

    public Guid SubjectId { get; set; }
    public Guid SectionId { get; set; }
    public Guid QuestionTypeId { get; set; }
    public Guid QuestionLevelId { get; set; }
    public Guid GradeId { get; set; }
    public Guid AcademicYearId { get; set; }
}
