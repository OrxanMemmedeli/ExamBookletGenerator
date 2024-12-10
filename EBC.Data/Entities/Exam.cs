using EBC.Core.Entities.Common;

namespace EBC.Data.Entities;

public class Exam : AuditableEntity<Guid, EBC.Data.Entities.Identity.User>, IAuditable
{
    public Exam()
    {
        Booklets = new HashSet<Booklet>();

    }
    public string Name { get; set; }

    #region referances 

    public Guid GradeId { get; set; }
    public Guid ExamParameterId { get; set; }


    public virtual Grade Grade { get; set; }
    public virtual ExamParameter ExamParameter { get; set; }

    #endregion

    #region collections

    public ICollection<Booklet> Booklets { get; set; }

    #endregion
}
