using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.QuestionParameter;

public class QuestionParameterCreateDTO : BaseEntityCreateDTO
{
    public Guid QuestionTypeId { get; set; }
    public Guid SubjectParameterId { get; set; }
    public int StartQuestionNumber { get; set; }
    public int EndQuestionNumber { get; set; }
}
