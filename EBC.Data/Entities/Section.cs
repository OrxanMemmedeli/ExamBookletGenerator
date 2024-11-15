using EBC.Core.Entities.Common;

namespace EBC.Data.Entities;

public class Section : BaseEntity<Guid>
{
    public Section()
    {
        Questions = new HashSet<Question>();
    }
    public string Name { get; set; }



    public Guid SubjectId { get; set; }
    public virtual Subject Subject { get; set; }



    public ICollection<Question> Questions { get; set; }

}
