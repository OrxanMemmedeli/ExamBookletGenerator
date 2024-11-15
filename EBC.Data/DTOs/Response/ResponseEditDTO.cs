using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Response;

public class ResponseEditDTO : BaseEntityEditDTO
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public bool IsTrue { get; set; }

    public Guid SubjectId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid QuestionTypeId { get; set; }
    public Guid AcademicYearId { get; set; }
}
