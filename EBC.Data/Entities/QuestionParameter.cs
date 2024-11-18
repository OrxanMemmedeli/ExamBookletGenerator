using EBC.Core.Entities.Common;

namespace EBC.Data.Entities;

public class QuestionParameter : BaseEntity<Guid>, IAuditable
{
    public int StartQuestionNumber { get; set; }
    public int EndQuestionNumber { get; set; }

    public Guid QuestionTypeId { get; set; }
    public Guid SubjectParameterId { get; set; }
    public virtual QuestionType QuestionType { get; set; }
    public virtual SubjectParameter SubjectParameter { get; set; }

}
