using EBC.Core.Entities.Common;

namespace EBC.Data.Entities;

public class Response : AuditableEntity<Guid, EBC.Data.Entities.Identity.User>, IAuditable
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public bool IsTrue { get; set; }


    #region Referanses

    public Guid SubjectId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid QuestionTypeId { get; set; }
    public Guid AcademicYearId { get; set; }

    public virtual AcademicYear AcademicYear { get; set; }
    public virtual Subject Subject { get; set; }
    public virtual Question Question { get; set; }
    public virtual QuestionType QuestionType { get; set; }

    #endregion
}
