using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Question;

public class QuestionIndexDTO : BaseEntityViewDTO
{
    public string Content { get; set; }

    public string SubjectName { get; set; }
    public string SectionName { get; set; }
    public string QuestionTypeName { get; set; }
    public string QuestionLevelName { get; set; }
    public string GradeName { get; set; }
    public string AcademicYearName { get; set; }
}
