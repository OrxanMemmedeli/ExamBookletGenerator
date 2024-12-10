using EBC.Core.Entities.Common;

namespace EBC.Data.Entities;

public class ExamParameter : AuditableEntity<Guid, EBC.Data.Entities.Identity.User>, IAuditable
{
    public ExamParameter()
    {
        SubjectParameters = new HashSet<SubjectParameter>();
        Exams = new HashSet<Exam>();
    }


    public string Name { get; set; }
    public int SubjectCount { get; set; }
    public string Description { get; set; }

    public ICollection<SubjectParameter> SubjectParameters { get; set; }
    public ICollection<Exam> Exams { get; set; }
}
