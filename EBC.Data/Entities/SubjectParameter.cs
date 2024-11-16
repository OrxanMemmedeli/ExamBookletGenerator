using EBC.Core.Entities.Common;

namespace EBC.Data.Entities;

public class SubjectParameter : BaseEntity<Guid>, IAuditable
{
    public SubjectParameter()
    {
        QuestionParameters = new HashSet<QuestionParameter>();
    }

    public int QuestionCount { get; set; }
    public int Order { get; set; }

    #region Referances

    public Guid? SubjectId { get; set; }
    public Guid ExamParameterId { get; set; }

    public virtual Subject Subject { get; set; }
    public virtual ExamParameter ExamParameter { get; set; }

    #endregion

    public ICollection<QuestionParameter> QuestionParameters { get; set; }

}
