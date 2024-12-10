using EBC.Core.Entities.Common;

namespace EBC.Data.Entities;

public class Grade : AuditableEntity<Guid, EBC.Data.Entities.Identity.User>, IAuditable
{
    public Grade()
    {
        Questions = new HashSet<Question>();
        Exams = new HashSet<Exam>();
        Booklets = new HashSet<Booklet>();
    }

    public string Name { get; set; }

    public ICollection<Question> Questions { get; set; }
    public ICollection<Exam> Exams { get; set; }
    public ICollection<Booklet> Booklets { get; set; }
}
