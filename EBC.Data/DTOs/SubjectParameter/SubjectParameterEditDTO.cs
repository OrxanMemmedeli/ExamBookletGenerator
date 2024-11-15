using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.SubjectParameter;

public class SubjectParameterEditDTO : BaseEntityEditDTO
{
    public int QuestionCount { get; set; }
    public int Order { get; set; }
    public Guid? SubjectId { get; set; }
    public Guid ExamParameterId { get; set; }
}
