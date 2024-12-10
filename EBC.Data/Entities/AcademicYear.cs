using EBC.Core.Entities.Common;
using EBC.Data.Entities.Identity;

namespace EBC.Data.Entities;
public class AcademicYear : AuditableEntity<Guid, User>, IAuditable
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

