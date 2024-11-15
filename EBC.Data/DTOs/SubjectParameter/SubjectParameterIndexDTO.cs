using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.SubjectParameter;

public class SubjectParameterIndexDTO : BaseEntityViewDTO
{
    public int QuestionCount { get; set; }
    public int Order { get; set; }
    public string SubjectName { get; set; }
    public string ExamParameterName { get; set; }
}
