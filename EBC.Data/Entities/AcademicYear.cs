using EBC.Core.Entities.Common;

namespace EBC.Data.Entities;
public class AcademicYear : BaseEntity<Guid>
{
    public AcademicYear()
    {
        Questions = new HashSet<Question>();
        Responses = new HashSet<Response>();
        Booklets = new HashSet<Booklet>();
    }

    public string Name { get; set; }

    #region Collections
    public ICollection<Question> Questions { get; set; }
    public ICollection<Response> Responses { get; set; }
    public ICollection<Booklet> Booklets { get; set; }
    #endregion

}

