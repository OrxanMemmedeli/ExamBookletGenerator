using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Response;

public class ResponseIndexDTO : BaseEntityViewDTO
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public bool IsTrue { get; set; }

    public string SubjectName { get; set; }
    public string QuestionName { get; set; }
    public string QuestionTypeName { get; set; }
    public string AcademicYearName { get; set; }
}
