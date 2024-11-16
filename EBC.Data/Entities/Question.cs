using EBC.Core.Entities.Common;
using EBC.Data.Entities.CombineEntities;

namespace EBC.Data.Entities;

public class Question : BaseEntity<Guid>, IAuditable
{
    public Question()
    {
        Responses = new HashSet<Response>();
        QuestionAttahments = new HashSet<QuestionAttahment>();
    }

    public string Content { get; set; }

    #region referances 

    public Guid SubjectId { get; set; }
    public virtual Subject Subject { get; set; }

    public Guid SectionId { get; set; }
    public virtual Section Section { get; set; }

    public Guid QuestionTypeId { get; set; }
    public virtual QuestionType QuestionType { get; set; }

    public Guid QuestionLevelId { get; set; }
    public virtual QuestionLevel QuestionLevel { get; set; }

    public Guid GradeId { get; set; }
    public virtual Grade Grade { get; set; }

    public Guid? TextId { get; set; }
    public virtual Text Text { get; set; }

    public Guid AcademicYearId { get; set; }
    public virtual AcademicYear AcademicYear { get; set; }

    #endregion


    #region Collections  

    public ICollection<Response> Responses { get; set; }
    public ICollection<QuestionAttahment> QuestionAttahments { get; set; }

    #endregion
}
